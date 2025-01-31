using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models.Entities;
using Models.Infrastructure;

namespace SignalRWebpack.Services
{
	public class DataCollectionService : BackgroundService
	{
		private readonly IServiceScopeFactory _scopeFactory;
		private readonly ILogger<DataCollectionService> _logger;

		public DataCollectionService(IServiceScopeFactory scopeFactory, ILogger<DataCollectionService> logger)
		{
			_scopeFactory = scopeFactory;
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Data collection service started.");
			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					await GetDataLoop();
				}
				catch (Exception ex)
				{
					_logger.LogError($"Error in GetDataLoop: {ex.Message}");
				}
				await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
			}
		}

		private async Task GetDataLoop()
		{
			using var scope = _scopeFactory.CreateScope();
			var _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
			_logger.LogInformation("GetDataLoop started.");

			var centers = await _dbContext.Centers
				.Include(current => current.Modules)
				.Where(current => !current.IsDeleted && !string.IsNullOrEmpty(current.IPAddress) && !string.IsNullOrEmpty(current.Port))
				.ToListAsync();

			var tasks = centers.Select(center => getData(center)).ToList();
			await Task.WhenAll(tasks);
			await _dbContext.SaveChangesAsync();
		}

		private async Task getData(Center center)
		{
			try
			{
				using var client = new TcpClient();
				await client.ConnectAsync(center.IPAddress, int.Parse(center.Port));
				using var stream = client.GetStream();
				using var reader = new StreamReader(stream);

				char[] buffer = new char[256];
				int bytesRead = await reader.ReadAsync(buffer, 0, buffer.Length);
				var response = new string(buffer, 0, bytesRead);

				if (string.IsNullOrWhiteSpace(response)) return;

				var jsonData = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, double>>>(response);
				await ProcessSensorData(center, jsonData);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error retrieving data from {center.IPAddress}:{center.Port} {ex.Message}");
			}
		}

		private async Task ProcessSensorData(Center center, Dictionary<string, Dictionary<string, double>> jsonData)
		{
			using var scope = _scopeFactory.CreateScope();
			var _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

			var temperatureData = jsonData?.GetValueOrDefault("temperature") ?? new Dictionary<string, double>();
			var humidityData = jsonData?.GetValueOrDefault("humidity") ?? new Dictionary<string, double>();

			foreach (var (key, value) in temperatureData.Concat(humidityData))
			{
				var moduleNumber = int.Parse(key);
				var module = center.Modules.FirstOrDefault(m => m.ModuleNumber == moduleNumber);
				if (module == null) continue;

				var isTemperature = temperatureData.ContainsKey(key);
				module.IsTemp = isTemperature;

				var moduleData = new ModuleData(Guid.Empty, module.Id, value);
				_dbContext.ModuleDatas.Add(moduleData);
			}

			await _dbContext.SaveChangesAsync();
		}
	}
}
