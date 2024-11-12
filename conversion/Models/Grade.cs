package conversion.Models;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

public class Grade {
    [Key]
    public int Id { get; set; }

    [ForeignKey("Student")]
    public int StudentId { get; set; }

    [Required]
    [StringLength(100)]
    public string Subject { get; set; }

    [Range(0, 100)]
    public double Score { get; set; }

    public virtual Student Student { get; set; }
}

public class GradeRepository {
    private List<Grade> grades;

    public GradeRepository(List<Grade> grades) {
        this.grades = grades;
    }

    public List<Grade> GetStudentWithGrades(int studentId) {
        return grades.Where(g => g.StudentId == studentId).ToList();
    }
}