using Application.Abstractions;
using Domain.Events;

namespace Application.Services;

public class IncrementLeaderboardOnPostCreatedHandler(ILeaderboardService leaderboardService)
    : IEventHandler<PostCreatedDomainEvent>
{
    public Task HandleAsync(PostCreatedDomainEvent e, CancellationToken ct = default)
        => leaderboardService.IncrementAuthorPostsAsync(e.AuthorUserId, 1, ct);
}

