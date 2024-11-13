package Interfaces;

using System.Threading.Tasks;

public interface IStudentService
{
    Task<StudentWithGradesDto> GetStudentWithGradesAsync(int id);
}