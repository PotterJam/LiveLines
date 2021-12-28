using System;

namespace LiveLines.Api.Users;

public record LoggedInUser(Guid InternalId, string Username);