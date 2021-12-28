using System.Threading.Tasks;

namespace LiveLines.Api.Users;

public interface IUserStore
{
    Task<LoggedInUser> CreateUser(string provider, string username);
    Task<LoggedInUser> GetUser(string username);
    Task<LoggedInUser> GetUser(int userId);
}