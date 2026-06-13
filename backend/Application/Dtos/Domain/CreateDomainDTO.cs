using Application.Dtos.Auth;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Domain;

/// <summary>
/// Request DTO for creating a new DNS domain.
/// </summary>
public class CreateDomainDto
{
    /// <summary>
    /// The domain name to create (e.g., "example.com").
    /// Must be unique and follow valid domain name format.
    /// </summary>
    /// <example>example.com</example>
    [Required]
    public string Name { get; set; }
}