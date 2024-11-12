package Services;

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

public interface IGradeService
{
    Task<Grade> AddGradeAsync(AddGradeDto gradeDto);
}

public class Grade
{
    public int StudentId { get; set; }
    public string Subject { get; set; }
    public decimal Score { get; set; }
    public DateTime Date { get; set; }
}

public class AddGradeDto
{
    public int StudentId { get; set; }
    public string Subject { get; set; }
    public decimal Score { get; set; }
    public string Date { get; set; }
}

public class GradeService : IGradeService
{
    public async Task<Grade> AddGradeAsync(AddGradeDto gradeDto)
    {
        var errors = new List<string>();

        if (gradeDto.StudentId <= 0)
        {
            errors.Add("Invalid student ID. It must be a valid integer.");
        }

        if (string.IsNullOrWhiteSpace(gradeDto.Subject))
        {
            errors.Add("Subject cannot be empty.");
        }

        if (gradeDto.Score < 0 || gradeDto.Score > 100)
        {
            errors.Add("Score must be between 0 and 100.");
        }

        if (string.IsNullOrWhiteSpace(gradeDto.Date) || !DateTime.TryParseExact(gradeDto.Date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime date))
        {
            errors.Add("Invalid date format. Use YYYY-MM-DD.");
        }

        if (errors.Count > 0)
        {
            throw new ArgumentException(string.Join(", ", errors));
        }

        var grade = new Grade
        {
            StudentId = gradeDto.StudentId,
            Subject = gradeDto.Subject,
            Score = gradeDto.Score,
            Date = date
        };

        await Task.CompletedTask;

        return grade;
    }
}