using System.Threading.Tasks;
using LiveLines.Api.Users;

namespace LiveLines.Api.Spotify;

public interface ISpotifyService
{
    public Task<SpotifyCredentials> GetSpotifyCredentials(LoggedInUser user, string clientId, string clientSecret);

    public Task UpsertSpotifyCredentials(LoggedInUser user, string code, string redirectUri, string clientId, string clientSecret);
}