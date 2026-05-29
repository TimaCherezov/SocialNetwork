namespace Application.Settings;

public class RedisSettings
{
    public const string Section = "Redis";

    public string? ConnectionString { get; init; }
    public string LeaderboardAuthorsPostsKey { get; init; } = "leaderboard:authors:posts";
    public int UsersTtlSeconds { get; init; } = 3600;
}

