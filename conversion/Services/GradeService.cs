namespace Conversion.Services
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc;
    using YourNamespace.Data; 
    using YourNamespace.Models; 
    using Microsoft.Extensions.Logging;

    public class GradeService : IGradeService
    {
        private readonly YourDbContext _context;
        private readonly ILogger<GradeService> _logger;

        public GradeService(YourDbContext context, ILogger<GradeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ActionResult<Grade>> AddGradeAsync(Grade grade)
        {
            if (grade == null)
            {
                return new BadRequestObjectResult("Grade cannot be null.");
            }

            if (grade.StudentId <= 0 || grade.CourseId <= 0 || grade.Score < 0 || grade.Score > 100 || grade.DateAssigned < DateTime.Now)
            {
                return new BadRequestObjectResult("Invalid grade data.");
            }

            if (await _context.Grades.AnyAsync(g => g.StudentId == grade.StudentId && g.CourseId == grade.CourseId && g.DateAssigned == grade.DateAssigned))
            {
                return new BadRequestObjectResult("Grade already exists for this student and course on the same date.");
            }

            try
            {
                _context.Grades.Add(grade);
                await _context.SaveChangesAsync();
                return new ActionResult<Grade>(grade);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error occurred while adding grade.");
                return new ObjectResult("An error occurred while saving the grade.") { StatusCode = 500 };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding grade.");
                return new ObjectResult("An unexpected error occurred.") { StatusCode = 500 };
            }
        }
    }
}