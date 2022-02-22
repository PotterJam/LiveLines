using System.Threading.Tasks;
using LiveLines.Api.Users;

namespace LiveLines.Api.Streaks;

public interface IStreakService
{
    Task<int> UpdateStreakForNewLine(LoggedInUser user, bool forYesterday);

    Task<int> GetOrCreateStreak(LoggedInUser user);
}