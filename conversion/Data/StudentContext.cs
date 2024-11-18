namespace Conversion.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class StudentContext : DbContext
    {
        public StudentContext(DbContextOptions<StudentContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }

        public async Task<IActionResult> AddStudentAsync(Student student)
        {
            if (student == null || string.IsNullOrWhiteSpace(student.Name) || student.Age <= 0 || string.IsNullOrWhiteSpace(student.Email))
            {
                return new BadRequestObjectResult("Invalid student data.");
            }

            try
            {
                await Students.AddAsync(student);
                await SaveChangesAsync();
                return new OkResult();
            }
            catch (DbUpdateException ex)
            {
                return new StatusCodeResult(500, $"Error occurred while saving the student: {ex.Message}");
            }
        }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
    }
}