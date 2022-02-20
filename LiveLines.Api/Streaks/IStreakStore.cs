using System.Threading.Tasks;
using LiveLines.Api.Users;

namespace LiveLines.Api.Streaks;

public interface IStreakStore
{
    Task<Streak> GetStreak(LoggedInUser loggedInUser);
}