namespace YourNamespace.Services
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using YourNamespace.Data;
    using YourNamespace.Models;
    using Microsoft.Extensions.Logging;

    public class GradeService
    {
        private readonly YourDbContext _context;
        private readonly ILogger<GradeService> _logger;

        public GradeService(YourDbContext context, ILogger<GradeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> AddGradeAsync(Grade grade)
        {
            if (grade == null || !IsValidGrade(grade))
            {
                return new BadRequestObjectResult(new { error = "Invalid grade data." });
            }

            try
            {
                await _context.Grades.AddAsync(grade);
                await _context.SaveChangesAsync();
                return new CreatedAtActionResult(nameof(AddGradeAsync), new { id = grade.Id }, grade);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while saving the grade.");
                return new ObjectResult(new { error = "An error occurred while saving the grade." }) { StatusCode = StatusCodes.Status500InternalServerError };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return new ObjectResult(new { error = "An unexpected error occurred." }) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }

        private bool IsValidGrade(Grade grade)
        {
            return !string.IsNullOrEmpty(grade.StudentId) && grade.Score >= 0 && grade.Score <= 100;
        }
    }
}