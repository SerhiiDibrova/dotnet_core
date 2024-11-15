namespace YourNamespace.Services
{
    using System;
    using System.Threading.Tasks;
    using YourNamespace.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc;
    using System.Globalization;

    public interface IGradeService
    {
        Task<IActionResult> AddGradeAsync(Grade grade);
    }

    public class GradeService : IGradeService
    {
        private readonly YourDbContext _context;

        public GradeService(YourDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AddGradeAsync(Grade grade)
        {
            if (grade == null)
            {
                return new BadRequestObjectResult("Grade object cannot be null.");
            }

            if (string.IsNullOrEmpty(grade.StudentId))
            {
                return new BadRequestObjectResult("StudentId is required.");
            }

            if (string.IsNullOrEmpty(grade.CourseId))
            {
                return new BadRequestObjectResult("CourseId is required.");
            }

            if (!decimal.TryParse(grade.Value.ToString(), out var value) || value < 0 || value > 100)
            {
                return new BadRequestObjectResult("Value must be a numeric type between 0 and 100.");
            }

            if (!DateTime.TryParseExact(grade.DateAssigned, "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return new BadRequestObjectResult("DateAssigned must be in ISO 8601 format.");
            }

            var studentExists = await _context.Students.AnyAsync(s => s.Id == grade.StudentId);
            var courseExists = await _context.Courses.AnyAsync(c => c.Id == grade.CourseId);

            if (!studentExists || !courseExists)
            {
                return new BadRequestObjectResult("Student or Course does not exist.");
            }

            try
            {
                _context.Grades.Add(grade);
                await _context.SaveChangesAsync();
                return new OkObjectResult(grade);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}