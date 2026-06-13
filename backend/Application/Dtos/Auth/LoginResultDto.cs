namespace Application.Dtos.Auth;

/// <summary>
/// Response DTO for login operations.
/// Contains authentication result and JWT token if successful.
/// </summary>
public class LoginResultDto
{
    /// <summary>
    /// Indicates whether the login was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// JWT authentication token for subsequent API requests.
    /// Null if login failed.
    /// </summary>
    /// <remarks>
    /// Include this token in the Authorization header as "Bearer {token}" for authenticated requests.
    /// </remarks>
    public string? Token { get; set; }

    /// <summary>
    /// Error message if login failed.
    /// Null if login was successful.
    /// </summary>
    public string? ErrorMessage { get; set; }
}
