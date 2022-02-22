using System.Threading.Tasks;
using Extensions;
using LiveLines.Api.Spotify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiveLines.Users;

[Authorize]
[ApiController, Route("api")]
public class UserController : ControllerBase
{
    private readonly ISpotifyService _spotifyService;

    public UserController(ISpotifyService spotifyService)
    {
        _spotifyService = spotifyService;
    }

    public record ProfileResponse(string Username, bool SpotifyLoggedIn);
    
    [HttpGet, Route("user/profile")]
    public async Task<ProfileResponse> GetProfile()
    {
        var user = User.GetLoggedInUser();
        
        var spotifyCredentials = await _spotifyService.GetSpotifyCredentials(User.GetLoggedInUser());
        var hasSpotifyCreds = spotifyCredentials != null;
        
        return new ProfileResponse(user.Username, hasSpotifyCreds);
    }
}
