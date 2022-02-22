using System;
using LiveLines.Api.Lines;

namespace LiveLines.Api.Users;

public record Profile(Guid InternalId, Guid UserId, LinePrivacy LinePrivacy);