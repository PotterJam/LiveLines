using LiveLines.Api.Streaks;
using LiveLines.Api.Users;

namespace LiveLines.Streaks;

public class StreakService : IStreakService
{
    private readonly IStreakStore _streakStore;

    public StreakService(IStreakStore streakStore)
    {
        _streakStore = streakStore;
    }

    public async Task<int> UpdateStreak(LoggedInUser user, DateTime newLineCreation)
    {
        throw new NotImplementedException();
    }

    public async Task<Streak> GetStreak(LoggedInUser user)
    {
        return await _streakStore.GetStreak(user);
    }
}