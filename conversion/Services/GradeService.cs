namespace Conversion.Services
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Conversion.Data;
    using Conversion.Models;
    using Conversion.Exceptions;
    using Microsoft.Extensions.Logging;

    public class GradeService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GradeService> _logger;

        public GradeService(ApplicationDbContext context, ILogger<GradeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddGradeAsync(Grade grade)
        {
            ValidateGrade(grade);
            try
            {
                await _context.Grades.AddAsync(grade);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while saving the grade.");
                throw new ApiException("An error occurred while saving the grade.", 500);
            }
        }

        private void ValidateGrade(Grade grade)
        {
            if (grade == null)
            {
                throw new ApiException("Grade cannot be null.", 400);
            }

            if (grade.StudentId <= 0)
            {
                throw new ApiException("Invalid StudentId.", 400);
            }

            if (grade.CourseId <= 0)
            {
                throw new ApiException("Invalid CourseId.", 400);
            }

            if (grade.Score < 0 || grade.Score > 100)
            {
                throw new ApiException("Score must be between 0 and 100.", 400);
            }

            if (grade.DateAssigned == default)
            {
                throw new ApiException("DateAssigned is required.", 400);
            }
        }
    }
}