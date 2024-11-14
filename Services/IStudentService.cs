namespace YourNamespace.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;

    public interface IStudentService
    {
        Task<List<Grade>> GetGradesForStudentAsync(int studentId);
    }

    public class StudentService : IStudentService
    {
        private readonly List<Grade> _grades;

        public StudentService()
        {
            _grades = new List<Grade>
            {
                new Grade { Id = 1, Subject = "Math", Letter = "A", StudentId = 1 },
                new Grade { Id = 2, Subject = "Science", Letter = "B", StudentId = 1 },
                new Grade { Id = 3, Subject = "History", Letter = "A", StudentId = 2 }
            };
        }

        public async Task<List<Grade>> GetGradesForStudentAsync(int studentId)
        {
            var grades = _grades.Where(g => g.StudentId == studentId).ToList();
            return await Task.FromResult(grades);
        }
    }

    public class Grade
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Letter { get; set; }
        public int StudentId { get; set; }
    }
}