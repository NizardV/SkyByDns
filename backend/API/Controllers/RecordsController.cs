using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.Dtos;
using Application.Dtos.Record;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

/// <summary>
/// Controller for managing DNS records.
/// Handles CRUD operations for DNS records associated with domains.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[Authorize]
public class RecordsController : ControllerBase
{
    private readonly IRecordService _recordService;
    private readonly ILogger<RecordsController> _logger;

    public RecordsController(IRecordService recordService, ILogger<RecordsController> logger)
    {
        _recordService = recordService;
        _logger = logger;
    }

    /// <summary>
    /// Get all records for a domain.
    /// </summary>
    /// <param name="domainId">The ID of the domain.</param>
    /// <returns>List of records.</returns>
    /// <response code="200">Records retrieved successfully.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="404">Domain not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<RecordListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error),StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRecords([FromQuery] int domainId)
    {
        try
        {
            _logger.LogTrace("Get records attempt for domain ID: {DomainId}", domainId);
            var records = await _recordService.GetRecordsByDomainId(domainId);
            if (records == null) 
            {
                return NotFound();
            }
            return Ok(records);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving records for domain ID {DomainId}", domainId);
            return StatusCode(StatusCodes.Status500InternalServerError, new Error("An error occurred while processing your request." ));
        }
    }

    /// <summary>
    /// Get record by ID.
    /// </summary>
    /// <returns>Record details.</returns>
    /// <param name="id">The ID of the record.</param>
    /// <response code="200">Record retrieved successfully.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="404">Record not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RecordListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error),StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRecordById(int id)
    {
        try
        {
            _logger.LogTrace("Get record attempt for record ID: {RecordId}", id);
            var record = await _recordService.GetRecordById(id);
            if (record == null)
            {
                return NotFound(new Error ("Record not found." ));
            }
            return Ok(record);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving record with ID {RecordId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new Error("An error occurred while processing your request."));
        }
    }

    /// <summary>
    /// Create a new record.
    /// </summary>
    /// <param name="recordCreateDto">The record creation details.</param>
    /// <returns>Created record details.</returns>
    /// <response code="201">Record created successfully.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost]
    [ProducesResponseType(typeof(RecordListDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Error),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateRecord([FromBody] CreateRecordDto recordCreateDto)
    {
        try
        {
            _logger.LogTrace("Create record attempt for domain ID: {CreateRecordDto.DomainId}", recordCreateDto.DomainId);
            var createdRecord = await _recordService.CreateRecord(recordCreateDto);
            return CreatedAtAction(nameof(GetRecordById), new { id = createdRecord.Id }, createdRecord);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating record for domain ID {CreateRecordDto.DomainId}", recordCreateDto.DomainId);
            return StatusCode(StatusCodes.Status500InternalServerError, new Error("An error occurred while processing your request."));
        }
    }

    /// <summary>
    /// Modify an existing record.
    /// </summary>
    /// <param name="id">The ID of the record to update.</param>
    /// <param name="recordUpdateDto">The updated record details.</param>
    /// <returns>Updated record details.</returns>
    /// <response code="200">Record updated successfully.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="404">Record not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(RecordListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error),StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateRecord(int id, [FromBody] UpdateRecordDto recordUpdateDto)
    {
        try
        {
            _logger.LogTrace("Update record attempt for record ID: {RecordId}", id);
            var updatedRecord = await _recordService.UpdateRecord(id, recordUpdateDto);
            if (!updatedRecord)
            {
                return NotFound(new Error ("Record not found." ));
            }
            return Ok(updatedRecord);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating record with ID {RecordId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new Error("An error occurred while processing your request."));
        }
    }

    /// <summary>
    /// Delete a record.
    /// </summary>
    /// <param name="id">The ID of the record to delete.</param>
    /// <returns>204 No Content if successful, 404 Not Found if the record does not exist.</returns>
    /// <response code="204">Record deleted successfully.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="404">Record not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error),StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Error),StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteRecord(int id)
    {
        try
        {
            _logger.LogTrace("Delete record attempt for record ID: {RecordId}", id);
            var result = await _recordService.DeleteRecord(id);
            if (!result)
            {
                return NotFound(new Error ("Record not found." ));
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting record with ID {RecordId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new Error("An error occurred while processing your request."));
        }
    }
}