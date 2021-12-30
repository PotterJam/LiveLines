using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Extensions;
using LiveLines.Api;
using LiveLines.Api.Lines;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiveLines.Lines;

[Authorize]
[ApiController, Route("api")]
public class LinesController : ControllerBase
{
    private readonly ILinesService _linesService;

    public LinesController(ILinesService linesService)
    {
        _linesService = linesService;
    }

    public record LineResponse(Guid Id, string Message, string? SpotifyId, DateTime CreatedAt);
    
    [HttpGet, Route("lines")]
    public async Task<IEnumerable<LineResponse>> FetchLines()
    {
        var user = User.GetLoggedInUser();
        var lines = await _linesService.GetLines(user);
        return lines.Select(line => new LineResponse(line.Id, line.Message, line.SpotifyId, line.CreatedAt));
    }

    public record LineRequest(string Message, string? SongId);

    [HttpPost, Route("line")]
    public async Task<LineResponse> CreateLine([FromBody] LineRequest lineRequest)
    {
        var user = User.GetLoggedInUser();

        var lineToCreate = new LineToCreate(lineRequest.Message, lineRequest.SongId);
        var line = await _linesService.CreateLine(user, lineToCreate);
        return new LineResponse(line.Id, line.Message, line.SpotifyId, line.CreatedAt);
    }
}