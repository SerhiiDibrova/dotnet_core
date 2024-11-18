namespace YourNamespace.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGradeService
    {
        Task<List<Grade>> GetGradesForStudentAsync(int studentId);
    }

    public class GradeService : IGradeService
    {
        private readonly List<Grade> _grades;

        public GradeService()
        {
            _grades = new List<Grade>
            {
                new Grade { StudentId = 1, Subject = "Math", Score = 90 },
                new Grade { StudentId = 1, Subject = "Science", Score = 85 },
                new Grade { StudentId = 2, Subject = "Math", Score = 78 }
            };
        }

        public async Task<List<Grade>> GetGradesForStudentAsync(int studentId)
        {
            return await Task.FromResult(_grades.FindAll(g => g.StudentId == studentId));
        }
    }

    public class Grade
    {
        public int StudentId { get; set; }
        public string Subject { get; set; }
        public int Score { get; set; }
    }
}