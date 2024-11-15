namespace YourNamespace.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public class Student
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }

    public class Grade
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }
    }

    public class GradeService
    {
        private readonly YourDbContext _context;

        public GradeService(YourDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddGradeAsync(Grade grade)
        {
            if (!await _context.Students.AnyAsync(s => s.Id == grade.StudentId) ||
                !await _context.Courses.AnyAsync(c => c.Id == grade.CourseId))
            {
                return false;
            }

            try
            {
                _context.Grades.Add(grade);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}