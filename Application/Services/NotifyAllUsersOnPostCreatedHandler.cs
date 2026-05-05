using Application.Abstractions;
using Domain.Events;

namespace Application.Services;

public class NotifyAllUsersOnPostCreatedHandler(INotificationBroadcaster broadcaster)
    : IEventHandler<PostCreatedDomainEvent>
{
    public Task HandleAsync(PostCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var message = $"Новый пост: '{domainEvent.Title}' (authorId: {domainEvent.AuthorUserId})";
        return broadcaster.BroadcastToAllAsync(message, cancellationToken);
    }
}

