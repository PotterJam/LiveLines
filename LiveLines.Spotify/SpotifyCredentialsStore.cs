using Extensions;
using LiveLines.Api.Database;
using LiveLines.Api.Lines;
using LiveLines.Api.Spotify;
using LiveLines.Api.Users;

namespace LiveLines.Spotify;

public class SpotifyCredentialsStore : ISpotifyCredentialsStore
{
    private readonly IDatabaseCommandExecutor _dbExecutor;

    public SpotifyCredentialsStore(IDatabaseCommandExecutor dbExecutor)
    {
        _dbExecutor = dbExecutor;
    }
    
    public async Task<SpotifyCredentials?> GetCredentialsForUser(LoggedInUser user)
    {
        return await _dbExecutor.ExecuteCommand(async cmd =>
        {
            cmd.AddParam("@userId", user.InternalId);

            cmd.CommandText = @"
                    SELECT user_id, access_token, refresh_token, token_type, ""scope"", expires_at
                    FROM spotify_credentials 
                    WHERE user_id = @userId;";

            var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            var accessToken = reader.Get<string>("access_token");
            var refreshToken = reader.Get<string>("refresh_token");
            var tokenType = reader.Get<string>("token_type");
            var scope = reader.Get<string>("scope");
            var expiresAt = reader.Get<DateTime>("expires_at");

            return new SpotifyCredentials(
                AccessToken: accessToken,
                TokenType: tokenType,
                Scope: scope,
                ExpiresAt: expiresAt,
                RefreshToken: refreshToken);
        });
    }

    public Task UpsertCredentialsForUser(LoggedInUser user, SpotifyCredentials credentials)
    {
        throw new NotImplementedException();
    }
}