using Domain.Entities;

namespace Domain.Abstractions.Repositories;

public interface IMessageRepository
{
    Task AddAsync(Message message, CancellationToken ct = default);

    Task<List<Message>> GetByConversationAsync(
        Guid conversationId,
        int count,
        CancellationToken ct = default);
}

