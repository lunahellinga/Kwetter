using Microsoft.EntityFrameworkCore;
using Postgres.Data;
using Postgres.Models;

namespace Postgres.Services;

public class KweetService
{
    private readonly DataContext _dataContext;

    public KweetService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<List<Kweet>> GetAsync()
    {
        return await _dataContext.Kweets.OrderByDescending(k => k.Timestamp)
            .Take(100)
            .ToListAsync();
    }

    public async Task<Kweet?> GetAsync(Guid id)
    {
        return await _dataContext.Kweets.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task CreateAsync(Kweet newKweet)
    {
        await _dataContext.Kweets.AddAsync(newKweet);
        await _dataContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Kweet updatedKweet)
    {
        var kweet = await _dataContext.Kweets.FindAsync(updatedKweet.Id);

        if (kweet != null)
        {
            kweet.Message = updatedKweet.Message;
            kweet.Timestamp = updatedKweet.Timestamp;
            await _dataContext.SaveChangesAsync();
        }
    }

    public async Task RemoveAsync(Guid id)
    {
        var kweet = await _dataContext.Kweets.FindAsync(id);
        if (kweet != null)
        {
            _dataContext.Remove(kweet);
            await _dataContext.SaveChangesAsync();
        }
    }
}