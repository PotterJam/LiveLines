using System.Threading.Tasks;
using LiveLines.Api;
using LiveLines.Api.Users;

namespace LiveLines.Users;

public class ProfileService : IProfileService
{
    private readonly IProfileStore _profileStore;

    public ProfileService(IProfileStore profileStore)
    {
        _profileStore = profileStore;
    }

    public async Task<Profile> CreateProfile(LoggedInUser user)
    {
        return await _profileStore.UpsertProfile(user, Privacy.Private);
    }

    public async Task<Profile> UpdateProfile(LoggedInUser user, Privacy defaultPrivacy)
    {
        return await _profileStore.UpsertProfile(user, defaultPrivacy);
    }

    public async Task<Profile> GetProfile(LoggedInUser user)
    {
        return await _profileStore.GetProfile(user);
    }
}