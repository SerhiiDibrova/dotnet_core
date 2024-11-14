package conversion.Models;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class Course {
    public int Id { get; set; }
    public string Title { get; set; }
    public List<Grade> Grades { get; set; }

    public Course() {
        Grades = new List<Grade>();
    }

    public void AddGrade(Grade grade, DbContext context) {
        if (grade != null && grade.CourseId == Id) {
            if (context.Set<Course>().Any(c => c.Id == grade.CourseId)) {
                Grades.Add(grade);
            } else {
                throw new System.ArgumentException("CourseId does not exist in the database.");
            }
        } else {
            throw new System.ArgumentException("Invalid grade or courseId does not match.");
        }
    }
}

public class Grade {
    [Required]
    public int CourseId { get; set; }
    [Range(0, 100)]
    public int Value { get; set; }
}