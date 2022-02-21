using System;
using System.Data.Common;
using System.Threading.Tasks;
using Extensions;
using LiveLines.Api;
using LiveLines.Api.Database;
using LiveLines.Api.Users;

namespace LiveLines.Users;

public class ProfileStore : IProfileStore
{
    private readonly IDatabaseCommandExecutor _dbExecutor;

    public ProfileStore(IDatabaseCommandExecutor dbExecutor)
    {
        _dbExecutor = dbExecutor;
    }

    public async Task<Profile> UpsertProfile(LoggedInUser user, Privacy defaultPrivacy)
    {
        return await _dbExecutor.ExecuteCommand(async cmd =>
        {
            cmd.AddParam("@userId", user.InternalId);
            cmd.AddParam("@defaultPrivacy", defaultPrivacy);

            cmd.CommandText = @"
                    WITH new_user AS (
                        INSERT INTO users (provider, username)
                        VALUES (@provider, @username)
                        ON CONFLICT(username) DO UPDATE
                            SET last_login = NOW()
                        RETURNING id
                    ) SELECT COALESCE(
                        (SELECT id FROM new_user),
                        (SELECT id FROM users WHERE username = @username)
                    ) AS id;";

            var guid = (Guid?) await cmd.ExecuteScalarAsync();

            if (guid == null)
                throw new UserStoreException("Tried to create profile, nothing got returned");

            return await GetProfile(user, guid.Value);
        });
    }
    
    private async Task<Profile> GetProfile(LoggedInUser loggedInUser, Guid profileId)
    {
        return await _dbExecutor.ExecuteCommand(async cmd =>
        {
            cmd.AddParam("@profileId", profileId);
            cmd.AddParam("@userId", loggedInUser.InternalId);

            cmd.CommandText = @"
                    SELECT p.id, p.user_id, p.default_privacy
                    FROM profiles p
                    LEFT JOIN users u ON p.user_id = u.id
                    WHERE p.id = @profileId
                        AND p.user_id = @userId
                    LIMIT 1;";

            var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                throw new ProfileStoreException($"Couldn't get profile {profileId} for user {loggedInUser.InternalId}");

            return ReadProfile(reader);
        });
    }
    
    public async Task<Profile> GetProfile(LoggedInUser loggedInUser)
    {
        return await _dbExecutor.ExecuteCommand(async cmd =>
        {
            cmd.AddParam("@userId", loggedInUser.InternalId);

            cmd.CommandText = @"
                    SELECT p.id, p.user_id, p.default_privacy
                    FROM profiles p
                    WHERE p.user_id = @userId
                    LIMIT 1;";

            var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                throw new ProfileStoreException($"Couldn't get profile for user {loggedInUser.InternalId}");

            return ReadProfile(reader);
        });
    }

    private Profile ReadProfile(DbDataReader reader)
    {
        var id = reader.Get<Guid>("id");
        var userId = reader.Get<Guid>("user_id"); 
        var defaultPrivacy = reader.Get<Privacy>("default_privacy"); 

        return new Profile(id, userId, defaultPrivacy);
    }
}