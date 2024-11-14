namespace YourNamespace.Services
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using YourNamespace.Models;
    using YourNamespace.Data;
    using Microsoft.EntityFrameworkCore;

    public interface IGradeService
    {
        Task<Grade> AddGradeAsync(Grade grade);
    }

    public class GradeService : IGradeService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GradeService> _logger;

        public GradeService(ApplicationDbContext context, ILogger<GradeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Grade> AddGradeAsync(Grade grade)
        {
            if (grade == null)
            {
                _logger.LogError("Grade cannot be null.");
                throw new BadHttpRequestException("Grade cannot be null.");
            }

            if (string.IsNullOrEmpty(grade.StudentId))
            {
                _logger.LogError("StudentId is required.");
                throw new BadHttpRequestException("StudentId is required.");
            }

            if (string.IsNullOrEmpty(grade.CourseId))
            {
                _logger.LogError("CourseId is required.");
                throw new BadHttpRequestException("CourseId is required.");
            }

            if (grade.Score < 0 || grade.Score > 100)
            {
                _logger.LogError("Score must be between 0 and 100.");
                throw new BadHttpRequestException("Score must be between 0 and 100.");
            }

            var studentExists = await _context.Students.AnyAsync(s => s.Id == grade.StudentId);
            var courseExists = await _context.Courses.AnyAsync(c => c.Id == grade.CourseId);

            if (!studentExists)
            {
                _logger.LogError("Student does not exist.");
                throw new BadHttpRequestException("Student does not exist.");
            }

            if (!courseExists)
            {
                _logger.LogError("Course does not exist.");
                throw new BadHttpRequestException("Course does not exist.");
            }

            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();

            return grade;
        }
    }
}