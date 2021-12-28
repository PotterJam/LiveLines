using System.Threading.Tasks;

namespace LiveLines.Api.Users;

public interface IUserService
{
    Task<LoggedInUser> CreateUser(string provider, string username);
    Task<LoggedInUser> GetUser(string username);
    Task<LoggedInUser> GetUser(int userId);
}