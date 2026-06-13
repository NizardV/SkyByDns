using Application.Dtos.Auth;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    /// <summary>
    /// Service implementation for user authentication operations.
    /// Handles user login and registration with JWT token generation.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<AuthService> _logger;

        /// <summary>
        /// Initializes a new instance of the AuthService with required dependencies.
        /// </summary>
        /// <param name="userRepository">Repository for user data access.</param>
        /// <param name="tokenGenerator">Service for generating JWT tokens.</param>
        /// <param name="passwordHasher">Service for password hashing and verification.</param>
        /// <param name="logger">Logger for authentication events.</param>
        public AuthService(
            IUserRepository userRepository,
            ITokenGenerator tokenGenerator,
            IPasswordHasher passwordHasher,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _tokenGenerator = tokenGenerator;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates a user with email and password.
        /// </summary>
        /// <param name="request">Login credentials containing email and password.</param>
        /// <returns>Login result with JWT token if successful, or error message if failed.</returns>
        public async Task<LoginResultDto> Login(LoginRequestDto request)
        {
            // Attempt to find the user by email
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("Login attempt failed for non-existing user: {Email}", request.Email);
                return new LoginResultDto
                {
                    Success = false,
                    ErrorMessage = "Invalid username or password."
                };
            }

            // Verify the provided password against the stored hash
            var valid = _passwordHasher.VerifyPassword(user.PasswordHash, request.Password);
            if (!valid)
            {
                _logger.LogWarning("Login attempt failed for user: {Email}", request.Email);
                return new LoginResultDto
                {
                    Success = false,
                    ErrorMessage = "Invalid username or password."
                };
            }

            // Generate JWT token for authenticated user
            var token = _tokenGenerator.GenerateToken(user);

            _logger.LogInformation("Login attempt successful for user: {Email}", request.Email);
            return new LoginResultDto
            {
                Success = true,
                Token = token
            };
        }

        /// <summary>
        /// Registers a new user account.
        /// Validates email uniqueness, hashes password, and creates user.
        /// </summary>
        /// <param name="registerRequestDto">Registration data including name, email, password, and role.</param>
        /// <returns>Login result with JWT token if successful, or error message if email already exists.</returns>
        public async Task<LoginResultDto> Register(RegisterRequestDto registerRequestDto)
        {
            // Check if email is already in use
            var existingUser = await _userRepository.GetByEmailAsync(registerRequestDto.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("Create account attempt failed - email already in use: {Email}", registerRequestDto.Email);
                return new LoginResultDto
                {
                    Success = false,
                    ErrorMessage = "Username already exists."
                };
            }

            // Hash the password before storing
            var hashedPassword = _passwordHasher.HashPassword(registerRequestDto.Password);

            // Create new user entity
            var newUser = new User
            {
                FirstName = registerRequestDto.FirstName,
                LastName = registerRequestDto.LastName,
                PasswordHash = hashedPassword,
                Email = registerRequestDto.Email,
            };

            // Assign role if provided
            if (!string.IsNullOrEmpty(registerRequestDto.Role))
            {
                newUser.Role = registerRequestDto.Role;
                _logger.LogTrace("Assigning role {Role} to new user: {Email}", registerRequestDto.Role, registerRequestDto.Email);
            }

            // Save user to database
            _logger.LogInformation("Registering user: {Email}", registerRequestDto.Email);
            await _userRepository.Add(newUser);
            _logger.LogInformation("User created successfully: {Email}", registerRequestDto.Email);

            // Generate JWT token for the new user
            var token = _tokenGenerator.GenerateToken(newUser);

            _logger.LogInformation("Login attempt successful for user: {Email}", registerRequestDto.Email);
            return new LoginResultDto
            {
                Success = true,
                Token = token
            };
        }
    }
}