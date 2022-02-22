using System;
using System.Threading.Tasks;
using Extensions;
using LiveLines.Api.Spotify;
using LiveLines.Api.Lines;
using LiveLines.Api.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiveLines.Users;

[Authorize]
[ApiController, Route("api")]
public class UserController : ControllerBase
{
    private readonly ISpotifyService _spotifyService;
    private readonly IUserService _userService;

    public UserController(ISpotifyService spotifyService, IUserService userService)
    {
        _spotifyService = spotifyService;
        _userService = userService;
    }

    public record FetchProfileResponse(string Username, bool SpotifyLoggedIn, string LinePrivacy);
    
    [HttpGet, Route("user/profile")]
    public async Task<FetchProfileResponse> GetProfile()
    {
        var user = User.GetLoggedInUser();
        var profile = await _userService.GetProfile(user);

        var spotifyCredentials = await _spotifyService.GetSpotifyCredentials(User.GetLoggedInUser());
        var hasSpotifyCreds = spotifyCredentials != null;
        
        return new FetchProfileResponse(user.Username, hasSpotifyCreds, profile.LinePrivacy.ToString());
    }
    
    public record ProfileRequest(string LinePrivacy);
    public record UpdateProfileResponse(string Username, string LinePrivacy);

    [HttpPost, Route("user/profile")]
    public async Task<UpdateProfileResponse> UpdateProfile([FromBody] ProfileRequest profileRequest)
    {
        var user = User.GetLoggedInUser();
        var linePrivacy = Enum.Parse<LinePrivacy>(profileRequest.LinePrivacy);
        
        var profileToUpdate = new ProfileToUpdate(linePrivacy);
        var profile = await _userService.UpdateProfile(user, profileToUpdate);

        return new UpdateProfileResponse(user.Username, profile.LinePrivacy.ToString());
    }
}
