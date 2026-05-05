using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.Services;

public class CreateNotificationServer(IUnitOfWork unitOfWork) : ICreateNotificationService
{

    public async Task CreateAsync(Guid userId, string message, CancellationToken ct = default)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Message = message,
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };

        await unitOfWork.Notifications.AddAsync(notification, ct);
    }
}