using System.Threading.Tasks;
using LiveLines.Api.Users;

namespace LiveLines.Users;

public class UserService : IUserService
{
    private readonly IUserStore _userStore;

    public UserService(IUserStore userStore)
    {
        _userStore = userStore;
    }
        
    public async Task<User> CreateUser(string provider, string username)
    {
        return await _userStore.CreateUser(provider, username);
    }
        
    public async Task<User> GetUser(string username)
    {
        return await _userStore.GetUser(username);
    }
        
    public async Task<User> GetUser(int userId)
    {
        return await _userStore.GetUser(userId);
    }
}