using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Models.Infrastructure;

namespace SignalRWebpack.Services;

public class BroadcastingService
{
	private readonly ConcurrentBag<WebSocket> _clients = new();
	private readonly CancellationTokenSource _cancellationTokenSource = new();
	private readonly ILogger<BroadcastingService> _logger;
	private readonly IServiceScopeFactory _scopeFactory;

	public BroadcastingService(IServiceScopeFactory scopeFactory, ILogger<BroadcastingService> logger)
	{
		_scopeFactory = scopeFactory;
		_logger = logger;
	}

	public void Start()
	{
		_logger.LogInformation("Starting background tasks...");
		Task.Run(async () => await BroadcastLoop());
	}

	public async Task HandleWebSocketConnection(HttpContext context)
	{
		if (!context.WebSockets.IsWebSocketRequest)
		{
			context.Response.StatusCode = 400;
			return;
		}

		var webSocket = await context.WebSockets.AcceptWebSocketAsync();
		_clients.Add(webSocket);
		_logger.LogInformation("Client connected.");

		var buffer = new byte[1024 * 4];
		try
		{
			while (webSocket.State == WebSocketState.Open)
			{
				var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
				if (result.MessageType == WebSocketMessageType.Close)
				{
					_clients.TryTake(out _);
					await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
					_logger.LogInformation("Client disconnected.");
				}
			}
		}
		catch (Exception ex)
		{
			_logger.LogError($"WebSocket error: {ex.Message}");
		}
	}

	private async Task BroadcastLoop()
	{
		_logger.LogInformation("Starting BroadcastLoop...");
		while (!_cancellationTokenSource.Token.IsCancellationRequested)
		{
			var data = await CollectDataAsync();
			var jsonData = JsonSerializer.Serialize(data);
			_logger.LogInformation($"Broadcasting to {_clients.Count} clients...");

			foreach (var socket in _clients.ToList())
			{
				if (socket.State == WebSocketState.Open)
				{
					var message = Encoding.UTF8.GetBytes(jsonData);
					await socket.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Text, true, CancellationToken.None);
				}
			}

			await Task.Delay(TimeSpan.FromSeconds(20));
		}
	}

	public async Task<List<CenterViewModel>> CollectDataAsync()
	{
		try
		{
			using var scope = _scopeFactory.CreateScope();
			var _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

			var moduleData = await _dbContext.ModuleDatas
				.Include(current => current.Module)
					.ThenInclude(current => current.Center)
				.Where(current => !current.IsDeleted)
				.GroupBy(current => current.ModuleId)
				.Select(g => g.OrderByDescending(x => x.DateAdded).FirstOrDefault())
				.ToListAsync();

			var centerViewModel = await _dbContext.Centers
				.Where(current => !current.IsDeleted)
				.Select(current => new CenterViewModel
				{
					Id = current.Id,
					CenterName = current.Name,
					IpAddress = current.IPAddress,
					Port = current.Port
				})
				.OrderBy(current=>current.CenterName)
				.ToListAsync();

			foreach (var center in centerViewModel)
			{
				var centerSensors = await _dbContext.ModuleDatas
					.Include(current => current.Module)
						.ThenInclude(current => current.Center)
					.Where(current => !current.IsDeleted && current.Module.CenterId == center.Id)
					.GroupBy(current => current.ModuleId)
					.Select(g => g.OrderByDescending(x => x.DateAdded).FirstOrDefault())
					.ToListAsync();

				center.Sensors = centerSensors
					.Select(current => new SensorViewModel
					{
						Name = current.Module.ModuleName,
						ModuleNumber=current.Module.ModuleNumber,
						Type = current.Module.IsTemp ? "Temperature" : "Humidity",
						Value = current.Value,
						HasAlarm = current.Value < current.Module.Minimum || current.Value > current.Module.Maximum
					})
					.OrderBy(current=>current.ModuleNumber)
					.ToList();
			}

			return centerViewModel;
		}
		catch (Exception ex)
		{
			_logger.LogError($"Error in CollectDataAsync: {ex.Message}");
			throw;
		}
	}
}
