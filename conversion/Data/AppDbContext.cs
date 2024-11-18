namespace Conversion.Data
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Student>()
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Student>()
                .Property(s => s.Age)
                .IsRequired();
        }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class StudentController
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetStudent(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task AddStudent(Student student)
        {
            if (student == null || string.IsNullOrWhiteSpace(student.Name) || student.Age <= 0)
            {
                throw new ArgumentException("Invalid student data.");
            }

            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStudent(Student student)
        {
            if (student == null || string.IsNullOrWhiteSpace(student.Name) || student.Age <= 0)
            {
                throw new ArgumentException("Invalid student data.");
            }

            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
        }
    }
}