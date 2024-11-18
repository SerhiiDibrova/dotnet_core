namespace Conversion.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Conversion.Models;
    using Microsoft.AspNetCore.Http;

    public interface IGradeService
    {
        Task<Grade> AddGradeAsync(Grade grade, HttpContext httpContext);
    }

    public class GradeService : IGradeService
    {
        private readonly IGradeRepository _gradeRepository;

        public GradeService(IGradeRepository gradeRepository)
        {
            _gradeRepository = gradeRepository;
        }

        public async Task<Grade> AddGradeAsync(Grade grade, HttpContext httpContext)
        {
            var validationErrors = ValidateGrade(grade);

            if (validationErrors.Count > 0)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsync(string.Join(", ", validationErrors));
                return null;
            }

            return await _gradeRepository.AddAsync(grade);
        }

        private List<string> ValidateGrade(Grade grade)
        {
            var validationErrors = new List<string>();

            if (grade == null)
            {
                validationErrors.Add("Grade cannot be null.");
            }
            else
            {
                if (string.IsNullOrEmpty(grade.StudentId))
                {
                    validationErrors.Add("StudentId is required.");
                }
                if (string.IsNullOrEmpty(grade.CourseId))
                {
                    validationErrors.Add("CourseId is required.");
                }
                if (grade.Score < 0 || grade.Score > 100)
                {
                    validationErrors.Add("Score must be between 0 and 100.");
                }
                if (!DateTime.TryParseExact(grade.DateAssigned.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    validationErrors.Add("DateAssigned must be a valid date in the format yyyy-MM-dd.");
                }
            }

            return validationErrors;
        }
    }
}