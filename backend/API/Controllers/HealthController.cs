using System.Reflection;
using Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller for API health monitoring.
    /// Provides endpoints to check the health status and version information of the API.
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;

        public HealthController(ILogger<HealthController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Check the health status of the API.
        /// </summary>
        /// <returns>Returns a string indicating the health status of the API.</returns>
        /// <response code="200">API is healthy.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("health")]
        [ProducesResponseType(typeof(HealthResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<HealthResponse> GetHealth()
        {
            DateTime startTime = DateTime.UtcNow;
            _logger.LogInformation("Health check requested.");

            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
            var assemblyVersion = assembly.GetName().Version?.ToString() ?? "Unknown";
            var response = new HealthResponse
            {
                Status = "OK",
                Version = assemblyVersion,
                DateTime = DateTime.UtcNow,
                TimeResponse = DateTime.UtcNow - startTime,
            };
            return Ok(response);
        }
    }
}
