
namespace ProjectStudyTool.Data;
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User>? User { get; set; } = default!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().Property(u => u.UserId).IsRequired();
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Seed();
    }
}
