using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Entity Framework Core implementation of the User repository.
    /// Provides data access for user authentication and management.
    /// </summary>
    public class EfUserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the EfUserRepository with the database context.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public EfUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a user by their email address.
        /// Uses AsNoTracking for read-only queries to improve performance.
        /// </summary>
        /// <param name="email">The email address to search for.</param>
        /// <returns>The user if found, null otherwise.</returns>
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="user">The user entity to add.</param>
        /// <returns>The created user with generated ID.</returns>
        public async Task<User?> Add(User user)
        {
            var entity = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return entity.Entity;
        }
    }
}