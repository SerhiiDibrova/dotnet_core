namespace YourNamespace.Services
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using YourNamespace.Data;
    using YourNamespace.Exceptions;
    using YourNamespace.Models;
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

        public async Task AddGradeAsync(AddGradeRequest request)
        {
            if (request == null)
            {
                throw new ValidationException("Request cannot be null.");
            }

            ValidateRequest(request);

            var grade = new Grade
            {
                StudentId = request.StudentId,
                CourseId = request.CourseId,
                Score = request.Score,
                DateAssigned = request.DateAssigned
            };

            try
            {
                await _context.Grades.AddAsync(grade);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while saving the grade.");
                throw new DatabaseException("An error occurred while saving the grade.", ex);
            }
        }

        private void ValidateRequest(AddGradeRequest request)
        {
            if (string.IsNullOrEmpty(request.StudentId))
                throw new ValidationException("StudentId is required.");
            if (string.IsNullOrEmpty(request.CourseId))
                throw new ValidationException("CourseId is required.");
            if (request.Score < 0 || request.Score > 100)
                throw new ValidationException("Score must be between 0 and 100.");
            if (request.DateAssigned == default(DateTime) || request.DateAssigned > DateTime.Now)
                throw new ValidationException("DateAssigned is required and must be a valid date in the past.");
        }
    }
}