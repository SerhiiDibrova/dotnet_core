package conversion.Interfaces;

import System.Threading.Tasks;
import conversion.Models.Grade;
import conversion.Models.GradeDto;

public interface IGradeService {
    Task<Grade> AddGradeAsync(GradeDto gradeDto);
}