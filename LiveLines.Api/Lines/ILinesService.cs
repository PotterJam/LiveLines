using System.Collections.Generic;
using System.Threading.Tasks;
using LiveLines.Api.Users;

namespace LiveLines.Api.Lines;

public interface ILinesService
{
    Task<IEnumerable<Line>> GetLines(LoggedInUser loggedInUser);
    Task<Line> CreateLine(LoggedInUser loggedInUser, LineToCreate lineToCreate);
}