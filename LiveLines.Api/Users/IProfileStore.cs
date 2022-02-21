using System;
using System.Threading.Tasks;

namespace LiveLines.Api.Users;

public interface IProfileStore
{
    Task<Profile> CreateProfile(LoggedInUser user);
    Task<Profile> UpdateProfile(LoggedInUser user, Privacy defaultPrivacy);
    Task<Profile> GetProfile(LoggedInUser user);
}