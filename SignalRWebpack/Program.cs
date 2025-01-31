using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models.Infrastructure;
using SignalRWebpack.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<BroadcastingService>();
builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddHostedService<DataCollectionService>();


var app = builder.Build();
app.UseWebSockets();

var broadcastingService = app.Services.GetRequiredService<BroadcastingService>();
broadcastingService.Start();

app.Map("/WebSocket", async (HttpContext context) =>
{
	await broadcastingService.HandleWebSocketConnection(context);
});

app.Run();
