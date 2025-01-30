using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models.Infrastructure;
using SignalRWebpack.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<BroadcastingService>();

var app = builder.Build();
app.UseWebSockets();

var broadcastingService = app.Services.GetRequiredService<BroadcastingService>();
broadcastingService.Start();

app.Map("/WebSocket", async (HttpContext context) =>
{
	await broadcastingService.HandleWebSocketConnection(context);
});

app.Run();
