namespace Conversion.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public interface IStudentService
    {
        Task CreateStudentAsync(CreateStudentDto studentDto);
    }

    public class StudentService : IStudentService
    {
        private readonly DbContext _context;

        public StudentService(DbContext context)
        {
            _context = context;
        }

        public async Task CreateStudentAsync(CreateStudentDto studentDto)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(studentDto);

            if (!Validator.TryValidateObject(studentDto, validationContext, validationResults, true))
            {
                throw new ValidationException("Invalid student data");
            }

            var student = new Student
            {
                Name = studentDto.Name,
                Age = studentDto.Age,
                Email = studentDto.Email
            };

            await _context.Set<Student>().AddAsync(student);
            await _context.SaveChangesAsync();
        }
    }

    public class CreateStudentDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Range(1, 120)]
        public int Age { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
    }
}