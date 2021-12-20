using System.Collections.Generic;
using System.Threading.Tasks;
using LiveLines.Api.Database;
using LiveLines.Api.Lines;
using LiveLines.Api.Users;

namespace LiveLines.Lines
{
    public class LinesStore : ILinesStore
    {
        private readonly IDatabaseCommandExecutor _dbExecutor;

        public LinesStore(IDatabaseCommandExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public Task<IEnumerable<Line>> GetLines(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task<Line> CreateLine(User user, Line line)
        {
            throw new System.NotImplementedException();
        }
    }
}