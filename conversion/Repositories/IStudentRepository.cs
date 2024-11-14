```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Conversion.Repositories
{
    public interface IStudentRepository
    {
        Task AddAsync(Student student);
        Task<Student> GetByIdAsync(Guid id);
        Task UpdateAsync(Student student);
        Task DeleteAsync(Guid id);

        Task AddGradeAsync(Guid studentId, Grade grade);
        Task<List<Grade>> GetGradesAsync(Guid studentId);
    }

    public class Student
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Grade> Grades { get; set; } = new List<Grade>();
    }

    public class Grade
    {
        public Guid Id { get; set; }
        public decimal Value { get; set; }
        public Guid StudentId { get; set; }
    }
}
```