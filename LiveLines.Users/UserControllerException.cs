using System;

namespace LiveLines.Users;

internal class UserControllerException : Exception
{
    public UserControllerException(string msg) : base(msg)
    {
    }
}