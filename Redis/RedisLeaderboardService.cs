using Application.Abstractions;
using Application.Settings;
using Domain.Abstractions.Repositories;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Redis;

public class RedisLeaderboardService(
    IConnectionMultiplexer multiplexer,
    IOptions<RedisSettings> options,
    IUserRepository userRepository)
    : ILeaderboardService
{
    private readonly IDatabase _db = multiplexer.GetDatabase();
    private readonly string _leaderboardKey = options.Value.LeaderboardAuthorsPostsKey;
    private readonly TimeSpan _userNameTtl = TimeSpan.FromSeconds(options.Value.UsersTtlSeconds); 

    private static RedisKey UserNameKey(Guid id) => $"user:{id}:username";

    public Task IncrementAuthorPostsAsync(
        Guid authorUserId,
        double incrementBy = 1,
        CancellationToken cancellationToken = default)
    {
        return _db.SortedSetIncrementAsync(_leaderboardKey, authorUserId.ToString(), incrementBy);
    }

    public async Task<LeaderboardEntry[]> GetTopAuthorsByPostsAsync(
        int top,
        CancellationToken cancellationToken = default)
    {
        if (top <= 0)
            return [];

        var entries = await _db.SortedSetRangeByRankWithScoresAsync(
            _leaderboardKey,
            start: 0,
            stop: top - 1,
            order: Order.Descending);

        var topFromRedis = entries
            .Select(e => (ok: Guid.TryParse(e.Element.ToString(), out var id), id, score: e.Score))
            .Where(x => x.ok)
            .Select(x => (x.id, x.score))
            .ToArray();

        if (topFromRedis.Length == 0)
            return [];

        var ids = topFromRedis.Select(x => x.id).ToArray();
        var keys = ids.Select(UserNameKey).ToArray();
        var cachedNames = await _db.StringGetAsync(keys); 

        var nameById = new Dictionary<Guid, string>(ids.Length);
        var missingIds = new List<Guid>();

        for (var i = 0; i < ids.Length; i++)
        {
            if (cachedNames[i].HasValue)
                nameById[ids[i]] = cachedNames[i]!;
            else
                missingIds.Add(ids[i]);
        }

        if (missingIds.Count > 0)
        {
            var users = await userRepository.GetByIdsAsync(missingIds.ToArray(), cancellationToken);

            var setTasks = new List<Task>();

            foreach (var u in users)
            {
                nameById[u.Id] = u.UserName;

                setTasks.Add(_db.StringSetAsync(
                    UserNameKey(u.Id),
                    u.UserName,
                    expiry: _userNameTtl));
            }

            await Task.WhenAll(setTasks);
        }

        var result = new List<LeaderboardEntry>(topFromRedis.Length);

        foreach (var e in topFromRedis)
        {
            if (nameById.TryGetValue(e.id, out var userName))
                result.Add(new LeaderboardEntry(e.id, userName, e.score));
        }

        return result.ToArray();
    }
}