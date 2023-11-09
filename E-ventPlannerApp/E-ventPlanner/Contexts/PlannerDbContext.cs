using System.Reflection.Metadata.Ecma335;
using E_ventPlanner.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_ventPlanner.Contexts;

public class PlannerDbContext : IdentityDbContext
{
    public PlannerDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserData>()
            .HasKey(c => c.UserId);
    }

    public DbSet<Event> Events { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<UserData> UserData { get; set; }
}