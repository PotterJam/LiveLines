using System.Threading.Tasks;
using LiveLines.Api.Users;

namespace LiveLines.Api.Spotify;

public interface ISpotifyService
{
    public Task UpsertSpotifyCredentials(LoggedInUser user, SpotifyCredentials credentials);
}