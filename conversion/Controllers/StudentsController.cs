namespace Conversion.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Conversion.Models;
    using Conversion.Services;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] Student student)
        {
            if (student == null)
            {
                return BadRequest("Student data cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(student.Email) || !IsValidEmail(student.Email))
            {
                return BadRequest("Invalid email format.");
            }

            if (student.Age < 18)
            {
                return BadRequest("Student must be at least 18 years old.");
            }

            if (string.IsNullOrWhiteSpace(student.Name))
            {
                return BadRequest("Student name cannot be empty.");
            }

            try
            {
                var existingStudent = await _studentService.GetStudentByEmailAsync(student.Email);
                if (existingStudent != null)
                {
                    return Conflict("Email already exists.");
                }

                var createdStudent = await _studentService.CreateStudentAsync(student);
                return Ok(createdStudent);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating a student with email: {Email}", student.Email);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        private bool IsValidEmail(string email)
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }
    }
}