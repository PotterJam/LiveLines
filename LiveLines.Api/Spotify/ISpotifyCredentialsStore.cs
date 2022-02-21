using System.Threading.Tasks;
using LiveLines.Api.Users;

namespace LiveLines.Api.Spotify;

public interface ISpotifyCredentialsStore
{
    public Task<SpotifyCredentials> GetCredentialsForUser(LoggedInUser user);
    public Task UpsertCredentialsForUser(LoggedInUser user, SpotifyCredentials credentials);
}