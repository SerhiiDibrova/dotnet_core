namespace Conversion.DTOs
{
    public class AddGradeResponse
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public decimal Score { get; set; }
        public DateTime DateAssigned { get; set; }
    }

    public class AddGradeRequest
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public decimal Score { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Conversion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GradeController : ControllerBase
    {
        private readonly IGradeService _gradeService;

        public GradeController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        [HttpPost]
        public async Task<IActionResult> AddGrade([FromBody] AddGradeRequest request)
        {
            if (request == null || request.StudentId <= 0 || request.CourseId <= 0 || request.Score < 0)
            {
                return BadRequest("Invalid grade data.");
            }

            var grade = await _gradeService.AddGradeAsync(request);

            if (grade == null)
            {
                return StatusCode(500, "An error occurred while adding the grade.");
            }

            return Ok(grade);
        }
    }

    public interface IGradeService
    {
        Task<AddGradeResponse> AddGradeAsync(AddGradeRequest request);
    }
}