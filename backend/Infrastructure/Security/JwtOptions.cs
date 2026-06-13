namespace Infrastructure.Security;

/// <summary>
/// Configuration options for JWT token generation.
/// Loaded from appsettings.json under "Jwt" section.
/// </summary>
public class JwtOptions
{
    /// <summary>
    /// Secret key used to sign JWT tokens.
    /// Must be at least 256 bits (32 characters) for HS256 algorithm.
    /// </summary>
    /// <remarks>
    /// Keep this key secure and never commit it to source control.
    /// Use environment variables or Azure Key Vault in production.
    /// </remarks>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Issuer claim (iss) for JWT tokens.
    /// Identifies who issued the token.
    /// </summary>
    /// <example>https://api.example.com</example>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Audience claim (aud) for JWT tokens.
    /// Identifies who the token is intended for.
    /// </summary>
    /// <example>https://api.example.com</example>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Token expiration time in minutes.
    /// Defaults to 60 minutes (1 hour).
    /// </summary>
    public int ExpiresInMinutes { get; set; } = 60;
}