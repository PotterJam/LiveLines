using System;

namespace LiveLines.Users;

public class UserStoreException : Exception
{
    public UserStoreException(string msg) : base(msg)
    {
    }
}