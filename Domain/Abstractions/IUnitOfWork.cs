using System.Threading;
using System.Threading.Tasks;
using Domain.Abstractions.Repositories;

namespace Domain.Abstractions;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IPostRepository Posts { get; }
    IRefreshTokenRepostory RefreshTokens { get; }
    INotificationRepository Notifications { get; }
    IConversationRepository Conversations { get; }
    IMessageRepository Messages { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
