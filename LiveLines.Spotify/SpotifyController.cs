using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LiveLines.Spotify;

[ApiController, Route("api")]
public class SpotifyController : ControllerBase
{
    private static readonly HttpClient HttpClient;

    static SpotifyController()
    {
        HttpClient = new HttpClient();
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
        var spotifyTokenInfo = await response.Content.ReadFromJsonAsync<SpotifyTokenInfo>();
        return Redirect("https://localhost:44492/profile");
    }
    
    /***
     * From the spotify docs https://developer.spotify.com/documentation/general/guides/authorization/code-flow/
     * 
     * access_token 	string 	An Access Token that can be provided in subsequent calls, for example to Spotify Web API services.
     * token_type 	string 	How the Access Token may be used: always “Bearer”.
     * scope 	string 	A space-separated list of scopes which have been granted for this access_token
     * expires_in 	int 	The time period (in seconds) for which the Access Token is valid.
     * refresh_token 	string 	A token that can be sent to the Spotify Accounts service in place of an authorization code. (When the access code expires, send a POST request to the Accounts service /api/token endpoint, but use this code in place of an authorization code. A new Access Token will be returned. A new refresh token might be returned too.)
     */
    private record SpotifyTokenInfo(
        [property: JsonPropertyName("access_token")] string AccessToken,
        [property: JsonPropertyName("token_type")] string TokenType,
        [property: JsonPropertyName("scope")] string Scope,
        [property: JsonPropertyName("expires_in")] int ExpiresIn,
        [property: JsonPropertyName("refresh_token")] string RefreshToken);
}