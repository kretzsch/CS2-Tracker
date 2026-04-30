namespace Cs2Tracker.Models;

public class Case
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public DateTime UpdatedAt { get; set; }
}
