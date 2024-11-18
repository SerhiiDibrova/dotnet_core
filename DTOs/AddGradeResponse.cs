```csharp
namespace YourNamespace.DTOs
{
    using System;

    public class AddGradeResponse
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public decimal Score { get; set; }
        public DateTime DateAssigned { get; set; }
    }
}

namespace YourNamespace.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using YourNamespace.DTOs;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class GradesController : ControllerBase
    {
        private readonly IGradeService _gradeService;

        public GradesController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        [HttpPost]
        public async Task<ActionResult<AddGradeResponse>> AddGrade(int studentId, int courseId, decimal score)
        {
            if (studentId <= 0 || courseId <= 0 || score < 0)
            {
                return BadRequest("Invalid input parameters.");
            }

            try
            {
                var grade = await _gradeService.AddGradeAsync(studentId, courseId, score);
                return CreatedAtAction(nameof(AddGrade), new { id = grade.Id }, grade);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

namespace YourNamespace.Services
{
    using YourNamespace.DTOs;
    using System.Threading.Tasks;

    public interface IGradeService
    {
        Task<AddGradeResponse> AddGradeAsync(int studentId, int courseId, decimal score);
    }

    public class GradeService : IGradeService
    {
        private static int _nextId = 1;

        public async Task<AddGradeResponse> AddGradeAsync(int studentId, int courseId, decimal score)
        {
            var grade = new AddGradeResponse
            {
                Id = GenerateGradeId(),
                StudentId = studentId,
                CourseId = courseId,
                Score = score,
                DateAssigned = DateTime.UtcNow
            };

            // Save grade to the database here

            return grade;
        }

        private int GenerateGradeId()
        {
            return _nextId++;
        }
    }
}
```