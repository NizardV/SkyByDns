namespace Application.Dtos.Domain;

/// <summary>
/// Response DTO for domain availability checks.
/// Indicates whether a domain name is available for registration.
/// </summary>
public class DomainAvailabilityDto
{
    /// <summary>
    /// The domain name that was checked.
    /// </summary>
    /// <example>example.com</example>
    public string DomainName { get; set; }

    /// <summary>
    /// Indicates whether the domain is available for registration.
    /// True if available, false if already registered or in use.
    /// </summary>
    public bool IsAvailable { get; set; }

    /// <summary>
    /// Human-readable message describing the availability status.
    /// </summary>
    /// <example>Domain is available</example>
    public string Message { get; set; }

    /// <summary>
    /// Status code or category of the availability check.
    /// </summary>
    /// <example>available</example>
    public string Status { get; set; }
}