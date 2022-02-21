using System.Threading.Tasks;

namespace LiveLines.Api.Users;

public interface IProfileService
{
    Task<Profile> CreateProfile(LoggedInUser user);
    Task<Profile> UpdateProfile(LoggedInUser user, ProfileToUpdate profileToUpdate);
    Task<Profile> GetProfile(LoggedInUser user);
}