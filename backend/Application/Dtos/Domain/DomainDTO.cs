namespace Application.Dtos.Domain;

/// <summary>
/// Response DTO for domain details with full information.
/// Includes domain metadata and associated record count.
/// </summary>
public class DomainDto
{
    /// <summary>
    /// Domain's unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The domain name.
    /// </summary>
    /// <example>example.com</example>
    public string Name { get; set; }

    /// <summary>
    /// Indicates whether the domain is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Timestamp when the domain was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Timestamp when the domain was last updated.
    /// Null if never updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Number of DNS records associated with this domain.
    /// </summary>
    public int RecordCount { get; set; }
}