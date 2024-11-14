namespace Conversion.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the contract for the student service.
    /// </summary>
    public interface IStudentService
    {
        /// <summary>
        /// Asynchronously adds a grade for a student.
        /// </summary>
        /// <param name="grade">The grade to add.</param>
        /// <returns>The added grade.</returns>
        Task<Grade> AddGradeAsync(Grade grade);

        /// <summary>
        /// Asynchronously retrieves a student by their ID.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <returns>The student if found; otherwise, null.</returns>
        Task<Student> GetStudentByIdAsync(int studentId);

        /// <summary>
        /// Asynchronously retrieves all students.
        /// </summary>
        /// <returns>A collection of all students.</returns>
        Task<IEnumerable<Student>> GetAllStudentsAsync();

        /// <summary>
        /// Asynchronously adds a new student.
        /// </summary>
        /// <param name="student">The student to add.</param>
        /// <returns>The added student.</returns>
        Task<Student> AddStudentAsync(Student student);

        /// <summary>
        /// Asynchronously updates an existing student.
        /// </summary>
        /// <param name="student">The student with updated information.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        Task<bool> UpdateStudentAsync(Student student);

        /// <summary>
        /// Asynchronously deletes a student by their ID.
        /// </summary>
        /// <param name="studentId">The ID of the student to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        Task<bool> DeleteStudentAsync(int studentId);

        /// <summary>
        /// Asynchronously retrieves all grades for a specific student.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <returns>A collection of grades for the student.</returns>
        Task<IEnumerable<Grade>> GetGradesByStudentIdAsync(int studentId);
    }

    public class Grade
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string Subject { get; set; }
        public double Score { get; set; }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class StudentService : IStudentService
    {
        private readonly List<Student> students = new List<Student>();
        private readonly List<Grade> grades = new List<Grade>();

        public async Task<Grade> AddGradeAsync(Grade grade)
        {
            if (grade == null || grade.StudentId <= 0 || string.IsNullOrWhiteSpace(grade.Subject) || grade.Score < 0)
            {
                throw new ArgumentException("Invalid grade information.");
            }

            grades.Add(grade);
            return await Task.FromResult(grade);
        }

        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            var student = students.Find(s => s.Id == studentId);
            return await Task.FromResult(student);
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await Task.FromResult(students);
        }

        public async Task<Student> AddStudentAsync(Student student)
        {
            if (student == null || string.IsNullOrWhiteSpace(student.Name) || student.Age <= 0)
            {
                throw new ArgumentException("Invalid student information.");
            }

            students.Add(student);
            return await Task.FromResult(student);
        }

        public async Task<bool> UpdateStudentAsync(Student student)
        {
            if (student == null || student.Id <= 0 || string.IsNullOrWhiteSpace(student.Name) || student.Age <= 0)
            {
                throw new ArgumentException("Invalid student information.");
            }

            var existingStudent = students.Find(s => s.Id == student.Id);
            if (existingStudent != null)
            {
                existingStudent.Name = student.Name;
                existingStudent.Age = student.Age;
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> DeleteStudentAsync(int studentId)
        {
            var student = students.Find(s => s.Id == studentId);
            if (student != null)
            {
                students.Remove(student);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<IEnumerable<Grade>> GetGradesByStudentIdAsync(int studentId)
        {
            var studentGrades = grades.FindAll(g => g.StudentId == studentId);
            return await Task.FromResult(studentGrades);
        }
    }
}