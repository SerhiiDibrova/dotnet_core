package conversion.Data;

using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

public class ApplicationDbContext : DbContext
{
    public DbSet<Grade> Grades { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Grade>().ToTable("Grades");
    }
}

public class Grade
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Range(0, 100)]
    public int Score { get; set; }

    [StringLength(200)]
    public string Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}