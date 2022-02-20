using System.Threading.Tasks;
using LiveLines.Api.Users;

namespace LiveLines.Api.Streaks;

public interface IStreakService
{
    Task<int> IncrementStreak(LoggedInUser user);

    Task<int> GetStreak(LoggedInUser user);
}