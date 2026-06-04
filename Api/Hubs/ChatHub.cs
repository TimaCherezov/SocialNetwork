using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Application.Abstractions;

namespace Api.Hubs;

[Authorize]
public class ChatHub(ISendMessage sendMessage) : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{userId}");
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user:{userId}");
    }
    
    public async Task SendMessage(Guid conversationId, string content)
        => await sendMessage.SendAsync(conversationId, content);
}

