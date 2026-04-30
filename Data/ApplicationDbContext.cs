using Microsoft.EntityFrameworkCore;
using Cs2Tracker.Models;

namespace Cs2Tracker.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Case> Cases { get; set; }
}