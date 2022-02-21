using System.Threading.Tasks;
using Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiveLines.Users;

[Authorize]
[ApiController, Route("api")]
public class UserController : ControllerBase
{
    public record ProfileResponse(string Username);
    
    [HttpGet, Route("user/profile")]
    public Task<ProfileResponse> GetProfile()
    {
        var user = User.GetLoggedInUser();
        return Task.FromResult(new ProfileResponse(user.Username));
    }
}
