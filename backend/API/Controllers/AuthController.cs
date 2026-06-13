using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.Dtos;
using Application.Dtos.Auth;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    /// <summary>
    /// Controller for managing user authentication.
    /// Handles user login and registration operations, including JWT token generation.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <param name="request">Login request containing email and password.</param>
        /// <returns>JWT token</returns>
        /// <response code="200">Authentication successful.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error),StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Login([FromBody] LoginRequestDto request)
        {
            _logger.LogTrace("Login attempt for user: {Email}", request.Email);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid request: {Email}", request.Email);
                return BadRequest(ModelState);
            }

            var result = await _authService.Login(request);

            if (!result.Success || string.IsNullOrEmpty(result.Token))
            {
                _logger.LogWarning("Unauthorized login attempt for user: {Email}", request.Email);
                return Unauthorized(new Error(result.ErrorMessage));
            }

            _logger.LogInformation("User logged in successfully: {Email}", request.Email);
            return Ok(result);
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">Registration request containing user details.</param>
        /// <returns>201 Created with the URL of the new user.</returns>
        /// <response code="201">User registered successfully.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(LoginResultDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Error),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            _logger.LogTrace("Registration attempt for user: {Email}", request.Email);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid registration request: {Email}", request.Email);
                return BadRequest(ModelState);
            }

            var createdUser = await _authService.Register(request);

            if (!createdUser.Success)
            {
                _logger.LogWarning("Registration failed for user: {Email}. Reason: {ErrorMessage}", request.Email, createdUser.ErrorMessage);
                return BadRequest(new Error(createdUser.ErrorMessage));
            }

            _logger.LogInformation("User registered successfully: {Email}", request.Email);
            return CreatedAtAction(nameof(Register), createdUser, createdUser);
        }
    }
}