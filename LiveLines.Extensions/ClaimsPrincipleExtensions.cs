﻿using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using LiveLines.Api.Users;
using LiveLines.Security;

namespace Extensions;

public static class ClaimsPrincipleExtensions
{
    public static User GetLoggedInUser(this ClaimsPrincipal claimsPrincipal)
    {
        var internalIdStr = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == LoggedInClaims.InternalUserId)?.Value;
            
        if (internalIdStr == null)
            throw new InvalidCredentialException($"Can't get internal user id claim for user {claimsPrincipal.Identity?.Name ?? "Unknown"}. Should be logged in at this point.");
            
        if (!int.TryParse(internalIdStr, out var internalId))
            throw new InvalidCredentialException($"Couldn't parse internal id string to int: {internalIdStr}");
            
        var username = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == LoggedInClaims.Username)?.Value;
            
        if (username == null)
            throw new InvalidCredentialException($"Can't get username claim for user {claimsPrincipal.Identity?.Name ?? "Unknown"}. Should be logged in at this point.");

        return new User(internalId, username);
    }
}