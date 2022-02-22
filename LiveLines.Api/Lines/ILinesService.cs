using System.Collections.Generic;
using System.Threading.Tasks;
using LiveLines.Api.Users;

namespace LiveLines.Api.Lines;

public interface ILinesService
{
    Task<IEnumerable<Line>> GetLines(LoggedInUser loggedInUser);
    Task<IEnumerable<Line>> GetLinesWithPrivacy(LoggedInUser loggedInUser, LinePrivacy privacy);
    Task<Line> CreateLine(LoggedInUser loggedInUser, LineToCreate lineToCreate);
    Task<LineOperations> GetLineOperations(LoggedInUser loggedInUser);
}