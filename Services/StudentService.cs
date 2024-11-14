namespace YourNamespace.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public interface IStudentService
    {
        Task<(List<Grade> Grades, int StatusCode, string Message)> GetGradesForStudentAsync(int studentId);
    }

    public class StudentService : IStudentService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<StudentService> _logger;

        public StudentService(AppDbContext context, ILogger<StudentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<(List<Grade> Grades, int StatusCode, string Message)> GetGradesForStudentAsync(int studentId)
        {
            if (studentId <= 0)
            {
                return (null, StatusCodes.Status400BadRequest, "Invalid student ID.");
            }

            try
            {
                var grades = await _context.Grades
                    .Where(g => g.StudentId == studentId)
                    .ToListAsync();

                if (!grades.Any())
                {
                    return (null, StatusCodes.Status404NotFound, "No grades found for the specified student ID.");
                }

                return (grades, StatusCodes.Status200OK, "Grades retrieved successfully.");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving grades for student ID {StudentId}.", studentId);
                return (null, StatusCodes.Status500InternalServerError, "An internal server error occurred.");
            }
        }
    }

    public class Grade
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string Subject { get; set; }
        public int Score { get; set; }
    }

    public class AppDbContext : DbContext
    {
        public DbSet<Grade> Grades { get; set; }
    }
}