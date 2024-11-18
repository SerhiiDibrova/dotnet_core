namespace YourProject.DTOs
{
    using System;

    public class GradeDto
    {
        public int GradeId { get; set; }
        public int StudentId { get; set; }
        public string Subject { get; set; }
        public decimal Score { get; set; }
        public DateTime DateAwarded { get; set; }
    }
}