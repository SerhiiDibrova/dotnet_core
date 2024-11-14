namespace Conversion.Services
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Conversion.Data;
    using Conversion.Models;
    using Microsoft.Extensions.Logging;

    public class StudentService : IStudentService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<StudentService> _logger;

        public StudentService(AppDbContext context, ILogger<StudentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> AddGradeAsync(int studentId, int courseId, decimal value, string dateAssigned)
        {
            if (studentId <= 0)
            {
                return new BadRequestObjectResult("Invalid student ID.");
            }

            if (courseId <= 0)
            {
                return new BadRequestObjectResult("Invalid course ID.");
            }

            if (value < 0 || value > 100)
            {
                return new BadRequestObjectResult("Grade value must be between 0 and 100.");
            }

            if (!DateTime.TryParse(dateAssigned, out DateTime parsedDate) || !IsValidDate(parsedDate))
            {
                return new BadRequestObjectResult("Invalid date format or date is in the future.");
            }

            var studentExists = await _context.Students.AnyAsync(s => s.Id == studentId);
            var courseExists = await _context.Courses.AnyAsync(c => c.Id == courseId);

            if (!studentExists)
            {
                return new BadRequestObjectResult("Student does not exist.");
            }

            if (!courseExists)
            {
                return new BadRequestObjectResult("Course does not exist.");
            }

            var grade = new Grade
            {
                StudentId = studentId,
                CourseId = courseId,
                Value = value,
                DateAssigned = parsedDate
            };

            try
            {
                await _context.Grades.AddAsync(grade);
                await _context.SaveChangesAsync();
                return new OkObjectResult(grade);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the grade for Student ID: {StudentId}, Course ID: {CourseId}.", studentId, courseId);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        private bool IsValidDate(DateTime date)
        {
            return date != default && date <= DateTime.Now;
        }
    }
}