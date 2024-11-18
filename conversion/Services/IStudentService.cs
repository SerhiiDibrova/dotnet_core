namespace Conversion.Services
{
    using System;
    using System.Threading.Tasks;
    using Conversion.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc;

    public interface IStudentService
    {
        Task<Student> CreateStudentAsync(Student student);
        bool ValidateStudent(Student student);
    }

    public class StudentService : IStudentService
    {
        private readonly DbContext _context;

        public StudentService(DbContext context)
        {
            _context = context;
        }

        public async Task<Student> CreateStudentAsync(Student student)
        {
            if (!ValidateStudent(student))
            {
                throw new ArgumentException("Invalid student data.");
            }

            try
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                return student;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while saving the student: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred: " + ex.Message);
            }
        }

        public bool ValidateStudent(Student student)
        {
            return !string.IsNullOrEmpty(student.Name) && student.Age > 0;
        }
    }
}