
using Models.Infrastructure;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SignalRWebpack.Services;

public class BroadcastingService
{
	public BroadcastingService()
	{
	}
	private readonly List<WebSocket> _clients = new();
	private readonly CancellationTokenSource _cancellationTokenSource = new();

	public void Start()
	{
		Task.Run(async () => await BroadcastLoop());
	}

	public async Task HandleWebSocketConnection(HttpContext context)
	{
		if (!context.WebSockets.IsWebSocketRequest)
		{
			context.Response.StatusCode = 400;
			return;
		}

		using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
		_clients.Add(webSocket);
		Console.WriteLine("Client connected.");

		var buffer = new byte[1024 * 4];
		try
		{
			while (webSocket.State == WebSocketState.Open)
			{
				var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
				if (result.MessageType == WebSocketMessageType.Close)
				{
					_clients.Remove(webSocket);
					await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
					Console.WriteLine("Client disconnected.");
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"WebSocket error: {ex.Message}");
		}
	}

	private async Task BroadcastLoop()
	{
		var _dataCollectionService = new DataCollectorService();

		while (!_cancellationTokenSource.Token.IsCancellationRequested)
		{
			await _dataCollectionService.GetDataLoop();
			await Task.Delay(TimeSpan.FromSeconds(10));


			var data = await _dataCollectionService.CollectDataAsync();
			var jsonData = JsonSerializer.Serialize(data);
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
}
