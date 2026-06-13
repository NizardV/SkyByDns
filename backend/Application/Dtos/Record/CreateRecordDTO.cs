using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Record;

/// <summary>
/// Request DTO for creating a new DNS record.
/// Contains all required information to define a DNS record.
/// </summary>
public class CreateRecordDto
{
    /// <summary>
    /// ID of the domain this record belongs to.
    /// </summary>
    public int DomainId { get; set; }

    /// <summary>
    /// The record name (subdomain or @ for root).
    /// Maximum length: 60 characters.
    /// </summary>
    /// <example>www</example>
    /// <example>@</example>
    [Required]
    [StringLength(60)]
    public string RecordName { get; set; }

    /// <summary>
    /// The target value for the DNS record.
    /// Content varies by record type (IP address, hostname, text, etc.).
    /// Maximum length: 100 characters.
    /// </summary>
    /// <example>192.0.2.1</example>
    /// <example>mail.example.com</example>
    [Required]
    [StringLength(100)]
    public string Target { get; set; }

    /// <summary>
    /// Priority for MX and SRV records (1-2 range).
    /// Lower values indicate higher priority. Null for other record types.
    /// </summary>
    [Range(1, 2)]
    public int? Priority { get; set; }

    /// <summary>
    /// Time To Live in seconds.
    /// Determines how long DNS resolvers cache this record.
    /// Defaults to 3600 seconds (1 hour).
    /// </summary>
    /// <example>3600</example>
    public int TTL { get; set; } = 3600;

    /// <summary>
    /// Type of DNS record (A, AAAA, CNAME, MX, TXT, NS, SOA, SRV, CAA, PTR).
    /// Maximum length: 10 characters.
    /// </summary>
    /// <example>A</example>
    /// <example>CNAME</example>
    [Required]
    [StringLength(10)]
    public string RecordType { get; set; }
}