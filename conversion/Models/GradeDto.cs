namespace Conversion.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class GradeDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Subject { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal Score { get; set; }
    }

    public class GradesResponse
    {
        [Required]
        public List<GradeDto> Grades { get; set; }

        public GradesResponse()
        {
            Grades = new List<GradeDto>();
        }

        public void AddGrade(GradeDto grade)
        {
            if (grade != null)
            {
                Grades.Add(grade);
            }
        }
    }
}