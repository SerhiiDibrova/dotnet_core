namespace YourNamespace.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Grade
    {
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal Value { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateAssigned { get; set; }
    }
}

namespace YourNamespace.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using YourNamespace.Models;

    [ApiController]
    [Route("api/[controller]")]
    public class GradesController : ControllerBase
    {
        [HttpPost]
        public IActionResult AddGrade([FromBody] Grade grade)
        {
            if (grade == null)
            {
                return BadRequest("Grade object is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (grade.DateAssigned == default(DateTime) || grade.DateAssigned.Kind == DateTimeKind.Unspecified)
            {
                return BadRequest("DateAssigned is not a valid date.");
            }

            return CreatedAtAction(nameof(AddGrade), new { id = grade.Id }, grade);
        }
    }
}