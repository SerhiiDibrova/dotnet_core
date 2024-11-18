namespace Conversion.Services
{
    using System;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Conversion.Data;
    using Conversion.Models;
    using Conversion.Exceptions;
    using Microsoft.Extensions.Logging;

    public class StudentService : IStudentService
    {
        private readonly StudentContext _context;
        private readonly ILogger<StudentService> _logger;

        public StudentService(StudentContext context, ILogger<StudentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Student> CreateStudentAsync(StudentDto studentDto)
        {
            if (string.IsNullOrWhiteSpace(studentDto.FirstName))
                throw new BadRequestException("First name is required.");
            if (string.IsNullOrWhiteSpace(studentDto.LastName))
                throw new BadRequestException("Last name is required.");
            if (string.IsNullOrWhiteSpace(studentDto.Email))
                throw new BadRequestException("Email is required.");
            if (string.IsNullOrWhiteSpace(studentDto.DateOfBirth))
                throw new BadRequestException("Date of birth is required.");
            if (string.IsNullOrWhiteSpace(studentDto.EnrollmentDate))
                throw new BadRequestException("Enrollment date is required.");

            if (!IsValidEmail(studentDto.Email))
                throw new BadRequestException("Invalid email format.");

            if (await _context.Students.AnyAsync(s => s.Email == studentDto.Email))
                throw new ConflictException("Email is already in use.");

            if (!DateTime.TryParse(studentDto.DateOfBirth, out var dateOfBirth) || dateOfBirth > DateTime.Now)
                throw new BadRequestException("Invalid date of birth.");

            if (DateTime.Now.Year - dateOfBirth.Year < 18 || (DateTime.Now.Year - dateOfBirth.Year == 18 && DateTime.Now < dateOfBirth.AddYears(18)))
                throw new BadRequestException("Student must be at least 18 years old.");

            if (!DateTime.TryParse(studentDto.EnrollmentDate, out var enrollmentDate) || enrollmentDate > DateTime.Now || enrollmentDate < dateOfBirth)
                throw new BadRequestException("Invalid enrollment date.");

            var student = new Student
            {
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                Email = studentDto.Email,
                DateOfBirth = dateOfBirth,
                EnrollmentDate = enrollmentDate
            };

            try
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating student.");
                throw;
            }

            return student;
        }

        private bool IsValidEmail(string email)
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }
    }
}