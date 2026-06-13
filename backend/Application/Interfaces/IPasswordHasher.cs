namespace Application.Interfaces;

/// <summary>
/// Interface for password hashing and verification.
/// Provides secure password storage and validation using Identity's password hasher.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Hashes a plain text password for secure storage.
    /// </summary>
    /// <param name="password">The plain text password to hash.</param>
    /// <returns>A hashed password string safe for database storage.</returns>
    /// <remarks>
    /// Uses Microsoft.AspNetCore.Identity.PasswordHasher for secure hashing.
    /// Never store passwords in plain text.
    /// </remarks>
    string HashPassword(string password);

    /// <summary>
    /// Verifies a plain text password against a stored hash.
    /// </summary>
    /// <param name="hashedPassword">The stored password hash from the database.</param>
    /// <param name="providedPassword">The plain text password to verify.</param>
    /// <returns>True if the password matches the hash, false otherwise.</returns>
    bool VerifyPassword(string hashedPassword, string providedPassword);
}
