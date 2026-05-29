namespace Application.Abstractions;

public interface ILeaderboardService
{
    Task IncrementAuthorPostsAsync(Guid authorUserId, double incrementBy = 1, CancellationToken cancellationToken = default);
    Task<LeaderboardEntry[]> GetTopAuthorsByPostsAsync(int top, CancellationToken cancellationToken = default);
}

public record LeaderboardEntry(Guid AuthorUserId, string UserName, double Score);

