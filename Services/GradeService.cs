namespace YourNamespace.Services
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using YourNamespace.Models;
    using YourNamespace.Data;

    public interface IGradeService
    {
        IActionResult AddGrade(Grade grade);
    }

    public class GradeService : IGradeService
    {
        private readonly YourDbContext _context;

        public GradeService(YourDbContext context)
        {
            _context = context;
        }

        public IActionResult AddGrade(Grade grade)
        {
            if (grade == null)
            {
                return new BadRequestObjectResult("Grade cannot be null.");
            }

            if (string.IsNullOrEmpty(grade.StudentId) || grade.Score < 0)
            {
                return new BadRequestObjectResult("Invalid grade data.");
            }

            try
            {
                _context.Grades.Add(grade);
                _context.SaveChanges();
                return new OkResult();
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}