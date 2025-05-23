
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Server.Hubs;

[Authorize]
public class ChatHub : Hub<IChatClient>
{
    // public Task SendMessage(string username, string message)
    // {
    //     return Clients.All.SendAsync("ReceiveMessage", username, message);
    // }
    public Task SendMessage(string user, string message)
    {
        return Clients.All.ReceiveMessage(user, message);
    }

    // send message to a specified user
    // public Task SendMessageToUser(string user, string toUser, string message)
    // {
    //     return Clients.User(toUser).SendAsync("ReceiveMessage", user, message);
    // }
    public Task SendMessageToUser(string user, string toUser, string message)
    {
        return Clients.User(toUser).ReceiveMessage(user, message);
    }

    public Task SendMessageToGroup(string user, string group, string message)
    {
        return Clients.Group(group).ReceiveMessage(user, message);
    }

    // user connected event
    // public override async Task OnConnectedAsync()
    // {
    //     await Clients.All.SendAsync("UserConnected", Context.User.Identity.Name);
    //     await base.OnConnectedAsync();
    // }
    public override async Task OnConnectedAsync()
    {
        await Clients.All.UserConnected(Context.User.Identity.Name);
        await base.OnConnectedAsync();
    }

    // user discoinnected event
    // public override async Task OnDisconnectedAsync(Exception? exception)
    // {
    //     await Clients.All.SendAsync("UserDisconnected", Context.User.Identity.Name);
    //     await base.OnDisconnectedAsync(exception);
    // } 
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Clients.All.UserDisconnected(Context.User.Identity.Name);
        await base.OnDisconnectedAsync(exception);
    }

    // joining groups
    public async Task AddToGroup(string user, string group)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, group);
        await Clients.Group(group).ReceiveMessage(Context.User.Identity.Name,
        $"{user} has joined the group {group}. Connection Id: {Context.ConnectionId}");
    }
    public async Task RemoveFromGroup(string user, string group)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
        await Clients.Group(group).ReceiveMessage(Context.User.Identity.Name,
        $"{user} has left the group {group}. Connection Id: {Context.ConnectionId}");
    }

}


