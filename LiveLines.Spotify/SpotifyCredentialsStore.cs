using LiveLines.Api.Spotify;
using LiveLines.Api.Users;

namespace LiveLines.Spotify;

public class SpotifyCredentialsStore : ISpotifyCredentialsStore
{
    public Task<SpotifyCredentials> GetCredentialsForUser(LoggedInUser user)
    {
        throw new NotImplementedException();
    }

    public Task UpsertCredentialsForUser(LoggedInUser user, SpotifyCredentials credentials)
    {
        throw new NotImplementedException();
    }
}