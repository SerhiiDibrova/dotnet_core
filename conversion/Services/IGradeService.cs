namespace Conversion.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Conversion.Data;

    public interface IGradeService
    {
        Task<List<Grade>> GetGradesForStudentAsync(int studentId);
    }

    public class GradeService : IGradeService
    {
        private readonly ApplicationDbContext _context;

        public GradeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Grade>> GetGradesForStudentAsync(int studentId)
        {
            var grades = await _context.Grades
                .Where(g => g.StudentId == studentId)
                .ToListAsync();
            return grades;
        }
    }
}