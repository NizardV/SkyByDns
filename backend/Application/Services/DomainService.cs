using System.Text.Json;
using Application.Dtos.CallDNS;
using Application.Dtos.Domain;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Services;

/// <summary>
/// Service implementation for DNS domain management operations.
/// Handles domain creation, updates, deletion, and availability checking.
/// </summary>
public class DomainService : IDomainService
{
    private readonly ILogger<DomainService> _logger;
    private readonly IDomainRepository _domainRepository;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// JSON serialization options for DNS API responses.
    /// Configured for case-insensitive property matching.
    /// </summary>
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Initializes a new instance of the DomainService with required dependencies.
    /// </summary>
    /// <param name="logger">Logger for domain operations.</param>
    /// <param name="domainRepository">Repository for domain data access.</param>
    /// <param name="httpClientFactory">Factory for creating HTTP clients for DNS API calls.</param>
    public DomainService(
        ILogger<DomainService> logger,
        IDomainRepository domainRepository,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _domainRepository = domainRepository;
        _httpClient = httpClientFactory.CreateClient();
    }

    /// <summary>
    /// Checks if a domain name is available for registration.
    /// Queries local database and external DNS API.
    /// </summary>
    /// <param name="domainName">The domain name to check.</param>
    /// <returns>Domain availability information including status and message.</returns>
    public Task<DomainAvailabilityDto> CheckAvailabilityAsync(string domainName)
    {
        return CheckDomainAvailabilityAsync(domainName);
    }

    /// <summary>
    /// Internal implementation for checking domain availability.
    /// Performs a two-step check: local database then external DNS API.
    /// </summary>
    /// <param name="domainName">The domain name to check.</param>
    /// <returns>Domain availability details with status and explanation.</returns>
    private async Task<DomainAvailabilityDto> CheckDomainAvailabilityAsync(string domainName)
    {
        _logger.LogInformation("Checking availability for domain: {DomainName}", domainName);

        // Validate input
        if (string.IsNullOrWhiteSpace(domainName))
        {
            return new DomainAvailabilityDto
            {
                DomainName = domainName,
                IsAvailable = false,
                Message = "Domain name is required.",
                Status = "unavailable"
            };
        }

        var normalized = domainName.Trim().ToLowerInvariant();

        // Step 1: Check if domain exists in local database
        var existing = await _domainRepository.GetByNameAsync(normalized);
        if (existing != null)
        {
            return new DomainAvailabilityDto
            {
                DomainName = normalized,
                IsAvailable = false,
                Message = "Domain already exists.",
                Status = "unavailable"
            };
        }

        // Step 2: Check external DNS records to verify domain is truly available
        // A domain with no DNS records anywhere is likely available
        try
        {
            var isAvailableOnline = await IsDomainAvailaibleOnline(normalized);
            return new DomainAvailabilityDto
            {
                DomainName = normalized,
                IsAvailable = isAvailableOnline,
                Message = isAvailableOnline
                    ? "No DNS records found. This is NOT a guarantee of registrar availability."
                    : "DNS records found. Domain seems already used.",
                Status = isAvailableOnline ? "available" : "unavailable"
            };
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to check DNS records for domain: {DomainName}", normalized);
            return new DomainAvailabilityDto
            {
                DomainName = normalized,
                IsAvailable = false,
                Message = "Unable to check DNS records.",
                Status = "unknown"
            };
        }
    }

    /// <summary>
    /// Checks if a domain is available by querying external DNS API.
    /// A domain is considered available if it has NO DNS records of any type.
    /// </summary>
    /// <param name="domain">The domain name to check.</param>
    /// <returns>True if no DNS records found (likely available), false if records exist.</returns>
    /// <exception cref="ArgumentException">Thrown if domain is null or empty.</exception>
    private async Task<bool> IsDomainAvailaibleOnline(string domain)
    {
        if (string.IsNullOrWhiteSpace(domain))
            throw new ArgumentException("domain cannot be null or empty.", nameof(domain));

        // Query external DNS API for all record types
        var url = $"https://projects.rares.eu/dns/all/{Uri.EscapeDataString(domain)}";
        using var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var dnsData = JsonSerializer.Deserialize<DnsResponse>(content, _jsonOptions);

        // Domain is available only if ALL record types are empty
        return (dnsData?.Data?.A?.Count ?? 0) == 0 &&
               (dnsData?.Data?.AAAA?.Count ?? 0) == 0 &&
               (dnsData?.Data?.CAA?.Count ?? 0) == 0 &&
               (dnsData?.Data?.CNAME?.Count ?? 0) == 0 &&
               (dnsData?.Data?.MX?.Count ?? 0) == 0 &&
               (dnsData?.Data?.NS?.Count ?? 0) == 0 &&
               (dnsData?.Data?.SOA?.Count ?? 0) == 0 &&
               (dnsData?.Data?.SRV?.Count ?? 0) == 0 &&
               (dnsData?.Data?.TXT?.Count ?? 0) == 0;
    }

    /// <summary>
    /// Creates a new domain for the specified user.
    /// Validates availability before creation.
    /// </summary>
    /// <param name="domainDto">Domain creation data.</param>
    /// <param name="userId">ID of the user creating the domain.</param>
    /// <returns>The created domain with full details.</returns>
    /// <exception cref="ArgumentNullException">Thrown if domainDto is null.</exception>
    /// <exception cref="ArgumentException">Thrown if domain name is empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown if domain already exists or creation fails.</exception>
    public async Task<DomainDto> CreateAsync(CreateDomainDto domainDto, int userId)
    {
        if (domainDto == null) throw new ArgumentNullException(nameof(domainDto));
        if (string.IsNullOrWhiteSpace(domainDto.Name)) throw new ArgumentException("Name is required.", nameof(domainDto));

        // Normalize domain name to lowercase
        var name = domainDto.Name.Trim().ToLowerInvariant();
        
        // Check availability before creating
        var existing = await CheckDomainAvailabilityAsync(name);
        if (!existing.IsAvailable)
            throw new InvalidOperationException("Domain already exists.");

        // Create new domain entity
        var entity = new DomainEntities
        {
            UserId = userId,
            Name = name,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null,
            IsActive = true,
            Records = []
        };

        var created = await _domainRepository.AddAsync(entity) ?? throw new InvalidOperationException("Domain creation failed.");
        return ToDto(created);
    }

    /// <summary>
    /// Updates an existing domain.
    /// Only the domain owner can update their domain.
    /// </summary>
    /// <param name="domainDto">Updated domain data.</param>
    /// <param name="userId">ID of the user requesting the update.</param>
    /// <returns>True if update was successful, false if domain not found or user not authorized.</returns>
    /// <exception cref="ArgumentNullException">Thrown if domainDto is null.</exception>
    /// <exception cref="ArgumentException">Thrown if domain name is empty.</exception>
    public async Task<bool> UpdateAsync(UpdateDomainDto domainDto, int userId)
    {
        if (domainDto == null) throw new ArgumentNullException(nameof(domainDto));
        if (string.IsNullOrWhiteSpace(domainDto.Name)) throw new ArgumentException("Name is required.", nameof(domainDto));

        var name = domainDto.Name.Trim().ToLowerInvariant();
        var existing = await _domainRepository.GetByNameAsync(name);
        
        // Verify domain exists and user owns it
        if (existing == null) return false;
        if (existing.UserId != userId) return false;

        // Update IsActive status if provided
        if (domainDto.IsActive.HasValue)
            existing.IsActive = domainDto.IsActive.Value;

        existing.UpdatedAt = DateTime.UtcNow;
        return await _domainRepository.UpdateAsync(existing);
    }

    /// <summary>
    /// Deletes a domain and all its associated DNS records.
    /// Only the domain owner can delete their domain.
    /// </summary>
    /// <param name="domainId">ID of the domain to delete.</param>
    /// <param name="userId">ID of the user requesting the deletion.</param>
    /// <returns>True if deletion was successful, false if domain not found or user not authorized.</returns>
    public async Task<bool> DeleteAsync(int domainId, int userId)
    {
        var existing = await _domainRepository.GetByIdAsync(domainId);
        
        // Verify domain exists and user owns it
        if (existing == null) return false;
        if (existing.UserId != userId) return false;

        return await _domainRepository.DeleteAsync(domainId);
    }

    /// <summary>
    /// Retrieves a specific domain by ID.
    /// Only the domain owner can view their domain.
    /// </summary>
    /// <param name="domainId">ID of the domain to retrieve.</param>
    /// <param name="userId">ID of the user requesting the domain.</param>
    /// <returns>Domain details if found and authorized.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if domain not found or user not authorized.</exception>
    public async Task<DomainDto> GetByIdAsync(int domainId, int userId)
    {
        var existing = await _domainRepository.GetByIdAsync(domainId);
        if (existing == null || existing.UserId != userId)
            throw new KeyNotFoundException("Domain not found.");

        return ToDto(existing);
    }

    /// <summary>
    /// Retrieves all domains belonging to a specific user.
    /// </summary>
    /// <param name="userId">ID of the user whose domains to retrieve.</param>
    /// <returns>List of all user's domains.</returns>
    public async Task<List<DomainDto>> GetAllAsync(int userId)
    {
        var all = await _domainRepository.GetAllAsync();
        var userDomains = all.Where(d => d.UserId == userId).ToList();

        return userDomains.Select(domain => ToDto(domain)).ToList();
    }

    /// <summary>
    /// Retrieves a domain by its name.
    /// </summary>
    /// <param name="name">The domain name to search for.</param>
    /// <returns>Domain details if found.</returns>
    /// <exception cref="ArgumentException">Thrown if name is empty.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if domain not found.</exception>
    public async Task<DomainDto> GetByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));

        var normalized = name.Trim().ToLowerInvariant();
        var existing = await _domainRepository.GetByNameAsync(normalized);
        if (existing == null)
            throw new KeyNotFoundException("Domain not found.");

        return ToDto(existing);
    }

    /// <summary>
    /// Converts a DomainEntities entity to a DomainDto.
    /// </summary>
    /// <param name="entity">The domain entity to convert.</param>
    /// <returns>Domain DTO with record count.</returns>
    private static DomainDto ToDto(DomainEntities entity)
    {
        return new DomainDto
        {
            Id = entity.Id,
            Name = entity.Name,
            IsActive = entity.IsActive,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            RecordCount = entity.Records?.Count ?? 0,
        };
    }
}
