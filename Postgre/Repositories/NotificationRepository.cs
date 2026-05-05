using Domain.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Repositories;

public class NotificationRepository(AppDbContext dbContext) : INotificationRepository
{
    public async Task AddAsync(Notification notification, CancellationToken cancellationToken = default)
        => await dbContext.Notifications.AddAsync(notification, cancellationToken);

    public async Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId,
        CancellationToken cancellationToken = default)
        => await dbContext.Notifications.Where(n => n.UserId == userId).ToListAsync(cancellationToken);
}