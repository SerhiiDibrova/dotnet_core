namespace Conversion.Services
{
    using System;
    using System.Threading.Tasks;
    using Conversion.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc;
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

        public async Task<Student> CreateStudentAsync(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            if (string.IsNullOrWhiteSpace(student.Name) || student.Age <= 0)
            {
                throw new BadRequestObjectResult("Invalid student data.");
            }

            try
            {
                await _context.Students.AddAsync(student);
                await _context.SaveChangesAsync();
                return student;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while saving the student.");
                throw new StatusCodeResult(500);
            }
        }
    }
}