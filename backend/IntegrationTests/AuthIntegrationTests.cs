using Application.Dtos;
using Application.Dtos.Auth;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace IntegrationTests;

[Collection("Integration Tests")]
public class AuthIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;
    private readonly TestAuthHelper _authHelper;

    public AuthIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _authHelper = new TestAuthHelper(_client, _factory);
    }

    [Fact]
    public async Task Register_WithValidData_ShouldReturnCreatedAndToken()
    {
        // Arrange
        await _authHelper.ClearDatabaseAsync();
        var request = new RegisterRequestDto
        {
            Email = "newuser@example.com",
            Password = "SecurePassword123!",
            FirstName = "John",
            LastName = "Doe",
            Role = "User"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Auth/register", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<LoginResultDto>();
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.Token);
        Assert.NotEmpty(result.Token);
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_ShouldReturnBadRequest()
    {
        // Arrange
        await _authHelper.ClearDatabaseAsync();
        var request = new RegisterRequestDto
        {
            Email = "duplicate@example.com",
            Password = "SecurePassword123!",
            FirstName = "John",
            LastName = "Doe"
        };

        // Register first user
        await _client.PostAsJsonAsync("/api/Auth/register", request);

        // Act - Try to register again with same email
        var response = await _client.PostAsJsonAsync("/api/Auth/register", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Register_WithInvalidEmail_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            Email = "", // Invalid email
            Password = "SecurePassword123!",
            FirstName = "John",
            LastName = "Doe"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Auth/register", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnOkAndToken()
    {
        // Arrange
        await _authHelper.ClearDatabaseAsync();
        var email = "loginuser@example.com";
        var password = "ValidPassword123!";
        
        // Register user first
        await _authHelper.RegisterAndLoginAsync(email, password);

        var loginRequest = new LoginRequestDto
        {
            Email = email,
            Password = password
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Auth/login", loginRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<LoginResultDto>();
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.Token);
        Assert.NotEmpty(result.Token);
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ShouldReturnUnauthorized()
    {
        // Arrange
        await _authHelper.ClearDatabaseAsync();
        var email = "wrongpass@example.com";
        var correctPassword = "CorrectPassword123!";
        
        // Register user
        await _authHelper.RegisterAndLoginAsync(email, correctPassword);

        var loginRequest = new LoginRequestDto
        {
            Email = email,
            Password = "WrongPassword123!"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Auth/login", loginRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithNonExistentUser_ShouldReturnUnauthorized()
    {
        // Arrange
        await _authHelper.ClearDatabaseAsync();
        var loginRequest = new LoginRequestDto
        {
            Email = "nonexistent@example.com",
            Password = "SomePassword123!"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Auth/login", loginRequest);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithMissingCredentials_ShouldReturnBadRequest()
    {
        // Arrange
        var loginRequest = new LoginRequestDto
        {
            Email = "",
            Password = ""
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Auth/login", loginRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
