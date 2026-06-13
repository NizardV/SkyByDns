namespace Domain.Entities
{
    /// <summary>
    /// Represents a DNS server that can be used for domain resolution and availability checks.
    /// Used by the system to query DNS records and verify domain configurations.
    /// </summary>
    public class DNSServer
    {
        /// <summary>
        /// Unique identifier for the DNS server.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// IP address of the DNS server (IPv4 or IPv6).
        /// Example: "8.8.8.8" (Google DNS) or "1.1.1.1" (Cloudflare DNS)
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// URL or hostname of the DNS server.
        /// Can be used for DNS-over-HTTPS (DoH) or DNS-over-TLS (DoT) endpoints.
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// Indicates whether this DNS server is currently active and should be used for queries.
        /// Inactive servers are skipped during DNS resolution.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Timestamp when this DNS server was added to the system.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}