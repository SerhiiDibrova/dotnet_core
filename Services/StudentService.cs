package Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

public class StudentService
{
    private readonly DbContext _context;
    private readonly ILogger<StudentService> _logger;

    public StudentService(DbContext context, ILogger<StudentService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ActionResult<StudentWithGradesDto>> GetStudentWithGradesAsync(int id)
    {
        if (id <= 0)
        {
            _logger.LogError("Invalid student ID provided: {Id}", id);
            return new BadRequestResult();
        }

        try
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return new NotFoundResult();
            }

            var gradesList = await _context.Grades.Where(g => g.StudentId == id).ToListAsync();
            return new StudentWithGradesDto
            {
                StudentId = student.Id,
                StudentName = student.Name,
                Grades = gradesList.Select(g => new GradeDto
                {
                    GradeId = g.Id,
                    Value = g.Value
                }).ToList()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while retrieving student with grades for ID: {Id}", id);
            return new StatusCodeResult(500);
        }
    }
}

public class StudentWithGradesDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; }
    public List<GradeDto> Grades { get; set; }
}

public class GradeDto
{
    public int GradeId { get; set; }
    public string Value { get; set; }
}