using System;

namespace LiveLines.Lines;

public class LinesStoreException : Exception
{
    public LinesStoreException(string msg) : base(msg)
    {
    }
}