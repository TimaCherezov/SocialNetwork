using Application.Abstractions;
using Application.DTO;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Events;

namespace Application.Services;

public class SendMessage(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IEventDispatcher eventDispatcher) : ISendMessage
{
    public async Task<MessageDto> SendAsync(
        Guid conversationId,
        string content,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException(
                "Message content cannot be empty.",
                nameof(content));

        var senderUserId = currentUserService.GetUserId();

        var conversation =
            await unitOfWork.Conversations.GetByIdAsync(conversationId, ct);

        if (conversation is null)
            throw new InvalidOperationException("Conversation not found.");

        
        var isParticipant = await unitOfWork.Conversations.IsParticipantAsync(conversationId, senderUserId, ct);
        
        if (!isParticipant)
            throw new UnauthorizedAccessException(
                "You are not a participant of this conversation.");

        var message = new Message
        {
            Id = Guid.NewGuid(),
            ConversationId = conversation.Id,
            SenderId = senderUserId,
            Content = content.Trim(),
            SentAt = DateTime.UtcNow
        };

        await unitOfWork.Messages.AddAsync(message, ct);

        conversation.LastMessageId = message.Id;

        await unitOfWork.SaveChangesAsync(ct);

        var recipients = conversation.Participants
            .Where(p => p.UserId != senderUserId)
            .Select(p => p.UserId)
            .ToArray();

        await eventDispatcher.DispatchAsync(
            new MessageSentDomainEvent(
                conversation.Id,
                message.Id,
                senderUserId,
                recipients,
                message.Content,
                message.SentAt),
            ct);

        return new MessageDto(
            message.Id,
            conversation.Id,
            senderUserId,
            message.Content,
            message.SentAt);
    }
}