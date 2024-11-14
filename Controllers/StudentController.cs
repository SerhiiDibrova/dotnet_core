namespace YourNamespace.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using YourNamespace.Services;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentController> _logger;

        public StudentController(IStudentService studentService, ILogger<StudentController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetGradesForStudent(int? studentId)
        {
            if (!studentId.HasValue || studentId <= 0)
            {
                return BadRequest(new { error = "Invalid studentId." });
            }

            try
            {
                var gradesList = await _studentService.GetGradesForStudentAsync(studentId.Value);

                if (gradesList == null || gradesList.Count == 0)
                {
                    return NotFound(new { error = "No grades found for the specified studentId." });
                }

                return Ok(gradesList);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving grades for studentId {StudentId}", studentId);
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }
    }
}