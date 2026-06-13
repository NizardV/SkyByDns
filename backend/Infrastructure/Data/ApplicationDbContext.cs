using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

/// <summary>
/// Database context for the DNS management application.
/// Configures entity relationships, constraints, and database mappings using Entity Framework Core.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the ApplicationDbContext with the specified options.
    /// </summary>
    /// <param name="options">The options for this context.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the Users table.
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// Gets or sets the Domains table.
    /// </summary>
    public DbSet<Domain.Entities.DomainEntities> Domains => Set<Domain.Entities.DomainEntities>();

    /// <summary>
    /// Gets or sets the Records table for DNS records.
    /// </summary>
    public DbSet<Record> Records => Set<Record>();

    /// <summary>
    /// Gets or sets the DNSServers table.
    /// </summary>
    public DbSet<DNSServer> DNSServers => Set<DNSServer>();

    /// <summary>
    /// Configures the database schema using Fluent API.
    /// Defines entity relationships, constraints, indexes, and default values.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // Unique constraint on email for authentication
            entity.HasIndex(e => e.Email).IsUnique();
            
            // Field constraints
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        // Domain configuration
        modelBuilder.Entity<Domain.Entities.DomainEntities>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // Unique constraint on domain name
            entity.HasIndex(e => e.Name).IsUnique();
            
            // Field constraints
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            // Relationship: Domain belongs to User (1-to-many)
            // DeleteBehavior.Restrict prevents deleting a user with domains
            entity.HasOne(d => d.User)
                .WithMany(u => u.Domains)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Record configuration
        modelBuilder.Entity<Record>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // Field constraints
            entity.Property(e => e.RecordName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Target).IsRequired().HasMaxLength(255);
            entity.Property(e => e.RecordType).IsRequired().HasMaxLength(10);
            entity.Property(e => e.TTL).HasDefaultValue(3600);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");

            // PostgreSQL identity column for auto-increment
            entity.Property(e => e.Id).UseIdentityColumn();

            // Relationship: Record belongs to Domain (1-to-many)
            // DeleteBehavior.Cascade automatically deletes records when domain is deleted
            entity.HasOne(r => r.DomainEntities)
                .WithMany(d => d.Records)
                .HasForeignKey(r => r.DomainId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // DNSServer configuration
        modelBuilder.Entity<DNSServer>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // Unique constraints on IP and URL
            entity.HasIndex(e => e.IPAddress).IsUnique();
            entity.HasIndex(e => e.URL).IsUnique();
            
            // Field constraints
            entity.Property(e => e.IPAddress).IsRequired().HasMaxLength(45); // Max length for IPv6
            entity.Property(e => e.URL).IsRequired().HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
        });
    }
}