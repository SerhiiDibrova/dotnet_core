```csharp
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using conversion.Models;

namespace conversion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetGradesForStudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;

        public GetGradesForStudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        }

        [HttpGet("{studentId}")]
        public IActionResult GetGradesForStudent(int studentId)
        {
            if (!int.TryParse(studentId.ToString(), out int parsedStudentId) || parsedStudentId <= 0)
            {
                return BadRequest("Invalid student ID");
            }

            try
            {
                var student = _studentRepository.GetStudent(parsedStudentId);
                if (student == null)
                {
                    return NotFound($"No student found with ID {parsedStudentId}");
                }

                var grades = _studentRepository.GetGradesForStudent(parsedStudentId);
                if (grades == null || grades.Count == 0)
                {
                    return NotFound($"No grades found for student with ID {parsedStudentId}");
                }
                grades.Sort((a, b) => a.CompareTo(b));
                return Ok(grades);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve grades: {ex.Message}");
            }
        }
    }
}
```