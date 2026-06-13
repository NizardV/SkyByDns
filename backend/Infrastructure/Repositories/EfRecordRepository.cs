using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Entity Framework Core implementation of the Record repository.
    /// Provides data access for DNS record management.
    /// </summary>
    public class EfRecordRepository : IRecordRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the EfRecordRepository with the database context.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public EfRecordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all DNS records for a specific domain.
        /// Returns records ordered by ID.
        /// </summary>
        /// <param name="domainId">ID of the domain whose records to retrieve.</param>
        /// <returns>A list of all DNS records for the domain.</returns>
        public async Task<List<Record>> GetRecordsByDomainIdAsync(int domainId)
        {
            return await _context.Records
                .AsNoTracking()
                .OrderBy(r => r.Id)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific DNS record by its ID.
        /// Uses AsNoTracking for read-only queries.
        /// </summary>
        /// <param name="id">The record ID.</param>
        /// <returns>The record if found, null otherwise.</returns>
        public async Task<Record?> GetRecordByIdAsync(int id)
        {
            return await _context.Records
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <summary>
        /// Creates a new DNS record in the database.
        /// </summary>
        /// <param name="recordCreateDto">The record entity to create.</param>
        /// <returns>The created record with generated ID.</returns>
        public async Task<Record> CreateRecordAsync(Record recordCreateDto)
        {
            _context.Records.Add(recordCreateDto);
            await _context.SaveChangesAsync();
            return recordCreateDto;
        }

        /// <summary>
        /// Updates an existing DNS record.
        /// </summary>
        /// <param name="id">The ID of the record to update.</param>
        /// <param name="recordUpdateDto">The record entity with updated values.</param>
        /// <returns>True if update was successful, false if record not found.</returns>
        public async Task<bool> UpdateRecordAsync(int id, Record recordUpdateDto)
        {
            var exists = await _context.Records.AnyAsync(r => r.Id == id);
            if (!exists)
            {
                return false;
            }

            _context.Records.Update(recordUpdateDto);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Deletes a DNS record from the database.
        /// </summary>
        /// <param name="id">The ID of the record to delete.</param>
        /// <returns>True if deletion was successful, false if record not found.</returns>
        public async Task<bool> DeleteRecordAsync(int id)
        {
            var entity = await _context.Records.FirstOrDefaultAsync(r => r.Id == id);
            if (entity == null)
            {
                return false;
            }

            _context.Records.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
