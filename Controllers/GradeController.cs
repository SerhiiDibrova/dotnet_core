namespace YourNamespace.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using YourNamespace.Models;
    using YourNamespace.Services;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GradeController : ControllerBase
    {
        private readonly IGradeService _gradeService;
        private readonly ILogger<GradeController> _logger;

        public GradeController(IGradeService gradeService, ILogger<GradeController> logger)
        {
            _gradeService = gradeService;
            _logger = logger;
        }

        [HttpPost("AddGrade")]
        public async Task<IActionResult> AddGrade([FromBody] Grade grade)
        {
            if (grade == null)
            {
                return BadRequest("Grade data is required.");
            }

            if (string.IsNullOrEmpty(grade.StudentId))
            {
                return BadRequest("StudentId is required.");
            }

            if (string.IsNullOrEmpty(grade.CourseId))
            {
                return BadRequest("CourseId is required.");
            }

            if (!decimal.TryParse(grade.Value.ToString(), out var value) || value < 0 || value > 100)
            {
                return BadRequest("Value must be a numeric type between 0 and 100.");
            }

            if (string.IsNullOrEmpty(grade.DateAssigned) || !DateTime.TryParse(grade.DateAssigned, out var dateAssigned) || dateAssigned.Kind != DateTimeKind.Utc)
            {
                return BadRequest("DateAssigned must be a valid date in ISO 8601 format.");
            }

            var studentExists = await _gradeService.StudentExists(grade.StudentId);
            var courseExists = await _gradeService.CourseExists(grade.CourseId);

            if (!studentExists || !courseExists)
            {
                return BadRequest("Student or Course does not exist.");
            }

            try
            {
                await _gradeService.AddGradeAsync(grade);
                return Ok(grade);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the grade.");
                return StatusCode(500, "An error occurred while adding the grade.");
            }
        }
    }
}