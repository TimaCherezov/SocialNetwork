using Domain.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Repositories;

public sealed class MessageRepository(AppDbContext dbContext) : IMessageRepository
{
    public async Task AddAsync(Message message, CancellationToken ct = default)
        => await dbContext.Messages.AddAsync(message, ct);

    public async Task<List<Message>> GetByConversationAsync(
        Guid conversationId,
        int count,
        CancellationToken ct = default)
        => await dbContext.Messages
            .Where(m => m.ConversationId == conversationId)
            .OrderByDescending(m => m.SentAt)
            .Take(count)
            .ToListAsync(ct);
}

