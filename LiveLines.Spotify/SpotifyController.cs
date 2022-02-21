using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiveLines.Spotify;

[ApiController, Route("api")]
public class SpotifyController : ControllerBase
{
    [AllowAnonymous]
    public Task Callback()
    {
        return Task.CompletedTask;
    }
}