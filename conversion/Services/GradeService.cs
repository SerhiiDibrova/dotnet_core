```csharp
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using conversion.Models;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace conversion.Services
{
    public class GradeService : IGradeService
    {
        private readonly SchoolContext _context;
        private readonly ILogger<GradeService> _logger;

        public GradeService(SchoolContext context, ILogger<GradeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddGradeAsync(AddGradeModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var validationResults = ValidateModel(model);

            if (!validationResults.IsValid)
                throw new ArgumentException("Invalid input data.");

            try
            {
                var student = await _context.Students.FindAsync(model.StudentId);

                if (student == null)
                    throw new InvalidOperationException("Student not found.");

                var grade = new Grade
                {
                    StudentId = model.StudentId,
                    Subject = model.Subject,
                    Score = model.Score
                };

                _context.Grades.Add(grade);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error adding grade to database.");
                throw;
            }
        }

        private ValidationResult ValidateModel(AddGradeModel model)
        {
            var validationResults = new ValidationResult();

            if (model == null)
                validationResults.Errors.Add("Input data is required.");

            if (string.IsNullOrWhiteSpace(model.StudentId.ToString()))
                validationResults.Errors.Add("StudentId is required and must be greater than zero.");

            if (model.StudentId <= 0)
                validationResults.Errors.Add("StudentId is required and must be greater than zero.");

            if (string.IsNullOrWhiteSpace(model.Subject))
                validationResults.Errors.Add("Subject is required.");

            if (model.Score < 0 || model.Score > 100)
                validationResults.Errors.Add("Score must be between 0 and 100.");

            return validationResults;
        }
    }

    public interface IGradeService
    {
        Task AddGradeAsync(AddGradeModel model);
    }

    public class ValidationResult
    {
        public bool IsValid => Errors.Count == 0;

        public System.Collections.Generic.List<string> Errors { get; } = new System.Collections.Generic.List<string>();
    }
}
```