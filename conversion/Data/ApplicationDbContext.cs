namespace Conversion.Data
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<Grade> Grades { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public async Task<List<Grade>> GetGradesByStudentIdAsync(int studentId)
        {
            try
            {
                return await Grades.AsNoTracking().Where(g => g.StudentId == studentId).ToListAsync();
            }
            catch
            {
                // Handle exceptions as needed
                throw;
            }
        }
    }

    public class Grade
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string LetterGrade { get; set; } = string.Empty;
    }
}