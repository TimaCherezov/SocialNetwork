using Domain.Abstractions;
using Domain.Abstractions.Repositories;

namespace Persistance;

public class UnitOfWork : IUnitOfWork
{
    public IUserRepository Users { get; }
    public IPostRepository Posts { get; }
    public IRefreshTokenRepostory RefreshTokens { get; }
    public INotificationRepository Notifications { get; }
    private readonly AppDbContext _dbContext;

    public UnitOfWork(
        AppDbContext dbContext, 
        IUserRepository userRepository, 
        IPostRepository postRepository, 
        IRefreshTokenRepostory refreshTokenRepository,
        INotificationRepository notificationRepository
        )
    {
        _dbContext = dbContext;
        Users = userRepository;
        Posts = postRepository;
        RefreshTokens = refreshTokenRepository;
        Notifications = notificationRepository;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _dbContext.SaveChangesAsync(cancellationToken);
}