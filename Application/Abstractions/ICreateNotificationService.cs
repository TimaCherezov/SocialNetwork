namespace Application.Abstractions;

public interface ICreateNotificationService
{
    public Task CreateAsync(Guid userId, string message, CancellationToken cancellationToken = default);
}