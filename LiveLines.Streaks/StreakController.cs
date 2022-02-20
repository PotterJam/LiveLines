using Extensions;
using LiveLines.Api.Streaks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiveLines.Streaks;

[Authorize]
[ApiController, Route("api")]
public class StreakController : ControllerBase
{
    private readonly IStreakService _streakService;

    public StreakController(IStreakService streakService)
    {
        _streakService = streakService;
    }

    [HttpGet, Route("streak")]
    public async Task<int> GetStreak()
    {
        return await _streakService.GetStreak(User.GetLoggedInUser());
    }
}