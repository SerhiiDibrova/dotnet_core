package Models;

using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

public class Grade
{
    public int Id { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    public string Subject { get; set; }

    [Required]
    public decimal Score { get; set; }

    [Required]
    public DateTime Date { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class GradesController : ControllerBase
{
    private readonly DbContext _context;

    public GradesController(DbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult AddGrade([FromBody] Grade grade)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Grades.Add(grade);
        _context.SaveChanges();

        return CreatedAtAction(nameof(AddGrade), new { id = grade.Id }, grade);
    }
}