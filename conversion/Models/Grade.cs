package conversion.Models;

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class Grade
{
    public int GradeId { get; set; }
    public int StudentId { get; set; }
    public string Subject { get; set; }
    public decimal Score { get; set; }
    public DateTime Date { get; set; }
}

public interface IGradeService
{
    IEnumerable<Grade> GetGradesByStudentId(int studentId);
}

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
    public ActionResult<IEnumerable<Grade>> GetGradesForStudent(int studentId)
    {
        if (studentId <= 0)
        {
            return BadRequest("Invalid student ID.");
        }

        var grades = _gradeService.GetGradesByStudentId(studentId);
        if (grades == null || !grades.Any())
        {
            return NotFound();
        }
        return Ok(grades);
    }
}