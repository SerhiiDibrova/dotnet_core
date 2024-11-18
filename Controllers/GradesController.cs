namespace YourNamespace.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AddGradeRequest
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int Score { get; set; }
        public DateTime DateAssigned { get; set; }
    }

    public interface IGradeService
    {
        Task<Grade> AddGradeAsync(AddGradeRequest request);
    }

    public class Grade
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int Score { get; set; }
        public DateTime DateAssigned { get; set; }
    }

    [ApiController]
    [Route("api/grades")]
    public class GradesController : ControllerBase
    {
        private readonly IGradeService _gradeService;

        public GradesController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        [HttpPost]
        public async Task<IActionResult> AddGrade([FromBody] AddGradeRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request body cannot be null.");
            }

            var validationErrors = new List<string>();

            if (request.StudentId <= 0)
            {
                validationErrors.Add("StudentId must be a positive integer.");
            }

            if (request.CourseId <= 0)
            {
                validationErrors.Add("CourseId must be a positive integer.");
            }

            if (request.Score < 0 || request.Score > 100)
            {
                validationErrors.Add("Score must be between 0 and 100.");
            }

            if (request.DateAssigned == default(DateTime) || request.DateAssigned > DateTime.Now)
            {
                validationErrors.Add("DateAssigned must be a valid date and cannot be in the future.");
            }

            if (validationErrors.Any())
            {
                return BadRequest(validationErrors);
            }

            try
            {
                var grade = await _gradeService.AddGradeAsync(request);
                return Ok(grade);
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}