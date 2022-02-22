namespace LiveLines.Api.Lines;

public record LineToCreate(string Body, string? SpotifySongId, bool ForYesterday, LinePrivacy Privacy);