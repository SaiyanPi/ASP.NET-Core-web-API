
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
}


