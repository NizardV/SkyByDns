using Microsoft.AspNetCore.Identity;
using Application.Interfaces;

namespace Infrastructure.Security;

/// <summary>
/// Implementation of password hashing using ASP.NET Core Identity's PasswordHasher.
/// Provides secure password hashing and verification for user authentication.
/// </summary>
public class IdentityPasswordHasher : IPasswordHasher
{
    /// <summary>
    /// Internal password hasher from ASP.NET Core Identity.
    /// Uses PBKDF2 with HMAC-SHA256, 128-bit salt, 256-bit subkey, 10000 iterations.
    /// </summary>
    private readonly PasswordHasher<object> _inner = new();

    /// <summary>
    /// Hashes a plain text password for secure storage.
    /// </summary>
    /// <param name="password">The plain text password to hash.</param>
    /// <returns>A hashed password string safe for database storage.</returns>
    public string HashPassword(string password)
    {
        return _inner.HashPassword(user: null!, password);
    }

    /// <summary>
    /// Verifies a plain text password against a stored hash.
    /// </summary>
    /// <param name="hashedPassword">The stored password hash from the database.</param>
    /// <param name="providedPassword">The plain text password to verify.</param>
    /// <returns>True if the password matches the hash, false otherwise.</returns>
    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        var result = _inner.VerifyHashedPassword(user: null!, hashedPassword, providedPassword);
        return result != PasswordVerificationResult.Failed;
    }
}