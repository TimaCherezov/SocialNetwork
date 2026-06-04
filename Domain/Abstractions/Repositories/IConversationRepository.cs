using Domain.Entities;

namespace Domain.Abstractions.Repositories;

public interface IConversationRepository
{
    Task<Conversation?> GetDirectByIdsAsync(Guid userAId, Guid userBId, CancellationToken ct = default);
    Task<Conversation?> GetByIdAsync(Guid conversationId, CancellationToken ct = default);
    Task<bool> IsParticipantAsync(Guid conversationId, Guid userId, CancellationToken ct = default);
    Task AddAsync(Conversation conversation, CancellationToken ct = default);
}

