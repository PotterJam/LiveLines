using System;
using System.Threading.Tasks;
using Extensions;
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

        public async Task<User> CreateUser(string provider, string username)
        {
            return await _dbExecutor.ExecuteCommand(async cmd =>
            {
                cmd.AddParam("@provider", provider);
                cmd.AddParam("@username", username);

                cmd.CommandText = @"
                    WITH new_user AS (
                        INSERT INTO users (provider, username)
                        VALUES (@provider, @username)
                        ON CONFLICT(username) DO UPDATE
                            SET last_login = NOW()
                        RETURNING id
                    ) SELECT COALESCE(
                        (SELECT id FROM new_user),
                        (SELECT id FROM users WHERE username = @username)
                    ) AS id;";

                var id = (int?) await cmd.ExecuteScalarAsync();

                if (id == null)
                {
                    throw new UserStoreException("Tried to create user, nothing got returned");
                }

                return new User(id.Value, username);
            });
        }

        public Task<User> GetUser(string username)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUser(int userId)
        {
            throw new NotImplementedException();
        }
    }
}