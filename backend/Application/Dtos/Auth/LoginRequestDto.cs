using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Auth;

/// <summary>
/// Request DTO for user login.
/// Contains credentials required for authentication.
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// User's email address for authentication.
    /// Must be a valid registered email.
    /// </summary>
    /// <example>user@example.com</example>
    [Required]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User's password in plain text.
    /// Will be verified against the stored password hash.
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;
}