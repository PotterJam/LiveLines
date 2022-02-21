using System.Threading.Tasks;
using LiveLines.Api.Users;

namespace LiveLines.Api.Spotify;

public interface ISpotifyService
{
    public Task<SpotifyCredentials> GetSpotifyCredentials(LoggedInUser user);

    public Task UpsertSpotifyCredentials(LoggedInUser user, string code, string redirectUrl);
}