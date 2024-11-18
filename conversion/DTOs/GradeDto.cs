namespace Conversion.DTOs
{
    public class GradeDto
    {
        public int GradeId { get; set; }
        public int StudentId { get; set; }
        public string Subject { get; set; }
        public decimal Score { get; set; }
        public DateTime DateAwarded { get; set; }
    }
}

namespace Conversion.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class GradesController : ControllerBase
    {
        [HttpGet("GetGradesForStudent/{studentId}")]
        public async Task<IActionResult> GetGradesForStudent(int studentId)
        {
            var grades = new List<GradeDto>
            {
                new GradeDto { GradeId = 1, StudentId = studentId, Subject = "Math", Score = 95.5m, DateAwarded = DateTime.Now },
                new GradeDto { GradeId = 2, StudentId = studentId, Subject = "Science", Score = 88.0m, DateAwarded = DateTime.Now }
            };

            return Ok(grades);
        }
    }
}