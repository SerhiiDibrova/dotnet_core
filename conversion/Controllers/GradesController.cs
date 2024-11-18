namespace Conversion.Controllers

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Conversion.Data;
using Conversion.Models;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class GradesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<GradesController> _logger;

    public GradesController(ApplicationDbContext context, ILogger<GradesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> AddGrade([FromBody] Grade grade)
    {
        if (grade == null)
        {
            return BadRequest("Grade object is null.");
        }

        var validationErrors = new List<string>();

        if (grade.StudentId == 0)
        {
            validationErrors.Add("StudentId is required.");
        }
        if (grade.CourseId == 0)
        {
            validationErrors.Add("CourseId is required.");
        }
        if (grade.Score < 0 || grade.Score > 100)
        {
            validationErrors.Add("Score must be between 0 and 100.");
        }
        if (grade.DateAssigned == default(DateTime))
        {
            validationErrors.Add("DateAssigned is required.");
        }
        if (grade.DateAssigned > DateTime.Now)
        {
            validationErrors.Add("DateAssigned must be a valid date in the past.");
        }

        if (validationErrors.Count > 0)
        {
            return BadRequest(string.Join(", ", validationErrors));
        }

        try
        {
            await _context.Grades.AddAsync(grade);
            await _context.SaveChangesAsync();
            return Ok(grade);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding the grade.");
            return StatusCode(500, "Internal server error.");
        }
    }
}