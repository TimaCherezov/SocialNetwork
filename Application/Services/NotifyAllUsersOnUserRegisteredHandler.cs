using Application.Abstractions;
using Domain.Events;

namespace Application.Services;

public class NotifyAllUsersOnUserRegisteredHandler(INotificationBroadcaster broadcaster)
    : IEventHandler<UserRegisteredDomainEvent>
{
    public Task HandleAsync(UserRegisteredDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var message = $"Новый пользователь зарегистрирован: {domainEvent.UserName} ({domainEvent.Email})";
        return broadcaster.BroadcastToAllAsync(message, cancellationToken);
    }
}

