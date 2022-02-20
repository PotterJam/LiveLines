using System;
using System.Threading.Tasks;
using LiveLines.Api.Users;

namespace LiveLines.Api.Streaks;

public interface IStreakService
{
    Task<int> UpdateStreak(LoggedInUser user, DateTime newLineCreation);

    Task<Streak> GetStreak(LoggedInUser user);
}