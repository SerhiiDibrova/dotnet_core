package Data;

using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<Grade> Grades { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Grade>().ToTable("Grades");
        modelBuilder.Entity<Grade>().HasKey(g => g.Id);
        modelBuilder.Entity<Grade>().Property(g => g.StudentName).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<Grade>().Property(g => g.Subject).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<Grade>().Property(g => g.Score).IsRequired();
    }
}

public class Grade
{
    public int Id { get; set; }
    public string StudentName { get; set; }
    public string Subject { get; set; }
    public int Score { get; set; }
}