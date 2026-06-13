using Application.Dtos;
using Application.Dtos.Domain;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace IntegrationTests;

[Collection("Integration Tests")]
public class DomainsIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;
    private readonly TestAuthHelper _authHelper;

    public DomainsIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _authHelper = new TestAuthHelper(_client, _factory);
    }

    [Fact]
    public async Task CreateDomain_WithValidData_ShouldReturnCreated()
    {
        // Arrange
        await _authHelper.ClearDatabaseAsync();
        var token = await _authHelper.RegisterAndLoginAsync("domainowner@example.com", "Password123!");
        _authHelper.SetAuthToken(token);

        var request = new CreateDomainDto
        {
            Name = "example.com"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Domains", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<DomainDto>();
        Assert.NotNull(result);
        Assert.Equal("example.com", result.Name);
        Assert.True(result.Id > 0);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task CreateDomain_WithoutAuthentication_ShouldReturnUnauthorized()
    {
        // Arrange
        await _authHelper.ClearDatabaseAsync();
        _authHelper.ClearAuthToken();

        var request = new CreateDomainDto
        {
            Name = "unauthorized.com"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Domains", request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateDomain_WithEmptyName_ShouldReturnBadRequest()
    {
        // Arrange
        await _authHelper.ClearDatabaseAsync();
        var token = await _authHelper.RegisterAndLoginAsync("user@example.com", "Password123!");
        _authHelper.SetAuthToken(token);

        var request = new CreateDomainDto
        {
            Name = ""
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Domains", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetAllDomains_WithAuthentication_ShouldReturnDomains()
    {
        // Arrange
        await _authHelper.ClearDatabaseAsync();
        var token = await _authHelper.RegisterAndLoginAsync("listuser@example.com", "Password123!");
        _authHelper.SetAuthToken(token);

        // Create some domains
        await _client.PostAsJsonAsync("/api/Domains", new CreateDomainDto { Name = "domain1.com" });
        await _client.PostAsJsonAsync("/api/Domains", new CreateDomainDto { Name = "domain2.com" });

        // Act
        var response = await _client.GetAsync("/api/Domains");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<List<DomainsListDto>>();
        Assert.NotNull(result);
        Assert.True(result.Count >= 2);
    }

    [Fact]
    public async Task GetAllDomains_WithoutAuthentication_ShouldReturnUnauthorized()
    {
        // Arrange
        _authHelper.ClearAuthToken();

        // Act
        var response = await _client.GetAsync("/api/Domains");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetDomainById_WithValidId_ShouldReturnDomain()
    {
        // Arrange
        await _authHelper.ClearDatabaseAsync();
        var token = await _authHelper.RegisterAndLoginAsync("getuser@example.com", "Password123!");
        _authHelper.SetAuthToken(token);

        // Create a domain
        var createResponse = await _client.PostAsJsonAsync("/api/Domains", new CreateDomainDto { Name = "getdomain.com" });
        var createdDomain = await createResponse.Content.ReadFromJsonAsync<DomainDto>();

        // Act
        var response = await _client.GetAsync($"/api/Domains/{createdDomain!.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<DomainDto>();
        Assert.NotNull(result);
        Assert.Equal(createdDomain.Id, result.Id);
        Assert.Equal("getdomain.com", result.Name);
    }

    [Fact]
    public async Task GetDomainById_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        await _authHelper.ClearDatabaseAsync();
        var token = await _authHelper.RegisterAndLoginAsync("user@example.com", "Password123!");
        _authHelper.SetAuthToken(token);

        // Act
        var response = await _client.GetAsync("/api/Domains/99999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateDomain_WithValidData_ShouldReturnOk()
    {
        // Arrange
        await _authHelper.ClearDatabaseAsync();
        var token = await _authHelper.RegisterAndLoginAsync("updateuser@example.com", "Password123!");
        _authHelper.SetAuthToken(token);

        // Create a domain
        var createResponse = await _client.PostAsJsonAsync("/api/Domains", new CreateDomainDto { Name = "olddomain.com" });
        var createdDomain = await createResponse.Content.ReadFromJsonAsync<DomainDto>();

        var updateRequest = new UpdateDomainDto
        {
            Name = "newdomain.com"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/Domains/{createdDomain!.Id}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<DomainDto>();
        Assert.NotNull(result);
        Assert.Equal("newdomain.com", result.Name);
    }

    [Fact]
    public async Task UpdateDomain_OwnedByAnotherUser_ShouldReturnForbidden()
    {
        // Arrange
        await _authHelper.ClearDatabaseAsync();
        
        // User 1 creates a domain
        var token1 = await _authHelper.RegisterAndLoginAsync("owner@example.com", "Password123!");
        _authHelper.SetAuthToken(token1);
        var createResponse = await _client.PostAsJsonAsync("/api/Domains", new CreateDomainDto { Name = "owneddomain.com" });
        var createdDomain = await createResponse.Content.ReadFromJsonAsync<DomainDto>();

        // User 2 tries to update it
        var token2 = await _authHelper.RegisterAndLoginAsync("attacker@example.com", "Password123!");
        _authHelper.SetAuthToken(token2);

        var updateRequest = new UpdateDomainDto
        {
            Name = "hackeddomain.com"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/Domains/{createdDomain!.Id}", updateRequest);

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteDomain_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        await _authHelper.ClearDatabaseAsync();
        var token = await _authHelper.RegisterAndLoginAsync("deleteuser@example.com", "Password123!");
        _authHelper.SetAuthToken(token);

        // Create a domain
        var createResponse = await _client.PostAsJsonAsync("/api/Domains", new CreateDomainDto { Name = "deleteme.com" });
        var createdDomain = await createResponse.Content.ReadFromJsonAsync<DomainDto>();

        // Act
        var response = await _client.DeleteAsync($"/api/Domains/{createdDomain!.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify it's deleted
        var getResponse = await _client.GetAsync($"/api/Domains/{createdDomain.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteDomain_OwnedByAnotherUser_ShouldReturnForbidden()
    {
        // Arrange
        await _authHelper.ClearDatabaseAsync();
        
        // User 1 creates a domain
        var token1 = await _authHelper.RegisterAndLoginAsync("owner2@example.com", "Password123!");
        _authHelper.SetAuthToken(token1);
        var createResponse = await _client.PostAsJsonAsync("/api/Domains", new CreateDomainDto { Name = "protected.com" });
        var createdDomain = await createResponse.Content.ReadFromJsonAsync<DomainDto>();

        // User 2 tries to delete it
        var token2 = await _authHelper.RegisterAndLoginAsync("attacker2@example.com", "Password123!");
        _authHelper.SetAuthToken(token2);

        // Act
        var response = await _client.DeleteAsync($"/api/Domains/{createdDomain!.Id}");

        // Assert
        Assert.True(response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.NotFound);
    }
}
