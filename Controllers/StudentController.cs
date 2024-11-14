namespace YourNamespace.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using YourNamespace.Services;
    using YourNamespace.Models;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Handles HTTP requests related to students and their grades.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// Retrieves the grades for a specified student.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <returns>A list of grades for the student.</returns>
        [HttpGet("{studentId}")]
        public ActionResult<IEnumerable<Grade>> GetGradesForStudent(int studentId)
        {
            if (studentId <= 0)
            {
                return BadRequest("The studentId must be provided and must be a valid positive integer.");
            }

            var gradesList = _studentService.GetGradesByStudentId(studentId);

            if (gradesList == null || !gradesList.Any())
            {
                return NotFound("No grades found for the specified studentId.");
            }

            return Ok(gradesList);
        }
    }
}