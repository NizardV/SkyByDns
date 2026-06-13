using Application.Dtos.Domain;

namespace Application.Dtos.Record;

/// <summary>
/// Response DTO for DNS record details.
/// Contains full record information including associated domain.
/// </summary>
public class RecordDto
{
    /// <summary>
    /// Record's unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ID of the domain this record belongs to.
    /// </summary>
    public int DomainId { get; set; }

    /// <summary>
    /// The record name (subdomain or @ for root).
    /// </summary>
    public string RecordName { get; set; }

    /// <summary>
    /// The target value (IP address, hostname, text, etc.).
    /// </summary>
    public string Target { get; set; }

    /// <summary>
    /// Priority for MX and SRV records.
    /// Null for other record types.
    /// </summary>
    public int? Priority { get; set; }

    /// <summary>
    /// Time To Live in seconds.
    /// </summary>
    public int TTL { get; set; }

    /// <summary>
    /// Type of DNS record.
    /// </summary>
    public string RecordType { get; set; }

    /// <summary>
    /// Timestamp when the record was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Timestamp when the record was last updated.
    /// Null if never updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// The domain this record belongs to.
    /// </summary>
    public DomainDto Domain { get; set; }
}