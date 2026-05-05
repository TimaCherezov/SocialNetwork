using Application.DTO.Responses;

namespace Application.Abstractions;

public interface IGetNotificationsServer
{
    public Task<IEnumerable<NotificationResponse>> GetAllNotificationsByUserIdAsync(Guid id, CancellationToken cancellationToken = default);
}