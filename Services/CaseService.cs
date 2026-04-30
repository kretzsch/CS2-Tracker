using Cs2Tracker.Models;

namespace Cs2Tracker.Services;

public class CaseService
{
    private List<Case> _cases = new()
    {
        new Case
        {
            Id = 1,
            Name = "CS:GO Weapon Case",
            Price = 0.45m,
            UpdatedAt = DateTime.UtcNow
        }
    };

    public List<Case> GetAll()
    {
        return _cases;
    }
}
