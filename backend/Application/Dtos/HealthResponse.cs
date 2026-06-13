namespace Application.Dtos
{
    /// <summary>
    /// Response DTO for health check endpoint.
    /// Provides information about the API's operational status and performance.
    /// </summary>
    public class HealthResponse
    {
        /// <summary>
        /// Current health status of the API.
        /// </summary>
        /// <example>Healthy</example>
        public string Status { get; set; }

        /// <summary>
        /// API version number.
        /// </summary>
        /// <example>1.0.0</example>
        public string Version { get; set; }

        /// <summary>
        /// Current server date and time when the health check was performed.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Time taken to generate this health response.
        /// Useful for monitoring API performance.
        /// </summary>
        public TimeSpan TimeResponse { get; set; }
    }
}
