package conversion.Data;

import Microsoft.EntityFrameworkCore;
import System.ComponentModel.DataAnnotations;
import System.ComponentModel.DataAnnotations.Schema;

public class AppDbContext : DbContext {
    public DbSet<Student> Students { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Student>()
            .HasKey(s => s.Id);

        modelBuilder.Entity<Student>()
            .Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Student>()
            .Property(s => s.Age)
            .HasRange(0, 120);
    }
}

public class Student {
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Range(0, 120)]
    public int Age { get; set; }
}