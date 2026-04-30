using CS2Tracker.Api.Models;

namespace Cs2Tracker.Api.Services;

public class SkinService
{
    private List<Skin> _skins = new()
    {
        new Skin
        {
            Id = 1,
            Name = "AK-47 | Redline",
            Price = 12.5m,
            UpdatedAt = DateTime.UtcNow
        }
    };

    public List<Skin> GetAll()
    {
        return _skins;
    }
}