```csharp
using System;
using System.Threading.Tasks;

namespace Conversion.Services
{
    public enum GradeServiceErrorCodes
    {
        StudentNotFound,
        InvalidGrade
    }

    public class GradeServiceException : Exception
    {
        public GradeServiceErrorCodes ErrorCode { get; set; }
        public override string Message => ErrorCode.ToString();
    }

    public interface IGradeService
    {
        Task AddGradeAsync(int studentId, decimal grade);
        Task<bool> ValidateStudentExistsAsync(int studentId);
        Task<bool> ValidateGradeAsync(decimal grade);

        event EventHandler<GradeServiceException> ErrorOccurred;
    }
}
```