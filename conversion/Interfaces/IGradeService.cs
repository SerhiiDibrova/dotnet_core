package conversion.Interfaces;

using System.Threading.Tasks;
using System.Web.Http;

public interface IGradeService
{
    Task<GradeDto> AddGradeAsync(GradeDto gradeDto);
}