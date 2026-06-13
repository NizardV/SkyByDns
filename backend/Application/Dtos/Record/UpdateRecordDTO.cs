using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Record;

/// <summary>
/// Request DTO for updating an existing DNS record.
/// All fields except DomainId can be updated.
/// </summary>
public class UpdateRecordDto
{
    /// <summary>
    /// The new record name.
    /// Maximum length: 60 characters.
    /// </summary>
    [Required]
    [MaxLength(60)]
    public string RecordName { get; set; }

    /// <summary>
    /// The new target value.
    /// Maximum length: 100 characters.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Target { get; set; }

    /// <summary>
    /// The new priority (for MX/SRV records).
    /// Maximum value: 1.
    /// </summary>
    [MaxLength(1)]
    public int? Priority { get; set; }

    /// <summary>
    /// The new Time To Live in seconds.
    /// Defaults to 3600 seconds (1 hour).
    /// </summary>
    public int Ttl { get; set; } = 3600;

    /// <summary>
    /// The new record type.
    /// Maximum length: 10 characters.
    /// </summary>
    [Required]
    [MaxLength(10)]
    public string RecordType { get; set; }
}