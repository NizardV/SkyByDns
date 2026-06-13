using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Auth;

/// <summary>
/// Request DTO for user registration.
/// Contains all required information to create a new user account.
/// </summary>
public class RegisterRequestDto
{
    /// <summary>
    /// User's first name (given name).
    /// </summary>
    /// <example>John</example>
    [Required]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// User's last name (surname/family name).
    /// </summary>
    /// <example>Doe</example>
    [Required]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// User's password in plain text.
    /// Will be hashed before storage using Identity's password hasher.
    /// </summary>
    /// <remarks>
    /// Ensure this meets password strength requirements in production.
    /// </remarks>
    [Required]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// User's email address.
    /// Must be unique across the system.
    /// </summary>
    /// <example>john.doe@example.com</example>
    [Required]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User's role in the system.
    /// Defaults to "User" if not specified.
    /// </summary>
    /// <remarks>
    /// Common roles: "User", "Admin"
    /// </remarks>
    /// <example>User</example>
    [DefaultValue("User")]
    public string? Role { get; set; } = "User";
}