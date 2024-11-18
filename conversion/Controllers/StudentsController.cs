namespace Conversion.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using Conversion.Services;
    using Conversion.Models;
    using System.ComponentModel.DataAnnotations;
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
            if (!ModelState.IsValid || IsUnderage(student.DateOfBirth))
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdStudent = await _studentService.CreateStudentAsync(student);
                return Ok(createdStudent);
            }
            catch (EmailAlreadyExistsException)
            {
                return Conflict("The email address is already in use.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating a student.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        private bool IsUnderage(DateTime dateOfBirth)
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth > DateTime.Today.AddYears(-age)) age--;
            return age < 18;
        }
    }

    public class Student
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    }
}