namespace YourProject.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IStudentService
    {
        Task<List<Student>> GetAllStudentsAsync();
    }
}