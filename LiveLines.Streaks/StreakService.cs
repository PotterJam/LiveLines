using LiveLines.Api.Streaks;
using LiveLines.Api.Users;
using Microsoft.Extensions.Caching.Memory;

namespace LiveLines.Streaks;

public class StreakService : IStreakService
{
    private readonly IMemoryCache _expiringStreakCache;

    public StreakService(IMemoryCache expiringStreakCache)
    {
        _expiringStreakCache = expiringStreakCache;
    }

    public Task<int> UpdateStreak(LoggedInUser user, DateTime newLineCreation)
    {
        throw new NotImplementedException();
    }

    public Task<Streak> GetStreak(LoggedInUser user)
    {
        throw new NotImplementedException();
    }
}