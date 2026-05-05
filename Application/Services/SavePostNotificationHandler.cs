using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Events;

namespace Application.Services;

public class SavePostNotificationHandler(IUnitOfWork unitOfWork) : IEventHandler<PostCreatedDomainEvent>
{
    public async Task HandleAsync(PostCreatedDomainEvent e, CancellationToken ct = default)
    {
        var users = await unitOfWork.Users.GetAllAsync(ct);
        var author = await unitOfWork.Users.GetByIdAsync(e.AuthorUserId, ct);
        if (author == null)
        {
            throw new InvalidOperationException($"Author with ID {e.AuthorUserId} not found.");
        }
        
        var notifications = users
            .Where(u => u.Id != e.AuthorUserId) 
            .Select(u => new Notification
            {
                Id = Guid.NewGuid(),
                UserId = u.Id,
                Message = $"Пользователь {author.FirstName} {author.SurName ?? ""} создал новый пост: '{e.Title}'",
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            });

        await unitOfWork.Notifications.AddRangeAsync(notifications, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }
}