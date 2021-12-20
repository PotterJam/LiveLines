using System.Collections.Generic;
using System.Threading.Tasks;
using Extensions;
using LiveLines.Api.Lines;
using Microsoft.AspNetCore.Mvc;

namespace LiveLines.Lines
{
    [ApiController, Route("api")]
    public class LinesController : ControllerBase
    {
        private readonly ILinesService _linesService;

        public LinesController(ILinesService linesService)
        {
            _linesService = linesService;
        }
        
        [HttpGet, Route("lines")]
        public async Task<IEnumerable<Line>> FetchLines()
        {
            var user = User.GetUser();
            return await _linesService.GetLines(user);
        }
        
        public record LineRequest(string Message);
        
        [HttpGet, Route("line")]
        public async Task<Line> CreateLine([FromBody] LineRequest lineRequest)
        {
            var user = User.GetUser();
            var newLine = new Line(lineRequest.Message);
            return await _linesService.CreateLine(user, newLine);
        }
    }
}