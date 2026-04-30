using Cs2Tracker.Data;
using Cs2Tracker.Models;
using Microsoft.EntityFrameworkCore;

namespace Cs2Tracker.Services;

public class CaseService
{
    private readonly ApplicationDbContext _db;

    public CaseService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<Case>> GetAllAsync()
    {
        return await _db.Cases.ToListAsync();
    }

    public async Task<Case?> GetByNameAsync(string name)
    {
        return await _db.Cases.FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task AddOrUpdateAsync(Case caseItem)
    {
        var existing = await GetByNameAsync(caseItem.Name);

        if (existing != null)
        {
            existing.Price = caseItem.Price;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            _db.Cases.Add(caseItem);
        }

        await _db.SaveChangesAsync();
    }
}
