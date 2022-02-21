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

        await _spotifyService.UpsertSpotifyCredentials(
            user: User.GetLoggedInUser(),
            code: code.First(),
            redirectUri: RedirectUri,
            clientId: ClientId,
            clientSecret: ClientSecret);
        
        return Redirect("https://localhost:44492/profile");
    }
}