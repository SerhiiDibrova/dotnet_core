namespace Conversion.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public class GradeService : IGradeService
    {
        private readonly AppDbContext _context;

        public GradeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Grade>> GetGradesForStudentAsync(int studentId)
        {
            return await _context.Grades
                .Where(g => g.StudentId == studentId)
                .ToListAsync();
        }
    }
}