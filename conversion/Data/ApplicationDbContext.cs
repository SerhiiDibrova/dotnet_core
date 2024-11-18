namespace Conversion.Data
{
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            _options = options;
        }

        public DbSet<Grade> Grades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Grade>()
                .HasKey(g => g.Id);

            modelBuilder.Entity<Grade>()
                .Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Grade>()
                .Property(g => g.Score)
                .IsRequired();
        }

        public void SaveChangesToDatabase()
        {
            SaveChanges();
        }
    }

    public class Grade
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
    }
}