namespace Application.Abstractions;

public interface INotificationBroadcaster
{
    Task BroadcastToAllAsync(string message, CancellationToken cancellationToken = default);

    Task SendToUserAsync<T>(
        Guid userId,
        string method,
        T payload,
        CancellationToken cancellationToken = default);
}

