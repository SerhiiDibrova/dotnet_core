namespace Conversion.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc;
    using System;

    public class ApplicationDbContext : DbContext
    {
        public DbSet<Grade> Grades { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public IActionResult AddGrade(Grade grade)
        {
            if (grade == null)
            {
                return new BadRequestObjectResult(new { message = "Grade cannot be null." });
            }

            try
            {
                Grades.Add(grade);
                SaveChanges();
                return new OkResult();
            }
            catch (DbUpdateException ex)
            {
                return new ObjectResult(new { message = "Database update error: " + ex.Message }) { StatusCode = 500 };
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { message = "An error occurred: " + ex.Message }) { StatusCode = 500 };
            }
        }
    }

    public class Grade
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
    }
}