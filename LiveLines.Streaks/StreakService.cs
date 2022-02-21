using LiveLines.Api.Lines;
using LiveLines.Api.Streaks;
using LiveLines.Api.Users;
using Microsoft.Extensions.Caching.Memory;

namespace LiveLines.Streaks;

public class StreakService : IStreakService
{
    private readonly ILinesService _linesService;
    private readonly IMemoryCache _expiringStreakCache;

    private const string StreakCachePrefix = "Streak_";

    public StreakService(ILinesService linesService, IMemoryCache expiringStreakCache)
    {
        _linesService = linesService;
        _expiringStreakCache = expiringStreakCache;
    }

    private string GetStreakCacheKey(LoggedInUser user) => StreakCachePrefix + user.InternalId;

    public async Task<int> IncrementStreak(LoggedInUser user)
    {
        var newStreak = await GetStreak(user) + 1;
        return _expiringStreakCache.Set(GetStreakCacheKey(user), newStreak);
    }

    public async Task<int> GetStreak(LoggedInUser user)
    {
        return await _expiringStreakCache.GetOrCreateAsync(
            GetStreakCacheKey(user),
            async cacheEntry =>
            {
                // We want it to expire at midnight the day after tomorrow, midnight makes this confusing.
                cacheEntry.AbsoluteExpiration = DateTime.UtcNow.AddDays(2).Date;
                return await GetStreakCount(user);
            });
    }

    private async Task<int> GetStreakCount(LoggedInUser loggedInUser)
    {
        // could do better than this, perhaps there's a fancy query we can do
        // that counts db-side consecutive line dates and short circuits. This will do for now.
        var orderedLines = (await _linesService.GetLines(loggedInUser))
            .OrderByDescending(x => x.CreatedAt);

        var previous = DateTime.Now;
        var count = 0;

        foreach (var line in orderedLines)
        {
            if ((previous.Date - line.CreatedAt.Date).TotalDays > 1)
            {
                break;
            }

            previous = line.CreatedAt;
            count++;
        }

        return count;
    }
}
