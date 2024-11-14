namespace YourNamespace.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using YourNamespace.Models;
    using YourNamespace.Services;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class GradeController : ControllerBase
    {
        private readonly IGradeService _gradeService;

        public GradeController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        [HttpPost("AddGrade")]
        public async Task<IActionResult> AddGrade([FromBody] Grade grade)
        {
            if (grade == null)
            {
                return BadRequest("Grade object is null.");
            }

            if (string.IsNullOrEmpty(grade.StudentId))
            {
                return BadRequest("Student ID is required.");
            }

            if (string.IsNullOrEmpty(grade.CourseId))
            {
                return BadRequest("Course ID is required.");
            }

            if (!decimal.TryParse(grade.Value.ToString(), out var value) || value < 0 || value > 100)
            {
                return BadRequest("Value must be a numeric type between 0 and 100.");
            }

            if (string.IsNullOrEmpty(grade.DateAssigned) || !DateTime.TryParse(grade.DateAssigned, out _))
            {
                return BadRequest("DateAssigned must be a valid date and cannot be null.");
            }

            if (!await _gradeService.StudentExists(grade.StudentId))
            {
                return BadRequest("Student ID does not exist.");
            }

            if (!await _gradeService.CourseExists(grade.CourseId))
            {
                return BadRequest("Course ID does not exist.");
            }

            try
            {
                var addedGrade = await _gradeService.AddGradeAsync(grade);
                return Ok(addedGrade);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}