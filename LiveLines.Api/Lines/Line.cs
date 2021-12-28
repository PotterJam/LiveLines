using System;

namespace LiveLines.Api.Lines;

public record Line(Guid Id, string Message, DateTime CreatedAt);