```csharp
using System;
using System.Collections.Generic;

namespace conversion.Models
{
    public class Student
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public List<Grade> Grades { get; private set; }

        public Student(int id, string firstName, string lastName, DateTime dateOfBirth)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Grades = new List<Grade>();
        }

        public void AddGrade(Grade grade)
        {
            if (grade == null)
            {
                throw new ArgumentNullException(nameof(grade), "Grade cannot be null");
            }

            if (Grades == null)
            {
                throw new InvalidOperationException("Grades collection is not initialized");
            }

            if (grade.Score < 0 || grade.Score > 100)
            {
                throw new ArgumentException("Invalid score value", nameof(grade));
            }

            if (!Grades.Contains(grade))
            {
                Grades.Add(grade);
            }
            else
            {
                throw new InvalidOperationException("Grade already exists for the student");
            }
        }
    }

    public class Grade : IEquatable<Grade>
    {
        public int Id { get; private set; }
        public string Subject { get; private set; }
        public decimal Score { get; private set; }

        public Grade(int id, string subject, decimal score)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentException("Subject cannot be null or empty", nameof(subject));
            }

            Id = id;
            Subject = subject;
            Score = score;
        }

        public bool Equals(Grade other)
        {
            return other != null &&
                   Id == other.Id &&
                   Subject == other.Subject &&
                   Score == other.Score;
        }
    }
}
```