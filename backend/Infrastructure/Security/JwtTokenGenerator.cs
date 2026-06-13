using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security;

/// <summary>
/// Implementation of JWT token generation for user authentication.
/// Creates JWT tokens with user claims for API authorization.
/// </summary>
public class JwtTokenGenerator : ITokenGenerator
{
    private readonly JwtOptions _options;

    /// <summary>
    /// Initializes a new instance of the JwtTokenGenerator with JWT configuration.
    /// </summary>
    /// <param name="options">JWT configuration options from appsettings.json.</param>
    public JwtTokenGenerator(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    /// <summary>
    /// Generates a JWT authentication token for the specified user.
    /// Token includes user ID, email, and role claims.
    /// </summary>
    /// <param name="user">The user entity to generate a token for.</param>
    /// <returns>A JWT token string containing user claims.</returns>
    /// <remarks>
    /// Token structure:
    /// - Header: Algorithm (HS256) and token type (JWT)
    /// - Payload: Claims (user ID, email, role) and expiration
    /// - Signature: HMAC-SHA256 signature using secret key
    /// 
    /// Include this token in API requests: Authorization: Bearer {token}
    /// </remarks>
    public string GenerateToken(User user)
    {
        // Create symmetric security key from secret
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Build claims for the token
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        // Create JWT token with claims and expiration
        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiresInMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}