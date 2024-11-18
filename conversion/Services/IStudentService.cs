namespace Conversion.Services
{
    using System.Threading.Tasks;
    using Conversion.Models;

    public interface IStudentService
    {
        Task<Student> CreateStudentAsync(CreateStudentRequest request);
    }
}