using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using LiveLines.Api.Spotify;
using LiveLines.Api.Users;

namespace LiveLines.Spotify;

public class SpotifyService : ISpotifyService
{
    private static readonly HttpClient HttpClient = new ();
    
    private readonly ISpotifyCredentialsStore _spotifyCredentialsStore;
    private readonly string _spotifyClientId;
    private readonly string _spotifyClientSecret;
    
    public SpotifyService(IConfiguration configuration, ISpotifyCredentialsStore spotifyCredentialsStore)
    {
        _spotifyClientId = configuration.GetValue<string>("SPOTIFY_CLIENT_ID");
        _spotifyClientSecret = configuration.GetValue<string>("SPOTIFY_CLIENT_SECRET");
        _spotifyCredentialsStore = spotifyCredentialsStore;
    }
    
    public async Task<IEnumerable<SpotifyTrack>> GetRecentlyPlayed(LoggedInUser user)
    {
        var spotifyCredentials = await GetSpotifyCredentials(user);
        if (spotifyCredentials == null)
        {
            // TODO: fix this once support for them unauthorizing works
            throw new Exception("This could happen if the user unauthorizes, it's a todo");
        }
        
        // TODO: move below to a spotify client
        const string recentlyPlayedUrl = "https://api.spotify.com/v1/me/player/recently-played?limit=10";
        var request = new HttpRequestMessage(HttpMethod.Get, recentlyPlayedUrl);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", spotifyCredentials.AccessToken);

        var response = await HttpClient.SendAsync(request);

        var tracks = await response.Content.ReadFromJsonAsync<ItemsResponse>();

        if (tracks == null)
        {
            throw new Exception("todo: handle malformed response");
        }

        return tracks.Items.Select(x => new SpotifyTrack(x.Track.Name));
    }
    
    private record ItemsResponse(
        [property: JsonPropertyName("items")] IEnumerable<ItemResponse> Items);

    private record ItemResponse(
        [property: JsonPropertyName("track")] TrackResponse Track);
    
    private record TrackResponse(
        [property: JsonPropertyName("name")] string Name);

    // give 5 minutes of leeway
    private DateTime GenerateAccessTokenExpiry(int expiresInSeconds) => DateTime.UtcNow.AddSeconds(expiresInSeconds - 60 * 5);

    public async Task<SpotifyCredentials?> GetSpotifyCredentials(LoggedInUser user)
    {
        var spotifyCredentials = await _spotifyCredentialsStore.GetCredentialsForUser(user);

        if (spotifyCredentials == null)
        {
            return null;
        }
        
        if (spotifyCredentials.ExpiresAt < DateTime.UtcNow)
        {
            // TODO: if this fails, remove the credentials from the database, as it means they've unauthorized the app
            var refreshedSpotifyCredentials = await RefreshAccessToken(spotifyCredentials.RefreshToken);
            
            await _spotifyCredentialsStore.UpsertCredentialsForUser(user, refreshedSpotifyCredentials);
            
            return refreshedSpotifyCredentials;
        }

        return spotifyCredentials;
    }

    private async Task<SpotifyCredentials> RefreshAccessToken(string refreshToken)
    {
        var data = new Dictionary<string, string>
        {
            {"refresh_token", refreshToken},
            {"grant_type", "refresh_token"},
        };

        // refresh doesn't give you another refresh token, so use the same one until they unauthorize the app
        var (accessToken, tokenType, scope, expiresIn, _) = await GetSpotifyCredentialsFromOptions(data);
        return new SpotifyCredentials(accessToken, tokenType, scope, GenerateAccessTokenExpiry(expiresIn), refreshToken);
    }

    public async Task UpsertSpotifyCredentials(LoggedInUser user, string code, string redirectUrl)
    {
        var data = new Dictionary<string, string>
        {
            {"code", code},
            {"grant_type", "authorization_code"},
            {"redirect_uri", redirectUrl},
        };

        var (accessToken, tokenType, scope, expiresIn, refreshToken) = await GetSpotifyCredentialsFromOptions(data);

        if (refreshToken == null)
            throw new ArgumentNullException(nameof(refreshToken), "Refresh token missing when retrieving Spotify credentials from the authorization request.");
        
        var spotifyCredentials = new SpotifyCredentials(accessToken, tokenType, scope, GenerateAccessTokenExpiry(expiresIn), refreshToken);

        await _spotifyCredentialsStore.UpsertCredentialsForUser(user, spotifyCredentials);
    }

    // TODO: can move this stuff to a wrapped HttpClient, SpotifyClient.
    private async Task<SpotifyCredentialsResponse> GetSpotifyCredentialsFromOptions(Dictionary<string, string> data)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
        request.Content = new FormUrlEncodedContent(data);

        var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_spotifyClientId}:{_spotifyClientSecret}"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authValue);

        var response = await HttpClient.SendAsync(request);
        var spotifyTokenInfo = await response.Content.ReadFromJsonAsync<SpotifyCredentialsResponse>();

        if (spotifyTokenInfo == null)
        {
            throw new ArgumentNullException($"Spotify token info not deserialised correctly from Spotify response");
        }

        return spotifyTokenInfo;
    }

    // From the spotify docs https://developer.spotify.com/documentation/general/guides/authorization/code-flow/
    private record SpotifyCredentialsResponse(
        [property: JsonPropertyName("access_token")] string AccessToken,
        [property: JsonPropertyName("token_type")] string TokenType,
        [property: JsonPropertyName("scope")] string Scope,
        [property: JsonPropertyName("expires_in")] int ExpiresIn,
        [property: JsonPropertyName("refresh_token")] string? RefreshToken);
}