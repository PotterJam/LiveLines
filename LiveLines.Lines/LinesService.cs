using System.Collections.Generic;
using System.Threading.Tasks;
using LiveLines.Api.Lines;
using LiveLines.Api.Users;

namespace LiveLines.Lines;

public class LinesService : ILinesService
{
    private readonly ILinesStore _linesStore;

    public LinesService(ILinesStore linesStore)
    {
        _linesStore = linesStore;
    }

    public async Task<IEnumerable<Line>> GetLines(LoggedInUser loggedInUser)
    {
        return await _linesStore.GetLines(loggedInUser);
    }

    public async Task<Line> CreateLine(LoggedInUser loggedInUser, string body)
    { 
        return await _linesStore.CreateLine(loggedInUser, body);
    }
}