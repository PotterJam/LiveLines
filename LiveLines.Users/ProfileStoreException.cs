using System;

namespace LiveLines.Users;

internal class ProfileStoreException : Exception
{
    public ProfileStoreException(string msg) : base(msg)
    {
    }
}