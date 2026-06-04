using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Hubs;
using Application.Abstractions;
using Microsoft.AspNetCore.SignalR;

namespace Api.Services;

public class SignalRNotificationBroadcaster(IHubContext<ChatHub> hubContext)
    : INotificationBroadcaster
{
    public Task BroadcastToAllAsync(string message, CancellationToken cancellationToken = default)
    {
        return hubContext.Clients.All.SendAsync("ReceiveNotification", message, cancellationToken);
    }

    public Task SendToUserAsync<T>(
        Guid userId,
        string method,
        T payload,
        CancellationToken cancellationToken = default)
    {
        return hubContext.Clients.Group($"user:{userId}").SendAsync(method, payload, cancellationToken);
    }
}

