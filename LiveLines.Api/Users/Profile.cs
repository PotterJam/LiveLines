using System;

namespace LiveLines.Api.Users;

public record Profile(Guid InternalId, Guid UserId, Privacy DefaultPrivacy);