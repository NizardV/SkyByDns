using Domain.Entities;

namespace Domain.Repositories;

/// <summary>
/// Repository interface for DomainEntities operations.
/// Handles data access for DNS domain management.
/// </summary>
public interface IDomainRepository
{
    /// <summary>
    /// Retrieves all domains from the database.
    /// </summary>
    /// <returns>A list of all domains.</returns>
    Task<List<Entities.DomainEntities>> GetAllAsync();

    /// <summary>
    /// Retrieves a domain by its name.
    /// Used for checking domain uniqueness and availability.
    /// </summary>
    /// <param name="name">The domain name to search for (e.g., "example.com").</param>
    /// <returns>The domain if found, null otherwise.</returns>
    Task<Entities.DomainEntities?> GetByNameAsync(string name);

    /// <summary>
    /// Retrieves a domain by its unique identifier.
    /// </summary>
    /// <param name="id">The domain ID.</param>
    /// <returns>The domain if found, null otherwise.</returns>
    Task<Entities.DomainEntities?> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new domain to the database.
    /// </summary>
    /// <param name="domainEntities">The domain entity to add.</param>
    /// <returns>The created domain with generated ID, or null if creation failed.</returns>
    Task<Entities.DomainEntities?> AddAsync(Entities.DomainEntities domainEntities);

    /// <summary>
    /// Updates an existing domain in the database.
    /// </summary>
    /// <param name="domainEntities">The domain entity with updated values.</param>
    /// <returns>True if the update was successful, false otherwise.</returns>
    Task<bool> UpdateAsync(Entities.DomainEntities domainEntities);

    /// <summary>
    /// Deletes a domain from the database.
    /// Cascade deletes all associated DNS records.
    /// </summary>
    /// <param name="id">The ID of the domain to delete.</param>
    /// <returns>True if the deletion was successful, false otherwise.</returns>
    Task<bool> DeleteAsync(int id);
}
