using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectStudyTool.Models;

namespace ProjectStudyTool.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<User>? User { get; set; } = default!;
    
    public DbSet<Card>? Cards { get; set; } = default!;
    public DbSet<CardSet>? CardSets { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>().Property(u => u.UserId).IsRequired();
        modelBuilder.Entity<User>().ToTable("User");

        modelBuilder.Entity<Card>().Property(c => c.CardId).IsRequired();
        modelBuilder.Entity<Card>()
        .ToTable("Cards"); 

        modelBuilder.Entity<CardSet>().Property(cs => cs.CardSetId).IsRequired();
        modelBuilder.Entity<CardSet>()
        .ToTable("CardSets"); 

        modelBuilder.Seed();
    }

    public DbSet<ProjectStudyTool.Models.Card> Card { get; set; } = default!;

    public DbSet<ProjectStudyTool.Models.CardSet> CardSet { get; set; } = default!;
}
