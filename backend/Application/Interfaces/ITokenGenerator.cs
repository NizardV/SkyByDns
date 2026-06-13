using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Interface for JWT token generation.
/// Provides authentication tokens for user sessions.
/// </summary>
public interface ITokenGenerator
{
    /// <summary>
    /// Generates a JWT authentication token for the specified user.
    /// </summary>
    /// <param name="user">The user entity to generate a token for.</param>
    /// <returns>A JWT token string containing user claims (ID, email, role).</returns>
    /// <remarks>
    /// The token should be included in the Authorization header as "Bearer {token}"
    /// for authenticated API requests.
    /// </remarks>
    string GenerateToken(User user);
}