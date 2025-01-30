using System.Net.Sockets;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Models.Infrastructure;

public class DataCollectorService 
{
	private readonly DatabaseContext _dbContext;

	public DataCollectorService()
	{
		_dbContext = new DatabaseContext();
	}

	public async Task<List<CenterViewModel>> CollectDataAsync()
	{
		try
		{
			var moduleData = await _dbContext.ModuleDatas
				.Include(current => current.Module)
					.ThenInclude(current => current.Center)
				.Where(current => !current.IsDeleted)
				.GroupBy(current => current.ModuleId)
				.Select(g => g.OrderByDescending(x => x.DateAdded).FirstOrDefault())
				.ToListAsync();

			var centerViewModel = await _dbContext.Centers
				.Where(current => current.IsDeleted == false)
				.Select(current=>new CenterViewModel
				{
					Id=current.Id,
					CenterName=current.Name,
					IpAddress=current.IPAddress,
					Port=current.Port
				})
				.ToListAsync();

            foreach (var center in centerViewModel)
            {
				var centerSensors = await _dbContext.ModuleDatas
				.Include(current => current.Module)
					.ThenInclude(current => current.Center)
				.Where(current => !current.IsDeleted
					&& current.Module.CenterId == center.Id)
				.GroupBy(current => current.ModuleId)
				.Select(g => g.OrderByDescending(x => x.DateAdded).FirstOrDefault())
				.ToListAsync();

				center.Sensors = centerSensors
				.Select(current => new SensorViewModel
				{
					Name = current.Module.ModuleName,
					Type = current.Module.IsTemp == true ? "Temperature" : "Humidity",
					Value = current.Value,
					HasAlarm = current.Value < current.Module.Minimum || current.Value > current.Module.Maximum ? true : false,
				})
				.ToList();

			}
            
			return centerViewModel;
		}
		catch (Exception)
		{

			throw;
		}
	}

	public async Task GetDataLoop()
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

			foreach (var center in centers)
			{
				try
				{
					var thread = new Thread(async () =>
					{
						await getData(center);
					});

					thread.Start();
				}
				catch (Exception)
				{
					continue;
				}
			}

			await _dbContext.SaveChangesAsync();

			return ;
		}
		catch (Exception)
		{

			throw;
		}
	}

	private async Task<CenterViewModel?> proccessData(Center center)
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

		if (centerResponse == null)
			return null;

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

	private async Task getData(Center center)
	{
		var client = new TcpClient();
		await client.ConnectAsync(center.IPAddress, int.Parse(center.Port));

		var stream = client.GetStream();
		var reader = new StreamReader(stream);

		char[] buffer = new char[256];
		int bytesRead = await reader.ReadAsync(buffer, 0, buffer.Length);

		var response = new string(buffer, 0, bytesRead);

		if (string.IsNullOrWhiteSpace(response))
			return;

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

			_dbContext.ModuleDatas.Add(moduleData);
		}

		_dbContext.SaveChanges();
	}
}
