using System.Threading.Tasks;
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
        return await _profileStore.CreateProfile(user);
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