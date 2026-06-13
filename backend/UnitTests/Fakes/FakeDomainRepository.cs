using Domain.Entities;
using Domain.Repositories;

namespace UnitTests.Fakes;

public class FakeDomainRepository : IDomainRepository
{
    private readonly List<DomainEntities> _domains = new();
    private int _nextId = 1;

    public async Task<List<DomainEntities>> GetAllAsync()
    {
        await Task.CompletedTask;
        return _domains.ToList();
    }

    public async Task<DomainEntities?> GetByNameAsync(string name)
    {
        await Task.CompletedTask;
        return _domains.FirstOrDefault(d => d.Name == name);
    }

    public async Task<DomainEntities?> GetByIdAsync(int id)
    {
        await Task.CompletedTask;
        return _domains.FirstOrDefault(d => d.Id == id);
    }

    public async Task<DomainEntities?> AddAsync(DomainEntities domainEntities)
    {
        await Task.CompletedTask;
        domainEntities.Id = _nextId++;
        _domains.Add(domainEntities);
        return domainEntities;
    }

    public async Task<bool> UpdateAsync(DomainEntities domainEntities)
    {
        await Task.CompletedTask;
        var existing = _domains.FirstOrDefault(d => d.Id == domainEntities.Id);
        if (existing == null)
            return false;

        var index = _domains.IndexOf(existing);
        _domains[index] = domainEntities;
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await Task.CompletedTask;
        var domain = _domains.FirstOrDefault(d => d.Id == id);
        if (domain == null)
            return false;

        _domains.Remove(domain);
        return true;
    }

    // Helper methods for testing
    public void Clear()
    {
        _domains.Clear();
        _nextId = 1;
    }

    public int Count => _domains.Count;
}
