using Microsoft.AspNetCore.SignalR;
using Models.Infrastructure;

namespace SignalRWebpack.Hubs;

public class ChatHub : Hub
{
	public async Task SendMessage(string user, string message)
	{
		using (var db=new DatabaseContext())
		{
			
		}
		await Clients.All.SendAsync("ReceiveMessage", user, message);
	}
}