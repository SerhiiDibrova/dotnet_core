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
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;

        public GradeController(IGradeService gradeService, IStudentService studentService, ICourseService courseService)
        {
            _gradeService = gradeService;
            _studentService = studentService;
            _courseService = courseService;
        }

        [HttpPost]
        public IActionResult AddGrade([FromBody] Grade grade)
        {
            if (grade == null)
            {
                return BadRequest("Grade object is null.");
            }

            if (grade.StudentId == null)
            {
                return BadRequest("StudentId is required.");
            }

            if (grade.CourseId == null)
            {
                return BadRequest("CourseId is required.");
            }

            if (!decimal.TryParse(grade.Value.ToString(), out var value) || value < 0 || value > 100)
            {
                return BadRequest("Value must be a numeric type between 0 and 100.");
            }

            if (grade.DateAssigned == null || !DateTime.TryParse(grade.DateAssigned.ToString(), out _))
            {
                return BadRequest("DateAssigned is required and must be in a valid date format.");
            }

            if (!_studentService.StudentExists(grade.StudentId).Result)
            {
                return BadRequest("Student does not exist.");
            }

            if (!_courseService.CourseExists(grade.CourseId).Result)
            {
                return BadRequest("Course does not exist.");
            }

            try
            {
                _gradeService.AddGrade(grade);
                return Ok(grade);
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}