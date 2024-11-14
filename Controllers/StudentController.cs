using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Controllers
{
    [ApiController]
    [Route("api/students")]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentController> _logger;

        public StudentController(IStudentService studentService, ILogger<StudentController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "The id parameter is required and must be a valid integer." });
            }

            try
            {
                var student = await _studentService.FindStudentByIdAsync(id);
                if (student == null)
                {
                    return NotFound(new { message = $"No student exists with the specified id: {id}." });
                }

                var grades = await _studentService.GetStudentGradesAsync(id);
                return Ok(new
                {
                    Student = new
                    {
                        Id = student.Id,
                        Name = student.Name,
                        Email = student.Email,
                        DateOfBirth = student.DateOfBirth,
                        Address = student.Address,
                        PhoneNumber = student.PhoneNumber,
                        EnrollmentDate = student.EnrollmentDate
                    },
                    Grades = grades
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving student data.");
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }
}