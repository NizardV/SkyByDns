using Application.Dtos.Auth;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace IntegrationTests;

public class TestAuthHelper
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public TestAuthHelper(HttpClient client, CustomWebApplicationFactory factory)
    {
        _client = client;
        _factory = factory;
    }

    public async Task<string> RegisterAndLoginAsync(string email = "test@example.com", string password = "Password123!", string firstName = "Test", string lastName = "User")
    {
        // Register user
        var registerRequest = new RegisterRequestDto
        {
            Email = email,
            Password = password,
            FirstName = firstName,
            LastName = lastName,
            Role = "User"
        };

        var registerResponse = await _client.PostAsJsonAsync("/api/Auth/register", registerRequest);
        
        if (!registerResponse.IsSuccessStatusCode)
        {
            // User might already exist, try to login
            return await LoginAsync(email, password);
        }

        var registerResult = await registerResponse.Content.ReadFromJsonAsync<LoginResultDto>();
        return registerResult?.Token ?? throw new Exception("Failed to get token from registration");
    }

    public async Task<string> LoginAsync(string email = "test@example.com", string password = "Password123!")
    {
        var loginRequest = new LoginRequestDto
        {
            Email = email,
            Password = password
        };

        var response = await _client.PostAsJsonAsync("/api/Auth/login", loginRequest);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<LoginResultDto>();
        return result?.Token ?? throw new Exception("Failed to get token from login");
    }

    public void SetAuthToken(string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public void ClearAuthToken()
    {
        _client.DefaultRequestHeaders.Authorization = null;
    }

    public async Task ClearDatabaseAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        // For InMemory database, just delete and recreate
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }
}
