namespace YourNamespace.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using YourNamespace.Models;
    using YourNamespace.Services;
    using System.Text.RegularExpressions;

    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] Student student)
        {
            if (student == null)
            {
                return BadRequest("Student data is required.");
            }

            if (string.IsNullOrWhiteSpace(student.FirstName))
            {
                return BadRequest("First name is required.");
            }

            if (string.IsNullOrWhiteSpace(student.LastName))
            {
                return BadRequest("Last name is required.");
            }

            if (string.IsNullOrWhiteSpace(student.Email))
            {
                return BadRequest("Email is required.");
            }

            if (student.DateOfBirth == default(DateTime))
            {
                return BadRequest("Date of birth is required.");
            }

            if (student.EnrollmentDate == default(DateTime))
            {
                return BadRequest("Enrollment date is required.");
            }

            if (student.DateOfBirth > DateTime.Now || student.EnrollmentDate > DateTime.Now)
            {
                return BadRequest("Date of birth and enrollment date must be in the past.");
            }

            if (!IsValidEmail(student.Email))
            {
                return BadRequest("Invalid email format.");
            }

            if (await _studentService.EmailExistsAsync(student.Email))
            {
                return Conflict("Email already exists.");
            }

            if (!IsAgeValid(student.DateOfBirth))
            {
                return BadRequest("Student must be at least 18 years old.");
            }

            try
            {
                var createdStudent = await _studentService.CreateStudentAsync(student);
                return Ok(createdStudent);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        private bool IsValidEmail(string email)
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        private bool IsAgeValid(DateTime dateOfBirth)
        {
            var age = DateTime.Now.Year - dateOfBirth.Year;
            if (dateOfBirth > DateTime.Now.AddYears(-age)) age--;
            return age >= 18;
        }
    }
}