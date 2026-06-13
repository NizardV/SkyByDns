namespace Domain.Entities
{
    /// <summary>
    /// Represents a DNS domain managed by a user.
    /// Domains can contain multiple DNS records (A, AAAA, CNAME, MX, TXT, etc.).
    /// </summary>
    public class DomainEntities
    {
        /// <summary>
        /// Unique identifier for the domain.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foreign key to the User who owns this domain.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The domain name (e.g., "example.com").
        /// Must be unique across the system.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Timestamp when the domain was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Timestamp when the domain was last updated.
        /// Null if never updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Indicates whether the domain is active.
        /// Inactive domains may not be resolved or managed.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The user who owns this domain.
        /// Required foreign key relationship.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Collection of DNS records associated with this domain.
        /// One-to-many relationship with cascade delete enabled.
        /// </summary>
        public virtual ICollection<Record> Records { get; set; }
    }
}