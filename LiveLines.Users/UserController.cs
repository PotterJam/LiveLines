using System;
using System.Threading.Tasks;
using Extensions;
using LiveLines.Api.Lines;
using LiveLines.Api.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiveLines.Users;

[Authorize]
[ApiController, Route("api")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    public record ProfileResponse(string Username, string LinePrivacy);
    
    [HttpGet, Route("user/profile")]
    public async Task<ProfileResponse> GetProfile()
    {
        var user = User.GetLoggedInUser();
        var profile = await _userService.GetProfile(user);
        
        return new ProfileResponse(user.Username, profile.LinePrivacy.ToString());
    }
    
    public record ProfileRequest(string LinePrivacy);
    
    [HttpPost, Route("user/profile")]
    public async Task<ProfileResponse> UpdateProfile([FromBody] ProfileRequest profileRequest)
    {
        var user = User.GetLoggedInUser();
        var linePrivacy = Enum.Parse<LinePrivacy>(profileRequest.LinePrivacy);
        
        var profileToUpdate = new ProfileToUpdate(linePrivacy);
        var profile = await _userService.UpdateProfile(user, profileToUpdate);
        
        return new ProfileResponse(user.Username, profile.LinePrivacy.ToString());
    }
}
