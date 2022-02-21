using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Extensions;
using LiveLines.Api.Lines;
using LiveLines.Api.Streaks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiveLines.Lines;

[Authorize]
[ApiController, Route("api")]
public class LinesController : ControllerBase
{
    private readonly ILinesService _linesService;
    private readonly IStreakService _streakService;

    public LinesController(ILinesService linesService, IStreakService streakService)
    {
        _linesService = linesService;
        _streakService = streakService;
    }

    public record LineResponse(Guid Id, string Message, string? SpotifyId, DateTime DateFor);
    
    [HttpGet, Route("lines")]
    public async Task<IEnumerable<LineResponse>> FetchLines(Privacy privacy)
    {
        var user = User.GetLoggedInUser();

        var lines = (await _linesService.GetLines(user, privacy))
            .OrderByDescending(x => x.DateFor);
        
        return lines.Select(line => new LineResponse(line.Id, line.Message, line.SpotifyId, line.DateFor));
    }

    public record LineRequest(string Message, string? SongId, bool ForYesterday, Privacy Privacy);

    [HttpPost, Route("line")]
    public async Task<LineResponse> CreateLine([FromBody] LineRequest lineRequest)
    {
        var user = User.GetLoggedInUser();

        var lineToCreate = new LineToCreate(lineRequest.Message, lineRequest.SongId, lineRequest.ForYesterday, lineRequest.Privacy);
        var line = await _linesService.CreateLine(user, lineToCreate);

        await _streakService.IncrementStreak(user);

        return new LineResponse(line.Id, line.Message, line.SpotifyId, line.DateFor);
    }

    public record LineOperationsResponse(bool CanPostToday, bool CanPostYesterday);

    [HttpGet, Route("line/operations")]
    public async Task<LineOperationsResponse> FetchLineOperations()
    {
        var user = User.GetLoggedInUser();
        var operations = await _linesService.GetLineOperations(user);
        return new LineOperationsResponse(operations.CanPostToday, operations.CanPostYesterday);
    }
}