namespace YourNamespace.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGradeService
    {
        Task<List<Grade>?> GetGradesForStudentAsync(int studentId);
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

        public async Task<List<Grade>?> GetGradesForStudentAsync(int studentId)
        {
            if (studentId <= 0)
            {
                throw new ArgumentException("Invalid student ID.");
            }

            try
            {
                return await Task.Run(() =>
                {
                    var gradesForStudent = _grades.FindAll(g => g.StudentId == studentId);
                    return gradesForStudent.Count > 0 ? gradesForStudent : null;
                });
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving grades.", ex);
            }
        }
    }

    public class Grade
    {
        public int StudentId { get; set; }
        public string Subject { get; set; }
        public int Score { get; set; }
    }
}