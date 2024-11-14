namespace YourNamespace.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using YourNamespace.Models;
    using Microsoft.Extensions.Logging;

    public class StudentService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<StudentService> _logger;

        public StudentService(AppDbContext context, ILogger<StudentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Grade>> GetGradesForStudentAsync(int studentId)
        {
            if (studentId <= 0)
            {
                throw new ArgumentException("Invalid student ID.");
            }

            try
            {
                var grades = await _context.Grades
                    .Where(g => g.StudentId == studentId)
                    .ToListAsync();

                if (!grades.Any())
                {
                    return new List<Grade>(); // Return an empty list if no grades found
                }

                return grades;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving grades for student ID {StudentId}.", studentId);
                throw new Exception("An error occurred while retrieving grades.", ex);
            }
        }
    }
}