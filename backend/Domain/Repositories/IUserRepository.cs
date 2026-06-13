using Domain.Entities;

namespace Domain.Repositories;

/// <summary>
/// Repository interface for User entity operations.
/// Handles data access for user authentication and management.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Retrieves a user by their email address.
    /// Used for authentication and checking email uniqueness.
    /// </summary>
    /// <param name="email">The email address to search for.</param>
    /// <returns>The user if found, null otherwise.</returns>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Adds a new user to the database.
    /// Used during user registration.
    /// </summary>
    /// <param name="user">The user entity to add.</param>
    /// <returns>The created user with generated ID, or null if creation failed.</returns>
    Task<User?> Add(User user);
}