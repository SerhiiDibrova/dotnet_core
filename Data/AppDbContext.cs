package Data;

import Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext {
    public DbSet<Student> Students { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Student>().HasKey(s => s.Id);
    }
}

public class Student {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}