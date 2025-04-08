
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

    // user connected event
    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("UserConnected", Context.User.Identity.Name);
        await base.OnConnectedAsync();
    }

    // user discoinnected event
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Clients.All.SendAsync("UserDisconnected", Context.User.Identity.Name);
        await base.OnDisconnectedAsync(exception);
    } 

    // send message to a specified user
    public Task SendMessageToUser(string user, string toUser, string message)
    {
        return Clients.User(toUser).SendAsync("ReceiveMessage", user, message);
    }

}


