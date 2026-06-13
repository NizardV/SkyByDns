namespace Application.Dtos.CallDNS;

/// <summary>
/// Response DTO from external DNS lookup API.
/// Wraps DNS record data returned from the API.
/// </summary>
public class DnsResponse
{
    /// <summary>
    /// DNS record data containing all record types.
    /// </summary>
    public DnsData Data { get; set; }
}