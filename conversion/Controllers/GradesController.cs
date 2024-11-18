namespace Conversion.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using Conversion.Services;
    using Conversion.Models;
    using Microsoft.Extensions.Logging;

    public class GradeDto
    {
        public int? Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public decimal Score { get; set; }
        public DateTime DateAssigned { get; set; }
    }

    [Route("api/grades")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly IGradeService _gradeService;
        private readonly ILogger<GradesController> _logger;

        public GradesController(IGradeService gradeService, ILogger<GradesController> logger)
        {
            _gradeService = gradeService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddGradeAsync([FromBody] GradeDto grade)
        {
            if (grade == null || grade.StudentId <= 0 || grade.CourseId <= 0 || grade.Score < 0 || grade.Score > 100 || grade.DateAssigned == default)
            {
                return BadRequest("Invalid grade data.");
            }

            try
            {
                var addedGrade = await _gradeService.AddGradeAsync(grade);
                return Ok(addedGrade);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the grade.");
                return StatusCode(500, "An error occurred while adding the grade.");
            }
        }
    }
}