using System.Threading.Tasks;

namespace LiveLines.Api.Users;

public interface IUserService
{
    Task<LoggedInUser> CreateUser(string provider, string username);
    Task<LoggedInUser?> GetUser(string provider, string username);
    Task<LoggedInUser> GetUser(int userId);
    Task<Profile> UpdateProfile(LoggedInUser user, ProfileToUpdate profileToUpdate);
    Task<Profile> GetProfile(LoggedInUser user);
}