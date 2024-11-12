using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GradesController : ControllerBase
    {
        private readonly IGradeService _gradeService;
        private readonly ILogger<GradesController> _logger;

        public GradesController(IGradeService gradeService, ILogger<GradesController> logger)
        {
            _gradeService = gradeService;
            _logger = logger;
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetGradesForStudent(int studentId)
        {
            if (studentId <= 0)
            {
                _logger.LogWarning("Invalid studentId: {studentId}", studentId);
                return BadRequest(new { error = "studentId must be a valid integer." });
            }

            try
            {
                var grades = await _gradeService.GetGradesByStudentIdAsync(studentId);

                if (grades == null || !grades.Any())
                {
                    _logger.LogInformation("No grades found for studentId: {studentId}", studentId);
                    return NotFound(new { error = "No grades found for the specified student." });
                }

                _logger.LogInformation("Retrieved {count} grades for studentId: {studentId}", grades.Count(), studentId);
                return Ok(grades);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving grades for studentId: {studentId}", studentId);
                return StatusCode(500, new { error = "An error occurred while processing your request." });
            }
        }
    }
}