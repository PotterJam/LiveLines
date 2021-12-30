using System;

namespace LiveLines.Users;

internal class UserStoreException : Exception
{
    public UserStoreException(string msg) : base(msg)
    {
    }
}