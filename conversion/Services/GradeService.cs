namespace Conversion.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Mvc;

    public interface IGradeService
    {
        Task<ActionResult<Grade>> AddGradeAsync(Grade grade);
    }

    public class GradeService : IGradeService
    {
        private readonly DbContext _context;
        private readonly ILogger<GradeService> _logger;

        public GradeService(DbContext context, ILogger<GradeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ActionResult<Grade>> AddGradeAsync(Grade grade)
        {
            var validationErrors = ValidateGrade(grade);
            if (validationErrors.Count > 0)
            {
                return new BadRequestObjectResult(new { Errors = validationErrors });
            }

            try
            {
                await _context.Set<Grade>().AddAsync(grade);
                await _context.SaveChangesAsync();
                return new ActionResult<Grade>(grade);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the grade for StudentId: {StudentId}, CourseId: {CourseId}", grade.StudentId, grade.CourseId);
                return new StatusCodeResult(500);
            }
        }

        private List<string> ValidateGrade(Grade grade)
        {
            var errors = new List<string>();
            if (grade == null)
            {
                errors.Add("Grade cannot be null.");
            }
            else
            {
                if (grade.StudentId <= 0)
                {
                    errors.Add("StudentId must be greater than zero.");
                }
                if (grade.CourseId <= 0)
                {
                    errors.Add("CourseId must be greater than zero.");
                }
                if (grade.Score < 0 || grade.Score > 100)
                {
                    errors.Add("Score must be between 0 and 100.");
                }
                if (grade.DateAssigned == default)
                {
                    errors.Add("DateAssigned must be a valid date.");
                }
            }
            return errors;
        }
    }

    public class Grade
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int Score { get; set; }
        public DateTime DateAssigned { get; set; }
    }
}