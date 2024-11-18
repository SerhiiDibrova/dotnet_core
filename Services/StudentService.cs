namespace YourNamespace.Services
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using YourNamespace.Data;
    using YourNamespace.Models;
    using YourNamespace.Interfaces;
    using Microsoft.Extensions.Logging;

    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StudentService> _logger;

        public StudentService(ApplicationDbContext context, ILogger<StudentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> CreateStudentAsync(Student student)
        {
            if (student == null)
            {
                return new BadRequestObjectResult("Student object cannot be null.");
            }

            if (string.IsNullOrEmpty(student.Email) || await _context.Students.AnyAsync(s => s.Email == student.Email))
            {
                return new BadRequestObjectResult("Email must be unique.");
            }

            if (student.Age == null || student.Age < 18)
            {
                return new BadRequestObjectResult("Student must be at least 18 years old.");
            }

            try
            {
                await _context.Students.AddAsync(student);
                await _context.SaveChangesAsync();
                return new OkObjectResult(student);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while saving the student.");
                return new StatusCodeResult(500);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while saving the student.");
                return new StatusCodeResult(500);
            }
        }
    }
}