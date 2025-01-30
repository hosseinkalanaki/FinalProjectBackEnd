using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DatabaseContext>(options =>
	options.UseSqlServer(connectionString));

builder.Services.AddScoped<ITcpDataCollectorService, TcpDataCollectorService>();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
	endpoints.MapHub<BroadcastHub>("/broadcastHub");
});

var scope = app.Services.CreateScope();
var dataCollectorService = scope.ServiceProvider.GetRequiredService<ITcpDataCollectorService>();
var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<BroadcastHub>>();

_ = Task.Run(async () =>
{
	while (true)
	{
		var data = await dataCollectorService.CollectDataAsync();
		await hubContext.Clients.All.SendAsync("ReceiveCollectedData", data);
		await Task.Delay(TimeSpan.FromSeconds(30));
	}
});

app.Run();
