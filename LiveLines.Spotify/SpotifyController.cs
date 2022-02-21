using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace LiveLines.Spotify;

[ApiController, Route("api")]
public class SpotifyController : ControllerBase
{

    private string GenerateRandomString(int length)
    {
        var bitCount = length * 6;
        var byteCount = (bitCount + 7) / 8; // rounded up
        var bytes = new byte[byteCount];
        RandomNumberGenerator.Create().GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
    
    [Authorize, Route("spotify/login")]
    public IActionResult Login()
    {
        var csrfToken = GenerateRandomString(16);
        Response.Cookies.Append(StateKey, csrfToken);
        
        const string scope = "user-read-private user-read-email";

        var query = new QueryBuilder
        {
            {"response_type", "code"},
            {"client_id", ClientId},
            {"scope", scope},
            {"redirect_uri", RedirectUri},
            {"state", csrfToken}
        };

        return Redirect("https://accounts.spotify.com/authorize" + query);
    }
    
    [AllowAnonymous, Route("spotify/callback")]
    public Task Callback()
    {
        var body = Request.Body;
        var request = Request;
        return Task.CompletedTask;
    }
}