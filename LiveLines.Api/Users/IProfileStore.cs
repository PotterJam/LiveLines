using System;
using System.Threading.Tasks;

namespace LiveLines.Api.Users;

public interface IProfileStore
{
    Task<Profile> UpsertProfile(LoggedInUser user, Privacy defaultPrivacy);
    Task<Profile> GetProfile(LoggedInUser user);
}