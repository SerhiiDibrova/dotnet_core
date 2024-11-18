namespace YourNamespace.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using YourNamespace.Data;
    using YourNamespace.Models;
    using Microsoft.Extensions.Logging;

    [Route("api/[controller]")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly YourDbContext _context;
        private readonly ILogger<GradesController> _logger;

        public GradesController(YourDbContext context, ILogger<GradesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddGrade([FromBody] GradeDto gradeDto)
        {
            if (gradeDto == null)
            {
                return BadRequest("Grade data is required.");
            }

            if (gradeDto.StudentId <= 0)
            {
                return BadRequest("StudentId must be a positive integer.");
            }

            if (gradeDto.CourseId <= 0)
            {
                return BadRequest("CourseId must be a positive integer.");
            }

            if (gradeDto.Score < 0 || gradeDto.Score > 100)
            {
                return BadRequest("Score must be between 0 and 100.");
            }

            if (!DateTime.TryParse(gradeDto.DateAssigned.ToString(), out var dateAssigned))
            {
                return BadRequest("DateAssigned must be a valid date.");
            }

            var grade = new Grade
            {
                StudentId = gradeDto.StudentId,
                CourseId = gradeDto.CourseId,
                Score = gradeDto.Score,
                DateAssigned = dateAssigned
            };

            try
            {
                await _context.Grades.AddAsync(grade);
                await _context.SaveChangesAsync();
                return Ok(new { grade.Id, grade.StudentId, grade.CourseId, grade.Score, grade.DateAssigned });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the grade.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}