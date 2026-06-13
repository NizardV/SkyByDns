namespace Application.Dtos.CallDNS;

/// <summary>
/// DTO representing DNS record data returned from external DNS API.
/// Contains arrays of different DNS record types.
/// </summary>
public class DnsData
{
    /// <summary>
    /// A records (IPv4 addresses).
    /// </summary>
    public List<object> A { get; set; }

    /// <summary>
    /// AAAA records (IPv6 addresses).
    /// </summary>
    public List<object> AAAA { get; set; }

    /// <summary>
    /// CAA records (Certificate Authority Authorization).
    /// </summary>
    public List<object> CAA { get; set; }

    /// <summary>
    /// CNAME records (Canonical Name aliases).
    /// </summary>
    public List<object> CNAME { get; set; }

    /// <summary>
    /// MX records (Mail Exchange servers).
    /// </summary>
    public List<object> MX { get; set; }

    /// <summary>
    /// NS records (Name Servers).
    /// </summary>
    public List<object> NS { get; set; }

    /// <summary>
    /// SOA records (Start of Authority).
    /// </summary>
    public List<object> SOA { get; set; }

    /// <summary>
    /// SRV records (Service locator).
    /// </summary>
    public List<object> SRV { get; set; }

    /// <summary>
    /// TXT records (Text records for verification, SPF, DKIM, etc.).
    /// </summary>
    public List<object> TXT { get; set; }
}