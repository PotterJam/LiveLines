using System;
using System.Threading.Tasks;
using LiveLines.Api.Database;
using LiveLines.Api.Users;

namespace LiveLines.Users
{
    public class UserStore : IUserStore
    {
        private readonly IDatabaseCommandExecutor _dbExecutor;

        public UserStore(IDatabaseCommandExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public Task<User> CreateUser(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUser(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUser(int userId)
        {
            throw new NotImplementedException();
        }
    }
}