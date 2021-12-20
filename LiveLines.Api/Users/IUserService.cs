using System.Threading.Tasks;

namespace LiveLines.Api.Users
{
    public interface IUserService
    {
        Task<User> CreateUser(string email);
        Task<User> GetUser(string email);
        Task<User> GetUser(int userId);
    }
}