using System.Threading.Tasks;
using LiveLines.Api.Users;

namespace LiveLines.Users;

public class UserService : IUserService
{
    private readonly IUserStore _userStore;
    private readonly IProfileStore _profileStore;

    public UserService(IUserStore userStore, IProfileStore profileStore)
    {
        _userStore = userStore;
        _profileStore = profileStore;
    }
        
    public async Task<LoggedInUser> CreateUser(string provider, string username)
    {
        LoggedInUser user = await _userStore.CreateUser(provider, username);
        await _profileStore.CreateProfile(user);
        
        return user;
    }

    public async Task<LoggedInUser> GetUser(string username)
    {
        return await _userStore.GetUser(username);
    }
        
    public async Task<LoggedInUser> GetUser(int userId)
    {
        return await _userStore.GetUser(userId);
    }

    public async Task<Profile> UpdateProfile(LoggedInUser user, ProfileToUpdate profileToUpdate)
    {
        return await _profileStore.UpdateProfile(user, profileToUpdate.LinePrivacy);
    }

    public async Task<Profile> GetProfile(LoggedInUser user)
    {
        return await _profileStore.GetProfile(user);
    }
}