using Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace LoginNotification;

public class LoggingNotificationBroadcaster(ILogger<LoggingNotificationBroadcaster> logger)
    : INotificationBroadcaster
{
    public Task BroadcastToAllAsync(string message, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("[NOTIFICATION] {Message}", message);
        return Task.CompletedTask;
    }

    public Task SendToUserAsync<T>(
        Guid userId,
        string method,
        T payload,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("[NOTIFICATION to {UserId}] {Method} {@Payload}", userId, method, payload);
        return Task.CompletedTask;
    }
}