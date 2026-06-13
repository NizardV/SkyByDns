using Domain.Entities;
using UnitTests.Fakes;
using Xunit;

namespace UnitTests.Repositories;

public class FakeUserRepositoryTests
{
    [Fact]
    public async Task Add_ShouldAddUserAndReturnWithId()
    {
        // Arrange
        var repository = new FakeUserRepository();
        var user = new User
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            PasswordHash = "hashedpassword",
            Role = "User",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        // Act
        var result = await repository.Add(user);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("test@example.com", result.Email);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var repository = new FakeUserRepository();
        var user = new User
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            PasswordHash = "hashedpassword",
            Role = "User",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
        await repository.Add(user);

        // Act
        var result = await repository.GetByEmailAsync("test@example.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@example.com", result.Email);
        Assert.Equal("John", result.FirstName);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        // Arrange
        var repository = new FakeUserRepository();

        // Act
        var result = await repository.GetByEmailAsync("nonexistent@example.com");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Add_ShouldIncrementId_ForMultipleUsers()
    {
        // Arrange
        var repository = new FakeUserRepository();
        var user1 = new User { Email = "user1@example.com", FirstName = "User", LastName = "One", PasswordHash = "hash1", Role = "User", CreatedAt = DateTime.UtcNow, IsActive = true };
        var user2 = new User { Email = "user2@example.com", FirstName = "User", LastName = "Two", PasswordHash = "hash2", Role = "User", CreatedAt = DateTime.UtcNow, IsActive = true };

        // Act
        var result1 = await repository.Add(user1);
        var result2 = await repository.Add(user2);

        // Assert
        Assert.Equal(1, result1.Id);
        Assert.Equal(2, result2.Id);
    }
}
