using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiveLines.Authentication;

[ApiController, Route("api")]
public class AuthenticationController : ControllerBase
{
    [HttpGet, Route("login")]
    public IActionResult Login(string returnUrl = "/")
    {
        return Challenge(new AuthenticationProperties { RedirectUri = returnUrl });
    }

    [Authorize]
    [HttpGet, Route("authenticated")]
    public IActionResult Authenticated()
    {
        return Ok();
    }
}