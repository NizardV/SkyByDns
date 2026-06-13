using Domain.Entities;
using Domain.Repositories;

namespace UnitTests.Fakes;

public class FakeUserRepository : IUserRepository
{
    private readonly List<User> _users = new();
    private int _nextId = 1;

    public async Task<User?> GetByEmailAsync(string email)
    {
        await Task.CompletedTask;
        return _users.FirstOrDefault(u => u.Email == email);
    }

    public async Task<User?> Add(User user)
    {
        await Task.CompletedTask;
        user.Id = _nextId++;
        _users.Add(user);
        return user;
    }

    // Helper methods for testing
    public void Clear()
    {
        _users.Clear();
        _nextId = 1;
    }

    public List<User> GetAll()
    {
        return _users.ToList();
    }
}
