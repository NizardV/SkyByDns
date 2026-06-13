using Application.Dtos.Auth;

namespace Application.Interfaces
{
    /// <summary>
    /// Service interface for user authentication operations.
    /// Handles user login and registration with JWT token generation.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user with email and password.
        /// </summary>
        /// <param name="loginRequestDto">Login credentials containing email and password.</param>
        /// <returns>
        /// A LoginResultDto containing:
        /// - Success flag
        /// - JWT token if successful
        /// - Error message if failed
        /// </returns>
        Task<LoginResultDto> Login(LoginRequestDto loginRequestDto);

        /// <summary>
        /// Registers a new user account.
        /// </summary>
        /// <param name="registerRequestDto">Registration data including name, email, password, and role.</param>
        /// <returns>
        /// A LoginResultDto containing:
        /// - Success flag
        /// - JWT token if successful
        /// - Error message if failed (e.g., email already exists)
        /// </returns>
        Task<LoginResultDto> Register(RegisterRequestDto registerRequestDto);
    }
}