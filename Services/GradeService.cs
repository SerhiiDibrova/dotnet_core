package Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public interface IGradeService
{
    Task<AddGradeResponse> AddGradeAsync(AddGradeRequest request);
}

public class AddGradeRequest
{
    public int StudentId { get; set; }
    public string Subject { get; set; }
    public decimal Score { get; set; }
    public string Date { get; set; }
}

public class AddGradeResponse
{
    public Grade AddedGrade { get; set; }
    public List<string> ValidationErrors { get; set; }
}

public class Grade
{
    public int StudentId { get; set; }
    public string Subject { get; set; }
    public decimal Score { get; set; }
    public DateTime Date { get; set; }
}

public class GradeService : IGradeService
{
    private readonly YourDbContext _context;

    public GradeService(YourDbContext context)
    {
        _context = context;
    }

    public async Task<AddGradeResponse> AddGradeAsync(AddGradeRequest request)
    {
        var validationErrors = new List<string>();

        if (request.StudentId <= 0)
        {
            validationErrors.Add("Invalid StudentId.");
        }
        if (string.IsNullOrWhiteSpace(request.Subject))
        {
            validationErrors.Add("Subject cannot be empty.");
        }
        if (request.Score < 0 || request.Score > 100)
        {
            validationErrors.Add("Score must be between 0 and 100.");
        }
        if (!DateTime.TryParseExact(request.Date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var date) || date > DateTime.Now)
        {
            validationErrors.Add("Invalid Date format or date is in the future. Use YYYY-MM-DD.");
        }

        if (validationErrors.Count > 0)
        {
            return new AddGradeResponse { AddedGrade = null, ValidationErrors = validationErrors };
        }

        var grade = new Grade
        {
            StudentId = request.StudentId,
            Subject = request.Subject,
            Score = request.Score,
            Date = date
        };

        try
        {
            _context.Set<Grade>().Add(grade);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            throw new Exception("An error occurred while saving the grade.");
        }

        return new AddGradeResponse { AddedGrade = grade, ValidationErrors = null };
    }
}