namespace Domain.Entities
{
    /// <summary>
    /// Represents a DNS record for a domain.
    /// Supports various record types including A, AAAA, CNAME, MX, TXT, NS, SOA, SRV, CAA, and PTR.
    /// </summary>
    public class Record
    {
        /// <summary>
        /// Unique identifier for the DNS record.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to the domain this record belongs to.
        /// </summary>
        public int DomainId { get; set; }

        /// <summary>
        /// The record name (subdomain or @ for root).
        /// Example: "www" for www.example.com or "@" for example.com
        /// </summary>
        public string RecordName { get; set; }

        /// <summary>
        /// The target value for the DNS record.
        /// Content depends on RecordType:
        /// - A/AAAA: IP address (e.g., "192.0.2.1" or "2001:db8::1")
        /// - CNAME: Target hostname (e.g., "example.com")
        /// - MX: Mail server hostname
        /// - TXT: Text value for verification or SPF records
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// Priority for MX and SRV records.
        /// Lower values indicate higher priority.
        /// Null for record types that don't use priority.
        /// </summary>
        public int? Priority { get; set; }

        /// <summary>
        /// Time To Live in seconds.
        /// Determines how long DNS resolvers should cache this record.
        /// Common values: 300 (5 min), 3600 (1 hour), 86400 (1 day)
        /// </summary>
        public int TTL { get; set; }

        /// <summary>
        /// Type of DNS record.
        /// Supported types: A (IPv4), AAAA (IPv6), CNAME (alias), MX (mail), 
        /// TXT (text), NS (nameserver), SOA (start of authority), 
        /// SRV (service), CAA (certificate authority), PTR (reverse DNS)
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
        /// Required foreign key relationship with cascade delete.
        /// </summary>
        public virtual DomainEntities DomainEntities { get; set; }
    }
}