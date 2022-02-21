using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using Extensions;
using LiveLines.Api.Spotify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LiveLines.Spotify;

[ApiController, Route("api")]
public class SpotifyController : ControllerBase
{
    private static readonly HttpClient HttpClient = new();

    private readonly ISpotifyService _spotifyService;

    public SpotifyController(ISpotifyService spotifyService)
    {
        _spotifyService = spotifyService;
    }
    private const string RedirectUri = "https://localhost:44492/api/spotify/callback";
    
    [Authorize, Route("spotify/login")]
    public IActionResult Login()
    {
        const string scope = "user-read-private user-read-email";

        var query = new QueryBuilder
        {
            {"response_type", "code"},
            {"client_id", ClientId},
            {"scope", scope},
            {"redirect_uri", RedirectUri},
        };

        return Redirect("https://accounts.spotify.com/authorize" + query);
    }
    
    [Authorize, Route("spotify/callback")]
    public async Task<IActionResult> Callback()
    {
        if (!Request.Query.TryGetValue("code", out var code))
        {
            throw new ArgumentNullException($"Spotify login callback is missing the code.");
        }

        var data = new Dictionary<string, string>
        {
            {"code", code.First()},
            {"grant_type", "authorization_code"},
            {"redirect_uri", RedirectUri},
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
        request.Content = new FormUrlEncodedContent(data);

        var authValue = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authValue);
        
        var response = await HttpClient.SendAsync(request);
        var spotifyTokenInfo = await response.Content.ReadFromJsonAsync<SpotifyCredentialsResponse>();

        if (spotifyTokenInfo == null)
        {
            throw new ArgumentNullException($"Spotify token info not deserialised correctly from Spotify response");
        }

        var (accessToken, tokenType, scope, expiresIn, refreshToken) = spotifyTokenInfo;

        // give 5 minutes of leeway
        var expiresAt = DateTime.UtcNow.AddSeconds(expiresIn - 60 * 5);
        var spotifyCredentials = new SpotifyCredentials(accessToken, tokenType, scope, expiresAt, refreshToken);
        await _spotifyService.UpsertSpotifyCredentials(User.GetLoggedInUser(), spotifyCredentials);
        
        return Redirect("https://localhost:44492/profile");
    }
    
    // From the spotify docs https://developer.spotify.com/documentation/general/guides/authorization/code-flow/
    private record SpotifyCredentialsResponse(
        [property: JsonPropertyName("access_token")] string AccessToken,
        [property: JsonPropertyName("token_type")] string TokenType,
        [property: JsonPropertyName("scope")] string Scope,
        [property: JsonPropertyName("expires_in")] int ExpiresIn,
        [property: JsonPropertyName("refresh_token")] string RefreshToken);
}