using Application.Dtos.Domain;

namespace Application.Interfaces;

/// <summary>
/// Service interface for DNS domain management operations.
/// Handles domain creation, updates, deletion, and availability checking.
/// </summary>
public interface IDomainService
{
    /// <summary>
    /// Creates a new domain for the specified user.
    /// </summary>
    /// <param name="domainDto">Domain creation data containing the domain name.</param>
    /// <param name="userId">ID of the user creating the domain.</param>
    /// <returns>The created domain with full details.</returns>
    Task<DomainDto> CreateAsync(CreateDomainDto domainDto, int userId);

    /// <summary>
    /// Updates an existing domain.
    /// Verifies user ownership before updating.
    /// </summary>
    /// <param name="domainDto">Updated domain data.</param>
    /// <param name="userId">ID of the user requesting the update.</param>
    /// <returns>True if update was successful, false otherwise.</returns>
    Task<bool> UpdateAsync(UpdateDomainDto domainDto, int userId);

    /// <summary>
    /// Checks if a domain name is available for registration.
    /// Queries local database and external DNS API.
    /// </summary>
    /// <param name="domainName">The domain name to check (e.g., "example.com").</param>
    /// <returns>Domain availability information including status and message.</returns>
    Task<DomainAvailabilityDto> CheckAvailabilityAsync(string domainName);

    /// <summary>
    /// Deletes a domain and all its associated DNS records.
    /// Verifies user ownership before deletion.
    /// </summary>
    /// <param name="domainId">ID of the domain to delete.</param>
    /// <param name="userId">ID of the user requesting the deletion.</param>
    /// <returns>True if deletion was successful, false otherwise.</returns>
    Task<bool> DeleteAsync(int domainId, int userId);

    /// <summary>
    /// Retrieves a specific domain by ID.
    /// Verifies user ownership before returning.
    /// </summary>
    /// <param name="domainId">ID of the domain to retrieve.</param>
    /// <param name="userId">ID of the user requesting the domain.</param>
    /// <returns>Domain details if found and authorized, null otherwise.</returns>
    Task<DomainDto> GetByIdAsync(int domainId, int userId);

    /// <summary>
    /// Retrieves all domains belonging to a specific user.
    /// </summary>
    /// <param name="userId">ID of the user whose domains to retrieve.</param>
    /// <returns>List of all user's domains.</returns>
    Task<List<DomainDto>> GetAllAsync(int userId);

    /// <summary>
    /// Retrieves a domain by its name.
    /// </summary>
    /// <param name="name">The domain name to search for.</param>
    /// <returns>Domain details if found, null otherwise.</returns>
    Task<DomainDto> GetByNameAsync(string name);
}