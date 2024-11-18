namespace YourProject.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IStudentService
    {
        Task<List<Student>> GetAllStudentsAsync();
    }

    public class StudentService : IStudentService
    {
        private readonly List<Student> _students;

        public StudentService()
        {
            _students = new List<Student>
            {
                new Student { Id = 1, Name = "John Doe" },
                new Student { Id = 2, Name = "Jane Smith" }
            };
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await Task.FromResult(_students);
        }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}