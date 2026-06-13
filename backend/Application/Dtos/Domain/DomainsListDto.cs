namespace Application.Dtos.Domain;

/// <summary>
/// Response DTO for domain list items.
/// Simplified domain information for list views.
/// </summary>
public class DomainsListDto
{
    /// <summary>
    /// The domain name.
    /// </summary>
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
}