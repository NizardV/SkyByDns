using Application.Dtos.Auth;
using Application.Dtos.Domain;
using Application.Dtos.Record;
using Application.Interfaces;
using Domain.Repositories;
using Domain.Entities;

namespace Application.Services;

/// <summary>
/// Service implementation for DNS record management operations.
/// Handles creation, retrieval, updates, and deletion of DNS records.
/// </summary>
public class RecordService : IRecordService
{
    private readonly IRecordRepository _recordRepository;

    /// <summary>
    /// Initializes a new instance of the RecordService with required dependencies.
    /// </summary>
    /// <param name="recordRepository">Repository for record data access.</param>
    public RecordService(IRecordRepository recordRepository)
    {
        _recordRepository = recordRepository;
    }

    /// <summary>
    /// Retrieves all DNS records for a specific domain.
    /// </summary>
    /// <param name="domainId">ID of the domain whose records to retrieve.</param>
    /// <returns>List of simplified record information for the domain.</returns>
    public async Task<List<RecordListDto>> GetRecordsByDomainId(int domainId)
    {
        var records = await _recordRepository.GetRecordsByDomainIdAsync(domainId);
        return records.Select(r => new RecordListDto
        {
            RecordName = r.RecordName,
            Target = r.Target,
            Priority = r.Priority,
            TTL = r.TTL,
            RecordType = r.RecordType
        }).ToList();
    }

    /// <summary>
    /// Retrieves a specific DNS record by its ID.
    /// </summary>
    /// <param name="id">ID of the record to retrieve.</param>
    /// <returns>Record details if found, null otherwise.</returns>
    public async Task<RecordDto?> GetRecordById(int id)
    {
        var records = await _recordRepository.GetRecordByIdAsync(id);
        return records == null ? null : MapToDto(records);
    }

    /// <summary>
    /// Creates a new DNS record.
    /// </summary>
    /// <param name="recordCreateDto">Record creation data including type, name, target, TTL, etc.</param>
    /// <returns>The created record with full details.</returns>
    public async Task<RecordDto> CreateRecord(CreateRecordDto recordCreateDto)
    {
        // Map DTO to entity
        var record = new Record
        {
            DomainId = recordCreateDto.DomainId,
            RecordName = recordCreateDto.RecordName,
            Target = recordCreateDto.Target,
            Priority = recordCreateDto.Priority,
            TTL = recordCreateDto.TTL,
            RecordType = recordCreateDto.RecordType
        };

        var created = await _recordRepository.CreateRecordAsync(record);
        return MapToDto(created);
    }

    /// <summary>
    /// Updates an existing DNS record.
    /// </summary>
    /// <param name="id">ID of the record to update.</param>
    /// <param name="recordUpdateDto">Updated record data.</param>
    /// <returns>True if update was successful, false if record not found.</returns>
    public async Task<bool> UpdateRecord(int id, UpdateRecordDto recordUpdateDto)
    {
        var existing = await _recordRepository.GetRecordByIdAsync(id);
        if (existing == null)
        {
            return false;
        }

        // Update record properties
        existing.RecordName = recordUpdateDto.RecordName;
        existing.Target = recordUpdateDto.Target;
        existing.Priority = recordUpdateDto.Priority;
        existing.TTL = recordUpdateDto.Ttl;
        existing.RecordName = recordUpdateDto.RecordName;

        return await _recordRepository.UpdateRecordAsync(id, existing);
    }

    /// <summary>
    /// Deletes a DNS record.
    /// </summary>
    /// <param name="id">ID of the record to delete.</param>
    /// <returns>True if deletion was successful, false otherwise.</returns>
    public async Task<bool> DeleteRecord(int id)
    {
        return await _recordRepository.DeleteRecordAsync(id);
    }

    /// <summary>
    /// Converts a Record entity to a RecordDto.
    /// </summary>
    /// <param name="record">The record entity to convert.</param>
    /// <returns>Record DTO with all details.</returns>
    private static RecordDto MapToDto(Record record)
    {
        return new RecordDto
        {
            Id = record.Id,
            DomainId = record.DomainId,
            RecordName = record.RecordName,
            Target = record.Target,
            Priority = record.Priority,
            TTL = record.TTL,
            RecordType = record.RecordType,
            UpdatedAt = record.UpdatedAt
        };
    }
}