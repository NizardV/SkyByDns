using Domain.Entities;

namespace Domain.Repositories
{
    /// <summary>
    /// Repository interface for Record entity operations.
    /// Handles data access for DNS record management.
    /// </summary>
    public interface IRecordRepository
    {
        /// <summary>
        /// Retrieves all DNS records for a specific domain.
        /// </summary>
        /// <param name="domainId">The ID of the domain.</param>
        /// <returns>A list of all records associated with the domain.</returns>
        Task<List<Record>> GetRecordsByDomainIdAsync(int domainId);

        /// <summary>
        /// Retrieves a specific DNS record by its unique identifier.
        /// </summary>
        /// <param name="id">The record ID.</param>
        /// <returns>The record if found, null otherwise.</returns>
        Task<Record?> GetRecordByIdAsync(int id);

        /// <summary>
        /// Creates a new DNS record in the database.
        /// </summary>
        /// <param name="recordCreateDto">The record entity to create.</param>
        /// <returns>The created record with generated ID.</returns>
        Task<Record> CreateRecordAsync(Record recordCreateDto);

        /// <summary>
        /// Updates an existing DNS record.
        /// </summary>
        /// <param name="id">The ID of the record to update.</param>
        /// <param name="recordUpdateDto">The record entity with updated values.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        Task<bool> UpdateRecordAsync(int id, Record recordUpdateDto);

        /// <summary>
        /// Deletes a DNS record from the database.
        /// </summary>
        /// <param name="id">The ID of the record to delete.</param>
        /// <returns>True if the deletion was successful, false otherwise.</returns>
        Task<bool> DeleteRecordAsync(int id);
    }
}
