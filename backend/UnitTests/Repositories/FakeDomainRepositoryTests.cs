using Domain.Entities;
using UnitTests.Fakes;
using Xunit;

namespace UnitTests.Repositories;

public class FakeDomainRepositoryTests
{
    [Fact]
    public async Task AddAsync_ShouldAddDomainAndReturnWithId()
    {
        // Arrange
        var repository = new FakeDomainRepository();
        var domain = new DomainEntities
        {
            Name = "example.com",
            UserId = 1
        };

        // Act
        var result = await repository.AddAsync(domain);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("example.com", result.Name);
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnDomain_WhenDomainExists()
    {
        // Arrange
        var repository = new FakeDomainRepository();
        var domain = new DomainEntities
        {
            Name = "example.com",
            UserId = 1
        };
        await repository.AddAsync(domain);

        // Act
        var result = await repository.GetByNameAsync("example.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("example.com", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnDomain_WhenDomainExists()
    {
        // Arrange
        var repository = new FakeDomainRepository();
        var domain = new DomainEntities
        {
            Name = "example.com",
            UserId = 1
        };
        var added = await repository.AddAsync(domain);

        // Act
        var result = await repository.GetByIdAsync(added.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(added.Id, result.Id);
        Assert.Equal("example.com", result.Name);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllDomains()
    {
        // Arrange
        var repository = new FakeDomainRepository();
        await repository.AddAsync(new DomainEntities { Name = "example1.com", UserId = 1 });
        await repository.AddAsync(new DomainEntities { Name = "example2.com", UserId = 1 });
        await repository.AddAsync(new DomainEntities { Name = "example3.com", UserId = 2 });

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateDomain_WhenDomainExists()
    {
        // Arrange
        var repository = new FakeDomainRepository();
        var domain = new DomainEntities
        {
            Name = "example.com",
            UserId = 1
        };
        var added = await repository.AddAsync(domain);
        added.Name = "updated.com";

        // Act
        var result = await repository.UpdateAsync(added);

        // Assert
        Assert.True(result);
        var updated = await repository.GetByIdAsync(added.Id);
        Assert.NotNull(updated);
        Assert.Equal("updated.com", updated.Name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenDomainDoesNotExist()
    {
        // Arrange
        var repository = new FakeDomainRepository();
        var domain = new DomainEntities
        {
            Id = 999,
            Name = "nonexistent.com",
            UserId = 1
        };

        // Act
        var result = await repository.UpdateAsync(domain);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteDomain_WhenDomainExists()
    {
        // Arrange
        var repository = new FakeDomainRepository();
        var domain = new DomainEntities
        {
            Name = "example.com",
            UserId = 1
        };
        var added = await repository.AddAsync(domain);

        // Act
        var result = await repository.DeleteAsync(added.Id);

        // Assert
        Assert.True(result);
        Assert.Equal(0, repository.Count);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenDomainDoesNotExist()
    {
        // Arrange
        var repository = new FakeDomainRepository();

        // Act
        var result = await repository.DeleteAsync(999);

        // Assert
        Assert.False(result);
    }
}
