using Application.Abstractions;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Domain.Events;

namespace Application.Services;

public class SaveUserNotificationHandler(IUnitOfWork unitOfWork) : IEventHandler<UserRegisteredDomainEvent>
{
    public async Task HandleAsync(UserRegisteredDomainEvent e, CancellationToken ct = default)
    {
        var users = await unitOfWork.Users.GetAllAsync(ct);

        var notifications = users
            .Where(u => u.Id != e.UserId) 
            .Select(u => new Notification
            {
                Id = Guid.NewGuid(),
                UserId = u.Id,
                Message = $"Новый пользователь зарегистрирован: {e.UserName}",
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            });

        await unitOfWork.Notifications.AddRangeAsync(notifications, ct);
        await unitOfWork.SaveChangesAsync(ct);

    }
}