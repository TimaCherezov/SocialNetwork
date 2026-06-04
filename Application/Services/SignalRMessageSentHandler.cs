using Application.Abstractions;
using Application.DTO;
using Domain.Events;

namespace Application.Services;

public class SignalRMessageSentHandler(INotificationBroadcaster broadcaster)
    : IEventHandler<MessageSentDomainEvent>
{
    public Task HandleAsync(MessageSentDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var dto = new MessageDto(
            domainEvent.MessageId,
            domainEvent.ConversationId,
            domainEvent.SenderUserId,
            domainEvent.Content,
            domainEvent.SentAt);

        var tasks = new List<Task>(domainEvent.RecipientUserIds.Length);
        tasks.AddRange(domainEvent.RecipientUserIds
            .Select(recipientId => broadcaster
                .SendToUserAsync(recipientId, "ReceiveMessage", dto, cancellationToken)));

        tasks.Add(broadcaster.SendToUserAsync(domainEvent.SenderUserId, "ReceiveMessage", dto, cancellationToken));

        return Task.WhenAll(tasks);
    }
}

