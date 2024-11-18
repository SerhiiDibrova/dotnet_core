namespace YourNamespace.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGradeService
    {
        Task<StudentWithGradesDto> GetStudentWithGradesAsync(int id);
        Task<List<GradeDto>> GetGradesByStudentIdAsync(int studentId);
        Task<bool> AddGradeAsync(int studentId, GradeDto grade);
        Task<bool> UpdateGradeAsync(int studentId, GradeDto grade);
        Task<bool> DeleteGradeAsync(int studentId, int subjectId);
    }

    public class StudentWithGradesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GradeDto> Grades { get; set; }
    }

    public class GradeDto
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public decimal Score { get; set; }
    }
}