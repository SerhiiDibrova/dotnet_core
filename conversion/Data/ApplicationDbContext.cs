namespace Conversion.Data
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ApplicationDbContext : DbContext
    {
        public DbSet<Grade> Grades { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Grade>().ToTable("Grades");
            modelBuilder.Entity<Grade>().HasKey(g => g.Id);
            modelBuilder.Entity<Grade>().Property(g => g.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Grade>().Property(g => g.Score).IsRequired();
            base.OnModelCreating(modelBuilder);
        }
    }

    public class Grade
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
    }

    public interface IGradeRepository
    {
        void Add(Grade grade);
        IEnumerable<Grade> GetAll();
    }

    public class GradeRepository : IGradeRepository
    {
        private readonly ApplicationDbContext _context;

        public GradeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Grade grade)
        {
            if (grade == null) throw new ArgumentNullException(nameof(grade));
            _context.Grades.Add(grade);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while saving the grade.", ex);
            }
        }

        public IEnumerable<Grade> GetAll()
        {
            return _context.Grades.ToList();
        }
    }
}