namespace Application.Dtos.Auth;

/// <summary>
/// Response DTO containing user information.
/// Used for returning user details without sensitive data like passwords.
/// </summary>
public class UserDto
{
    /// <summary>
    /// User's unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// User's first name.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// User's last name.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// User's email address.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// User's role in the system (e.g., "User", "Admin").
    /// </summary>
    public string Role { get; set; }
}