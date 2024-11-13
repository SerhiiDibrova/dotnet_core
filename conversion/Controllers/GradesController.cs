package conversion.Controllers;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class GradesController : ControllerBase
{
    private readonly IGradeService _gradeService;

    public GradesController(IGradeService gradeService)
    {
        _gradeService = gradeService;
    }

    [HttpGet("{studentId}")]
    public IActionResult GetGradesForStudent(int studentId)
    {
        if (studentId <= 0)
        {
            return BadRequest(new { error = "Invalid studentId." });
        }

        List<Grade> gradesList;
        try
        {
            gradesList = _gradeService.GetGradesForStudent(studentId);
        }
        catch (Exception)
        {
            return StatusCode(500, new { error = "An error occurred while retrieving grades." });
        }

        if (gradesList == null || gradesList.Count == 0)
        {
            return NotFound(new { error = "No grades found for the specified studentId." });
        }

        return Ok(gradesList.Select(g => new 
        {
            g.GradeId,
            g.StudentId,
            g.Subject,
            g.Score,
            g.Date
        }));
    }
}

public interface IGradeService
{
    List<Grade> GetGradesForStudent(int studentId);
}

public class Grade
{
    public int GradeId { get; set; }
    public int StudentId { get; set; }
    public string Subject { get; set; }
    public decimal Score { get; set; }
    public DateTime Date { get; set; }
}