using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Repositories;

public class ConversationRepository(AppDbContext dbContext) : IConversationRepository
{
    public Task<Conversation?> GetByIdAsync(Guid conversationId, CancellationToken ct = default)
        => dbContext.Conversations
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c => c.Id == conversationId, ct);

    public Task<Conversation?> GetDirectByIdsAsync(Guid userAId, Guid userBId, CancellationToken ct = default)
    {
        var (a, b) = userAId.CompareTo(userBId) <= 0 ? (userAId, userBId) : (userBId, userAId);

        return dbContext.Conversations
            .Include(c => c.Participants)
            .FirstOrDefaultAsync(c => c.Type == ConversationType.Private && c.DirectUserAId == a && c.DirectUserBId == b, ct);
    }

    public Task<bool> IsParticipantAsync(Guid conversationId, Guid userId, CancellationToken ct = default)
        => dbContext.ConversationParticipants.AnyAsync(cp => cp.ConversationId == conversationId && cp.UserId == userId, ct);

    public async Task AddAsync(Conversation conversation, CancellationToken ct = default)
        => await dbContext.Conversations.AddAsync(conversation, ct);
}

