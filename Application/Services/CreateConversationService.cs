using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services;

public class CreateConversationService(ICurrentUserService currentUserService, IUnitOfWork unitOfWork) : ICreateConversationService
{
    public async Task<Guid> CreatePrivateConversationAsync(Guid directUserBId, CancellationToken ct)
    {
        var directUserAId = currentUserService.GetUserId();
        var isExisted = await unitOfWork.Conversations
            .GetDirectByIdsAsync(directUserAId, directUserBId, ct);

        if (isExisted is not null)
        {
            throw new InvalidOperationException("This conversation already exists.");
        }
        
        var conversation = new Conversation
        {
            Id = Guid.NewGuid(),
            Type = ConversationType.Private,
            CreatedAt = DateTime.UtcNow,
            DirectUserAId = currentUserService.GetUserId(),
            DirectUserBId = directUserBId,
        };
        conversation.Participants.Add(new ConversationParticipant
        {
            ConversationId = conversation.Id,
            UserId = directUserAId,
            JoinedAt = DateTime.UtcNow
        });

        conversation.Participants.Add(new ConversationParticipant
        {
            ConversationId = conversation.Id,
            UserId = directUserBId,
            JoinedAt = DateTime.UtcNow
        });
        await unitOfWork.Conversations.AddAsync(conversation, ct);
        await unitOfWork.SaveChangesAsync(ct);
        
        return conversation.Id;
    }
}