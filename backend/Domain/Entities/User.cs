namespace Domain.Entities
{
    /// <summary>
    /// Represents a user account in the system.
    /// Users can authenticate and manage DNS domains.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique identifier for the user.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User's last name (surname/family name).
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// User's first name (given name).
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// User's email address. Used for authentication and must be unique.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Hashed password using Identity's password hasher.
        /// Never store passwords in plain text.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// User's role in the system (e.g., "Admin", "User").
        /// Used for authorization and access control.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Timestamp when the user account was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Timestamp when the user account was last updated.
        /// Null if never updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Indicates whether the user account is active.
        /// Inactive accounts cannot log in.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Collection of DNS domains owned by this user.
        /// One-to-many relationship with delete restricted.
        /// </summary>
        public virtual ICollection<DomainEntities> Domains { get; set; }
    }
}