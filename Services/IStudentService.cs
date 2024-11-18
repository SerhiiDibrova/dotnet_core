namespace YourProject.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using YourProject.Models;
    using Microsoft.AspNetCore.Mvc;

    public interface IStudentService
    {
        Task<IActionResult> CreateStudentAsync(StudentDto studentDto);
        Task<Student> GetStudentByIdAsync(int id);
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student> UpdateStudentAsync(int id, StudentDto studentDto);
        Task DeleteStudentAsync(int id);
    }

    public class StudentService : IStudentService
    {
        private readonly List<Student> _students = new List<Student>();

        public async Task<IActionResult> CreateStudentAsync(StudentDto studentDto)
        {
            if (_students.Exists(s => s.Email == studentDto.Email))
            {
                return new ConflictResult();
            }

            if (studentDto.Age < 18)
            {
                return new BadRequestResult();
            }

            var student = new Student
            {
                Id = _students.Count + 1,
                Name = studentDto.Name,
                Email = studentDto.Email,
                Age = studentDto.Age
            };

            _students.Add(student);
            return new OkObjectResult(student);
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            return _students.Find(s => s.Id == id);
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return _students;
        }

        public async Task<Student> UpdateStudentAsync(int id, StudentDto studentDto)
        {
            var student = _students.Find(s => s.Id == id);
            if (student != null)
            {
                student.Name = studentDto.Name;
                student.Email = studentDto.Email;
                student.Age = studentDto.Age;
            }
            return student;
        }

        public async Task DeleteStudentAsync(int id)
        {
            var student = _students.Find(s => s.Id == id);
            if (student != null)
            {
                _students.Remove(student);
            }
        }
    }
}