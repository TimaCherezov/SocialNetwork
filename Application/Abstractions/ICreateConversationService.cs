using Domain.Entities;

namespace Application.Abstractions;

public interface ICreateConversationService
{
    public Task<Guid> CreatePrivateConversationAsync(Guid directUserBId, CancellationToken ct);
}