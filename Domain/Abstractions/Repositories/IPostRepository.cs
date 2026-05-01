namespace Domain.Abstractions.Repositories;

using Domain.Entities;

public interface IPostRepository
{
    Task<Post?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Post post, CancellationToken cancellationToken = default);
    Task<IEnumerable<Post>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task RemoveAsync(Post post, CancellationToken cancellationToken = default);
}

