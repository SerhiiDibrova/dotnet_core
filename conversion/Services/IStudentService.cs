namespace Conversion.Services
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Conversion.Models;

    public interface IStudentService
    {
        Task<Student> CreateStudentAsync(Student student);
    }

    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Student> CreateStudentAsync(Student student)
        {
            if (student == null)
            {
                throw new ArgumentException("Student data is required.");
            }

            if (string.IsNullOrWhiteSpace(student.Email) || string.IsNullOrWhiteSpace(student.Name) || student.DateOfBirth == default)
            {
                throw new ArgumentException("All fields are required.");
            }

            if (await _context.Students.AnyAsync(s => s.Email == student.Email))
            {
                throw new InvalidOperationException("Email is already in use.");
            }

            var age = DateTime.Now.Year - student.DateOfBirth.Year;
            if (student.DateOfBirth > DateTime.Now.AddYears(-age)) age--;

            if (age < 18)
            {
                throw new ArgumentException("Student must be at least 18 years old.");
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }
    }
}