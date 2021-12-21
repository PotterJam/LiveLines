using System;
using System.Security.Claims;
using LiveLines.Api.Users;

namespace Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static User GetUser(this ClaimsPrincipal claimsPrincipal)
        {
            throw new NotImplementedException();
        }
    }
}