using System.Security.Claims;
using Application.Dtos;
using Application.Dtos.Domain;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Controller for managing DNS domains.
/// Handles domain registration, availability checks, and CRUD operations for user domains.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class DomainsController : ControllerBase
{
    private readonly IDomainService _domainService;
    private readonly ILogger<DomainsController> _logger;

    public DomainsController(IDomainService domainService, ILogger<DomainsController> logger)
    {
        _domainService = domainService;
        _logger = logger;
    }

    /// <summary>
    /// Declare a new domain.
    /// </summary>
    /// <param name="domain">The domain creation details.</param>
    /// <returns>The created domain object.</returns>
    /// <response code="201">Domain created successfully.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost]
    [EndpointSummary("Create a domain")]
    [EndpointDescription("Allows you to declare a new domain")]
    [ProducesResponseType(typeof(DomainDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest, Description = "Fields is missing or invalid")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<ActionResult<DomainDto>> CreateDomain([FromBody] CreateDomainDto domain)
    {
        // Method entry log
        _logger.LogTrace("Create domain attempt: {Domain}", domain);

        if (!ModelState.IsValid)
        {
            // Failure method log
            _logger.LogWarning("Invalid domain creation request: {Domain}", domain);
            return BadRequest(ModelState);
        }

        try {
            var userId = GetUserIdOrThrow();
            var createdDomain = await _domainService.CreateAsync(domain, userId);

            // Success method log 
            _logger.LogInformation("Domain created successfully: {DomainName}", domain.Name);

            return CreatedAtAction(
                nameof(GetDomains), createdDomain);
        }
        catch (Exception ex) {
            // Exception method log
            _logger.LogError(ex, "Error occurred while creating domain: {Domain}", domain);
            return StatusCode(StatusCodes.Status500InternalServerError, new Error("An error occurred while creating the domain."));
        }
    }

        /// <summary>
        /// Check if domain is available.
        /// </summary>
        /// <param name="domainName">The domain name to check.</param>
        /// <returns>Domain availability status.</returns>
        /// <response code="200">Domain availability checked successfully.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("check")]
        [ProducesResponseType(typeof(DomainAvailabilityDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CheckDomainAvailability([FromQuery] string domainName)
        {
            _logger.LogTrace("Check domain attempt: {DomainName}", domainName);
            if (string.IsNullOrWhiteSpace(domainName))
            {
                _logger.LogWarning("Domain name is required for availability check.");
                return BadRequest(new Error("Domain name is required."));
            }

            var availabilityDto = await _domainService.CheckAvailabilityAsync(domainName);

            _logger.LogInformation("Domain availability checked: {DomainName}, Available: {IsAvailable}", domainName, availabilityDto.IsAvailable);
            return Ok(availabilityDto);
        }

        /// <summary>
        /// Get all domains for the authenticated user.
        /// </summary>
        /// <returns>List of domains.</returns>
        /// <response code="200">Domains retrieved successfully.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<DomainsListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<ActionResult> GetDomains()
        {
            _logger.LogTrace("Get domains attempt for user.");
            var userId = GetUserIdOrThrow();

            // NOTE: IDomainService.GetAllAsync currently returns DomainDto, not a list.
            // Until the interface is corrected, we keep a simple response.
            var result = await _domainService.GetAllAsync(userId);

            _logger.LogInformation("Domains retrieved successfully for user.");
            return Ok(result);
        }

    /// <summary>
    /// Get a domain by id.
    /// </summary>
    /// <param name="id">The ID of the domain to retrieve.</param>
    /// <returns>The domain object.</returns>
    /// <response code="200">Domain retrieved successfully.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="404">Domain not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(DomainDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<ActionResult> GetDomainById(int id)
    {
        var userId = GetUserIdOrThrow();
        try
        {
            var domain = await _domainService.GetByIdAsync(id, userId);
            return Ok(domain);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new Error("Domain not found."));
        }
    }

    /// <summary>
    /// Delete a domain by id.
    /// </summary>
    /// <param name="id">The ID of the domain to delete.</param>
    /// <returns>204 No Content if successful, 404 Not Found if the domain does not exist.</returns>
    /// <response code="204">Domain deleted successfully.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="404">Domain not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error),StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> DeleteDomain(int id)
    {
        _logger.LogTrace("Delete domain attempt: {DomainId}", id);
        if (id <= 0)
        {
            _logger.LogWarning("Invalid domain ID for deletion: {DomainId}", id);
            return BadRequest(new Error("Invalid domain ID."));
        }
        var userId = GetUserIdOrThrow();
        var ok = await _domainService.DeleteAsync(id, userId);
        if (!ok)
            return NotFound(new Error("Domain not found."));

        _logger.LogInformation("Domain deleted successfully: {DomainId}", id);
        return NoContent();
    }

    /// <summary>
    /// Modify a domain by id.
    /// </summary>
    /// <param name="id">The ID of the domain to modify.</param>
    /// <param name="domainUpdateDto">The updated domain details.</param>
    /// <returns>204 No Content if successful, 404 Not Found if the domain does not exist.</returns>
    /// <response code="204">Domain modified successfully.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="404">Domain not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error),StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<IActionResult> UpdateDomain(int id, [FromBody] UpdateDomainDto domainUpdateDto)
    {
        _logger.LogTrace("Update domain attempt: {DomainId}", id);
        if (id <= 0)
        {
            _logger.LogWarning("Invalid domain ID for update: {DomainId}", id);
            return BadRequest(new Error("Invalid domain ID."));
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid domain update request: {DomainId}", id);
            return BadRequest(ModelState);
        }

        var userId = GetUserIdOrThrow();
        // For now, update by name (service contract). If the name is not provided, it will fail.
        var ok = await _domainService.UpdateAsync(domainUpdateDto, userId);
        if (!ok)
            return NotFound(new Error("Domain not found."));

        _logger.LogInformation("Domain updated successfully: {DomainId}", id);
        return NoContent();
    }

    /// <summary>
    /// Extracts the user ID from the JWT token claims.
    /// </summary>
    /// <returns>The authenticated user's ID.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the user ID in the token is invalid or missing.</exception>
    private int GetUserIdOrThrow()
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(id, out var userId))
            throw new InvalidOperationException("Invalid user id in token.");
        return userId;
    }
}
