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
}