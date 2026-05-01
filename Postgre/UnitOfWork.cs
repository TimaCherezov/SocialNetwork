using Domain.Abstractions;
using Domain.Abstractions.Repositories;

namespace Persistance;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    public IUserRepository Users { get; } = new UserRepository(dbContext);
    public IPostRepository Posts { get; } = new PostRepository(dbContext);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => dbContext.SaveChangesAsync(cancellationToken);
}