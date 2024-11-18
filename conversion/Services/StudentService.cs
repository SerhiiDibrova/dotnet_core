namespace Conversion.Services
{
    using System;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Http;

    public class StudentService : IStudentService
    {
        private readonly StudentContext _context;

        public StudentService(StudentContext context)
        {
            _context = context;
        }

        public async Task<IResult> CreateStudentAsync(Student student)
        {
            if (student == null)
                return Results.BadRequest("Student object is required.");

            if (string.IsNullOrWhiteSpace(student.FirstName))
                return Results.BadRequest("FirstName is required.");

            if (string.IsNullOrWhiteSpace(student.LastName))
                return Results.BadRequest("LastName is required.");

            if (string.IsNullOrWhiteSpace(student.Email))
                return Results.BadRequest("Email is required.");

            if (student.DateOfBirth == default)
                return Results.BadRequest("DateOfBirth is required.");

            if (student.EnrollmentDate == default)
                return Results.BadRequest("EnrollmentDate is required.");

            if (!IsValidEmail(student.Email))
                return Results.BadRequest("Email format is invalid.");

            if (await _context.Students.AnyAsync(s => s.Email.Equals(student.Email, StringComparison.OrdinalIgnoreCase)))
                return Results.Conflict("Email is already in use.");

            if (CalculateAge(student.DateOfBirth) < 18)
                return Results.BadRequest("Student must be at least 18 years old.");

            try
            {
                await _context.Students.AddAsync(student);
                await _context.SaveChangesAsync();
                return Results.Created($"/students/{student.Id}", student);
            }
            catch (DbUpdateException ex)
            {
                return Results.StatusCode((int)HttpStatusCode.InternalServerError, $"Database error: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        private bool IsValidEmail(string email)
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;
            return age;
        }
    }
}