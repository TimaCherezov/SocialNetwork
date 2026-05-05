using Domain.Entities;

namespace Domain.Abstractions.Repositories;

public interface INotificationRepository
{
    public Task AddAsync(Notification notification, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}