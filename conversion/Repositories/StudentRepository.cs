```csharp
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using conversion.Models;

namespace conversion.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly SchoolContext _context;

        public StudentRepository(SchoolContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddGrade(int studentId, Grade grade)
        {
            if (grade == null) 
                throw new ArgumentNullException(nameof(grade), "Grade cannot be null.");

            if (string.IsNullOrWhiteSpace(grade.Subject)) 
                throw new ArgumentException("Subject is required.", nameof(grade.Subject));

            if (grade.Value < 0 || grade.Value > 100) 
                throw new ArgumentOutOfRangeException(nameof(grade.Value), "Value must be between 0 and 100.");

            var student = _context.Students
                .Include(s => s.Grades)
                .FirstOrDefault(s => s.Id == studentId);

            if (student == null) 
                throw new InvalidOperationException($"Student with id {studentId} does not exist.", nameof(studentId));

            if (student.Grades == null) 
                student.Grades = new List<Grade>();

            var existingGrade = student.Grades.FirstOrDefault(g => g.Subject == grade.Subject);

            if (existingGrade != null)
            {
                throw new InvalidOperationException($"A grade for subject '{grade.Subject}' already exists for the student.");
            }

            student.Grades.Add(grade);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to add grade due to database error.", ex);
            }
        }
    }
}
```