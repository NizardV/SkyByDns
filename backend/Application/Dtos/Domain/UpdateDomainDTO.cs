namespace Application.Dtos.Domain;

/// <summary>
/// Request DTO for updating an existing domain.
/// Only specified fields will be updated.
/// </summary>
public class UpdateDomainDto
{
    /// <summary>
    /// The new domain name (if changing).
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Whether to activate or deactivate the domain.
    /// Null to keep current status unchanged.
    /// </summary>
    public bool? IsActive { get; set; }
}