using System;

namespace LiveLines.Api.Spotify;

public record SpotifyCredentials(
    string AccessToken,
    string TokenType,
    string Scope,
    DateTime ExpiresAt,
    string RefreshToken);