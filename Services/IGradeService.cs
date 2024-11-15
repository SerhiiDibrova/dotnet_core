namespace YourNamespace.Services
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using YourNamespace.Models;

    public interface IGradeService
    {
        Task<Grade> AddGradeAsync(Grade grade);
    }

    public class GradeService : IGradeService
    {
        private readonly YourDbContext _context;

        public GradeService(YourDbContext context)
        {
            _context = context;
        }

        public async Task<Grade> AddGradeAsync(Grade grade)
        {
            await _context.Grades.AddAsync(grade);
            await _context.SaveChangesAsync();
            return grade;
        }

        [HttpPost]
        public async Task<IActionResult> AddGrade(Grade grade)
        {
            if (ModelState.IsValid)
            {
                await AddGradeAsync(grade);
                return CreatedAtAction(nameof(AddGrade), new { id = grade.Id }, grade);
            }
            return BadRequest(ModelState);
        }
    }
}