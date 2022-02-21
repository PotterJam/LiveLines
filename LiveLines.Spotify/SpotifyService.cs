using LiveLines.Api.Spotify;
using LiveLines.Api.Users;

namespace LiveLines.Spotify;

public class SpotifyService : ISpotifyService
{
    private readonly ISpotifyCredentialsStore _spotifyCredentialsStore;

    public SpotifyService(ISpotifyCredentialsStore spotifyCredentialsStore)
    {
        _spotifyCredentialsStore = spotifyCredentialsStore;
    }

    public Task UpsertSpotifyCredentials(LoggedInUser user, SpotifyCredentials credentials)
    {
        throw new NotImplementedException();
    }
}