package conversion.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Grade
{
    public int Id { get; set; }
    public string StudentName { get; set; }
    public string Subject { get; set; }
    public double Score { get; set; }
}

public interface IGradeService
{
    Task AddGradeAsync(Grade grade);
}

public class GradeService : IGradeService
{
    private readonly List<Grade> _grades = new List<Grade>();

    public async Task AddGradeAsync(Grade grade)
    {
        await Task.Run(() => 
        {
            if (grade == null)
            {
                throw new ArgumentNullException(nameof(grade));
            }
            if (string.IsNullOrWhiteSpace(grade.StudentName))
            {
                throw new ArgumentException("Student name cannot be null or empty.", nameof(grade.StudentName));
            }
            if (string.IsNullOrWhiteSpace(grade.Subject))
            {
                throw new ArgumentException("Subject cannot be null or empty.", nameof(grade.Subject));
            }
            if (grade.Score < 0 || grade.Score > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(grade.Score), "Score must be between 0 and 100.");
            }
            if (grade.Id == 0 || _grades.Any(g => g.Id == grade.Id))
            {
                throw new InvalidOperationException("Grade ID must be unique and greater than zero.");
            }
            _grades.Add(grade);
        });
    }
}