using System;

namespace LiveLines.Api.Lines
{
    public record Line(int Id, string Message, DateTime CreatedAt);
}