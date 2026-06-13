using Microsoft.EntityFrameworkCore;
using Domain.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

/// <summary>
/// Entity Framework Core implementation of the Domain repository.
/// Provides data access for DNS domain management with eager loading of related records.
/// </summary>
public class EfDomainRepository : IDomainRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the EfDomainRepository with the database context.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public EfDomainRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Adds a new domain to the database.
    /// </summary>
    /// <param name="domain">The domain entity to add.</param>
    /// <returns>The created domain with generated ID.</returns>
    public async Task<DomainEntities?> AddAsync(DomainEntities domain)
    {
        await _context.AddAsync(domain);
        await _context.SaveChangesAsync();
        return domain;
    }

    /// <summary>
    /// Deletes a domain from the database.
    /// Cascade deletes all associated DNS records.
    /// </summary>
    /// <param name="id">The ID of the domain to delete.</param>
    /// <returns>True if deletion was successful, false if domain not found.</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Domains.FirstOrDefaultAsync(d => d.Id == id);

        if (entity == null)
        {
            return false;
        }

        _context.Domains.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Retrieves all domains with their associated DNS records.
    /// Uses AsNoTracking for read-only queries to improve performance.
    /// </summary>
    /// <returns>A list of all domains with records.</returns>
    public async Task<List<DomainEntities>> GetAllAsync()
    {
        return await _context.Domains
            .Include(d => d.Records)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves a domain by its ID with associated records.
    /// Uses tracking for potential updates.
    /// </summary>
    /// <param name="id">The domain ID.</param>
    /// <returns>The domain if found, null otherwise.</returns>
    public async Task<DomainEntities?> GetByIdAsync(int id)
    {
        return await _context.Domains
            .Include(d => d.Records)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    /// <summary>
    /// Retrieves a domain by its name with associated records.
    /// Uses tracking for potential updates.
    /// </summary>
    /// <param name="name">The domain name to search for.</param>
    /// <returns>The domain if found, null otherwise.</returns>
    public async Task<DomainEntities?> GetByNameAsync(string name)
    {
        return await _context.Domains
            .Include(d => d.Records)
            .FirstOrDefaultAsync(d => d.Name == name);
    }

    /// <summary>
    /// Updates an existing domain in the database.
    /// </summary>
    /// <param name="domain">The domain entity with updated values.</param>
    /// <returns>True if update was successful (changes made), false otherwise.</returns>
    public async Task<bool> UpdateAsync(DomainEntities domain)
    {
        _context.Domains.Update(domain);
        var changes = await _context.SaveChangesAsync();
        return changes > 0;
    }
}
