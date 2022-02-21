using System.Threading.Tasks;
using Extensions;
using LiveLines.Api;
using LiveLines.Api.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiveLines.Users;

[Authorize]
[ApiController, Route("api")]
public class UserController : ControllerBase
{
    private readonly IProfileService _profileService;

    public UserController(IProfileService profileService)
    {
        _profileService = profileService;
    }
    
    public record ProfileResponse(string Username, Privacy DefaultPrivacy);
    
    [HttpGet, Route("user/profile")]
    public async Task<ProfileResponse> GetProfile()
    {
        var user = User.GetLoggedInUser();
        var profile = await _profileService.GetProfile(user);
        
        return new ProfileResponse(user.Username, profile.DefaultPrivacy);
    }
}
