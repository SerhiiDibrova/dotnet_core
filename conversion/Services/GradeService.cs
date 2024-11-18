namespace Conversion.Services

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Conversion.Data;
using Conversion.Models;

public class GradeService
{
    private readonly ApplicationDbContext _context;

    public GradeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IResult, object)> AddGradeAsync(AddGradeRequest request)
    {
        if (request == null)
        {
            return Results.BadRequest(new { Error = "Request cannot be null." });
        }

        var validationErrors = new List<string>();

        if (request.StudentId <= 0)
        {
            validationErrors.Add("StudentId must be a positive integer.");
        }
        if (request.CourseId <= 0)
        {
            validationErrors.Add("CourseId must be a positive integer.");
        }
        if (request.Score < 0 || request.Score > 100)
        {
            validationErrors.Add("Score must be between 0 and 100.");
        }
        if (request.DateAssigned == default(DateTime) || request.DateAssigned > DateTime.Now)
        {
            validationErrors.Add("DateAssigned must be a valid date and cannot be in the future.");
        }

        if (validationErrors.Count > 0)
        {
            return Results.BadRequest(new { Errors = validationErrors });
        }

        var grade = new Grade
        {
            StudentId = request.StudentId,
            CourseId = request.CourseId,
            Score = request.Score,
            DateAssigned = request.DateAssigned
        };

        try
        {
            await _context.Grades.AddAsync(grade);
            await _context.SaveChangesAsync();
            return Results.Ok(new { Message = "Grade added successfully." });
        }
        catch (DbUpdateException ex)
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError, new { Error = $"An error occurred while saving the grade: {ex.InnerException?.Message ?? ex.Message}" });
        }
    }
}