namespace CS2Tracker.Api.Models
{
    public class Skin
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}