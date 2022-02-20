using Extensions;
using LiveLines.Api.Database;
using LiveLines.Api.Streaks;
using LiveLines.Api.Users;

namespace LiveLines.Streaks;

public class StreakStore : IStreakStore
{
    private readonly IDatabaseCommandExecutor _dbExecutor;

    public StreakStore(IDatabaseCommandExecutor dbExecutor)
    {
        _dbExecutor = dbExecutor;
    }
    
    public async Task<Streak> GetStreak(LoggedInUser loggedInUser)
    {
        return await _dbExecutor.ExecuteCommand(async cmd =>
        {
            cmd.AddParam("@userid", loggedInUser.InternalId);

            cmd.CommandText = @"
                    SELECT s.streak
                    FROM streaks s
                    JOIN users u on s.user_id = u.id
                    WHERE s.user_id = @userid
                    LIMIT 1;";

            var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                throw new StreakStoreException($"Couldn't get streak for user {loggedInUser.InternalId}");

            var streak = reader.Get<int>("streak");
            var lastUpdated = reader.Get<DateTime>("last_updated");
            
            return new Streak(streak, lastUpdated);
        });
    }
}