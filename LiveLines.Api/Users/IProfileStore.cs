﻿using System.Threading.Tasks;
using LiveLines.Api.Lines;

namespace LiveLines.Api.Users;

public interface IProfileStore
{
    Task<Profile> CreateProfile(LoggedInUser user);
    Task<Profile> UpdateProfile(LoggedInUser user, Privacy linePrivacy);
    Task<Profile> GetProfile(LoggedInUser user);
}