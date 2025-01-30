using System.Net.Sockets;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Models.Infrastructure;

public class TcpDataCollectorService : ITcpDataCollectorService
{
	private readonly DatabaseContext _dbContext;

	public TcpDataCollectorService(DatabaseContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<List<CenterViewModel>> CollectDataAsync()
	{
		try
		{
			var centers = await _dbContext.Centers
				.Include(current => current.Modules)
				.Where(current => !current.IsDeleted
				&& !string.IsNullOrEmpty(current.IPAddress)
				&& !string.IsNullOrEmpty(current.Port)
				)
				.ToListAsync();

			var collectedDataList = new List<CenterViewModel>();

			foreach (var center in centers)
			{
				try
				{
					var centerResponse = new CenterViewModel
					{
						Port = center.Port,
						CenterName = center.Name,
						IpAddress = center.IPAddress,
						Sensors = new List<SensorViewModel>()
					};

					var client = new TcpClient();
					await client.ConnectAsync(center.IPAddress, int.Parse(center.Port));

					var stream = client.GetStream();
					var reader = new StreamReader(stream);

					char[] buffer = new char[256];
					int bytesRead = await reader.ReadAsync(buffer, 0, buffer.Length);

					var response = new string(buffer, 0, bytesRead);

					centerResponse = proccessData(response, centerResponse, center);

					if (centerResponse == null)
						continue;

					collectedDataList.Add(centerResponse);
				}
				catch (Exception)
				{
					continue;
				}
			}

			await _dbContext.SaveChangesAsync();

			return collectedDataList;
		}
		catch (Exception)
		{

			throw;
		}
	}

	private CenterViewModel? proccessData(string response, CenterViewModel centerResponse, Center center)
	{

		if (string.IsNullOrWhiteSpace(response))
			return null;

		var jsonData = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, double>>>(response);

		var temperatureData = jsonData?.GetValueOrDefault("temperature") ?? new Dictionary<string, double>();
		foreach (var temperature in temperatureData)
		{
			var moduleNumber = int.Parse(temperature.Key);
			var foundedModule = center.Modules
				.Where(current => current.ModuleNumber == moduleNumber)
				.FirstOrDefault();

			if (foundedModule == null)
				continue;

			if (!foundedModule.IsTemp)
				foundedModule.IsTemp = true;

			var moduleData = new ModuleData(Guid.Empty, foundedModule.Id, temperature.Value);

			var sensorData = new SensorViewModel
			{
				HasAlarm = false,
				Type = "Temperature",
				Value = temperature.Value,
				Name = foundedModule.ModuleName,
			};
			if (foundedModule.Minimum > temperature.Value || foundedModule.Maximum < temperature.Value)
				sensorData.HasAlarm = true;

			centerResponse.Sensors.Add(sensorData);
			_dbContext.ModuleDatas.Add(moduleData);
		}

		var humidityData = jsonData?.GetValueOrDefault("humidity") ?? new Dictionary<string, double>();

		foreach (var humidity in humidityData)
		{
			var moduleNumber = int.Parse(humidity.Key);
			var foundedModule = center.Modules
				.Where(current => current.ModuleNumber == moduleNumber)
				.FirstOrDefault();

			if (foundedModule == null)
				continue;

			if (!foundedModule.IsTemp)
				foundedModule.IsTemp = true;

			var moduleData = new ModuleData(Guid.Empty, foundedModule.Id, humidity.Value);

			var sensorData = new SensorViewModel
			{
				HasAlarm = false,
				Type = "Humidity",
				Value = humidity.Value,
				Name = foundedModule.ModuleName,
			};
			if (foundedModule.Minimum > humidity.Value || foundedModule.Maximum < humidity.Value)
				sensorData.HasAlarm = true;

			centerResponse.Sensors.Add(sensorData);
			_dbContext.ModuleDatas.Add(moduleData);
		}

		return centerResponse;

	}
}
