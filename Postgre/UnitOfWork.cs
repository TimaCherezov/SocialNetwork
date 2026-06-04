using Domain.Abstractions;
using Domain.Abstractions.Repositories;

namespace Persistance;

public class UnitOfWork : IUnitOfWork
{
    public IUserRepository Users { get; }
    public IPostRepository Posts { get; }
    public IRefreshTokenRepostory RefreshTokens { get; }
    public INotificationRepository Notifications { get; }
    public IConversationRepository Conversations { get; }
    public IMessageRepository Messages { get; }
    private readonly AppDbContext _dbContext;

    public UnitOfWork(
        AppDbContext dbContext, 
        IUserRepository userRepository, 
        IPostRepository postRepository, 
        IRefreshTokenRepostory refreshTokenRepository,
        INotificationRepository notificationRepository,
        IConversationRepository conversationRepository,
        IMessageRepository messageRepository
        )
    {
        _dbContext = dbContext;
        Users = userRepository;
        Posts = postRepository;
        RefreshTokens = refreshTokenRepository;
        Notifications = notificationRepository;
        Conversations = conversationRepository;
        Messages = messageRepository;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _dbContext.SaveChangesAsync(cancellationToken);
}