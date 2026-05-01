using Domain.Abstractions.Repositories;
using Domain.Entities;
using Persistance;
using Microsoft.EntityFrameworkCore;

namespace Persistance;

public class PostRepository(AppDbContext dnContext) : IPostRepository
{
    public async Task<Post?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dnContext.Posts.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task AddAsync(Post post, CancellationToken cancellationToken = default)
        => await dnContext.Posts.AddAsync(post, cancellationToken);

    public async Task<IEnumerable<Post>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        => await dnContext.Posts.Where(p => p.UserId == userId).ToListAsync(cancellationToken);

    public Task RemoveAsync(Post post, CancellationToken cancellationToken = default)
    {
        dnContext.Posts.Remove(post);
        return Task.CompletedTask;
    }
}
