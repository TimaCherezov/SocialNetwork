using Application.Abstractions;
using Application.DTO.Responses;
using Domain.Abstractions;

namespace Application.Services;

public class GetNotificationsService(IUnitOfWork unitOfWork) : IGetNotificationsServer
{
    public async Task<IEnumerable<NotificationResponse>> GetAllNotificationsByUserIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var notifications = await unitOfWork.Notifications.GetByUserIdAsync(id, cancellationToken);
        return notifications
            .Select(n => new NotificationResponse(n.UserId, n.Message, n.CreatedAt, n.IsRead)).ToList();
    }
}