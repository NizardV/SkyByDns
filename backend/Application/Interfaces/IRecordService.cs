using Application.Dtos.Record;

namespace Application.Interfaces;

/// <summary>
/// Service interface for DNS record management operations.
/// Handles creation, retrieval, updates, and deletion of DNS records.
/// </summary>
public interface IRecordService
{
    /// <summary>
    /// Retrieves all DNS records for a specific domain.
    /// </summary>
    /// <param name="domainId">ID of the domain whose records to retrieve.</param>
    /// <returns>List of all DNS records associated with the domain.</returns>
    Task<List<RecordListDto>> GetRecordsByDomainId(int domainId);

    /// <summary>
    /// Retrieves a specific DNS record by its ID.
    /// </summary>
    /// <param name="id">ID of the record to retrieve.</param>
    /// <returns>Record details if found, null otherwise.</returns>
    Task<RecordDto?> GetRecordById(int id);

    /// <summary>
    /// Creates a new DNS record.
    /// </summary>
    /// <param name="recordCreateDto">Record creation data including type, name, target, TTL, etc.</param>
    /// <returns>The created record with full details.</returns>
    Task<RecordDto> CreateRecord(CreateRecordDto recordCreateDto);

    /// <summary>
    /// Updates an existing DNS record.
    /// </summary>
    /// <param name="id">ID of the record to update.</param>
    /// <param name="recordUpdateDto">Updated record data.</param>
    /// <returns>True if update was successful, false otherwise.</returns>
    Task<bool> UpdateRecord(int id, UpdateRecordDto recordUpdateDto);

    /// <summary>
    /// Deletes a DNS record.
    /// </summary>
    /// <param name="id">ID of the record to delete.</param>
    /// <returns>True if deletion was successful, false otherwise.</returns>
    Task<bool> DeleteRecord(int id);
}