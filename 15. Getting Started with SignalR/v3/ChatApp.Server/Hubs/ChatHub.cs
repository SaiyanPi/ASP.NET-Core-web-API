
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Server.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public Task SendMessage(string username, string message)
    {
        return Clients.All.SendAsync("ReceiveMessage", username, message);
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("UserConnected", Context.User.Identity.Name);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Clients.All.SendAsync("UserDisconnected", Context.User.Identity.Name);
        await base.OnDisconnectedAsync(exception);
    } 

}


