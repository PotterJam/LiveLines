using System;

namespace LiveLines.Api.Lines;

public record Line(Guid Id, string Message, string? SpotifyId, DateTime DateFor, Privacy Privacy);