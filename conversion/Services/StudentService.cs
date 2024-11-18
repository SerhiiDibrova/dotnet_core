namespace Conversion.Services
{
    using System;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Conversion.Exceptions;
    using Conversion.Models;

    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateStudentAsync(StudentDto studentDto)
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

            try
            {
                var email = new MailAddress(studentDto.Email);
            }
            catch
            {
                throw new BadRequestException("Invalid email format.");
            }

            if (await _context.Students.AnyAsync(s => s.Email == studentDto.Email))
                throw new ConflictException("Email is already in use.");

            if (!DateTime.TryParse(studentDto.DateOfBirth, out var dateOfBirth))
                throw new BadRequestException("Invalid date of birth format.");

            if ((DateTime.Now.Year - dateOfBirth.Year) < 18 || 
                (DateTime.Now.Year - dateOfBirth.Year == 18 && DateTime.Now < dateOfBirth.AddYears(18)))
                throw new BadRequestException("Student must be at least 18 years old.");

            if (!DateTime.TryParse(studentDto.EnrollmentDate, out var enrollmentDate))
                throw new BadRequestException("Invalid enrollment date format.");

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
                await _context.Students.AddAsync(student);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new ConflictException($"An error occurred while saving the student: {ex.Message}");
            }
        }
    }
}