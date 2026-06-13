namespace Application.Dtos.Record;

/// <summary>
/// Response DTO for record list items.
/// Simplified record information for list views.
/// </summary>
public class RecordListDto
{
    /// <summary>
    /// The record name.
    /// </summary>
    public string RecordName { get; set; }

    /// <summary>
    /// The target value.
    /// </summary>
    public string Target { get; set; }

    /// <summary>
    /// Priority (for MX/SRV records).
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
}