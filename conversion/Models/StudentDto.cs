namespace Conversion.Models
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<GradeDto> Grades { get; set; } = new List<GradeDto>();
    }

    public class GradeDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string Subject { get; set; }
        public decimal Score { get; set; }
    }
}