namespace Conversion.Services
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Conversion.Data;
    using Conversion.Models;
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

        public async Task<(bool Success, string Message)> CreateStudentAsync(Student student)
        {
            if (student == null)
            {
                _logger.LogWarning("Student object is null.");
                return (false, "Student object cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(student.Email) || !IsValidEmail(student.Email))
            {
                _logger.LogWarning("Invalid email: {Email}", student.Email);
                return (false, "Invalid email format.");
            }

            if (await _context.Students.AnyAsync(s => s.Email == student.Email))
            {
                _logger.LogWarning("Email already exists: {Email}", student.Email);
                return (false, "Email already in use.");
            }

            if (!student.Age.HasValue || student.Age < 18 || student.Age > 100)
            {
                _logger.LogWarning("Invalid age: {Age}", student.Age);
                return (false, "Age must be between 18 and 100.");
            }

            try
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                return (true, "Student created successfully.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database update error occurred while creating student.");
                return (false, "An error occurred while creating the student.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating student.");
                return (false, "An unexpected error occurred.");
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}