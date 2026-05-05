namespace Application.Abstractions;

public interface INotificationBroadcaster
{
    Task BroadcastToAllAsync(string message, CancellationToken cancellationToken = default);
}

