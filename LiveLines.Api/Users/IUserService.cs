using System.Threading.Tasks;

namespace LiveLines.Api.Users
{
    public interface IUserService
    {
        Task<User> CreateUser(string provider, string username);
        Task<User> GetUser(string username);
        Task<User> GetUser(int userId);
    }
}