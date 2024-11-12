package conversion.Services;

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

public interface IGradeService
{
    string AddGrade(int studentId, int subjectId, int score);
}

public class GradeService : IGradeService
{
    private readonly HashSet<int> _validStudentIds;
    private readonly HashSet<int> _validSubjectIds;
    private readonly List<(int studentId, int subjectId, int score)> _grades;
    private readonly int _minScore;
    private readonly int _maxScore;
    private readonly ILogger<GradeService> _logger;

    public GradeService(HashSet<int> validStudentIds, HashSet<int> validSubjectIds, ILogger<GradeService> logger, int minScore = 0, int maxScore = 100)
    {
        _validStudentIds = validStudentIds;
        _validSubjectIds = validSubjectIds;
        _grades = new List<(int studentId, int subjectId, int score)>();
        _minScore = minScore;
        _maxScore = maxScore;
        _logger = logger;
    }

    public string AddGrade(int studentId, int subjectId, int score)
    {
        try
        {
            if (!_validStudentIds.Contains(studentId))
            {
                return $"Error: Student ID {studentId} does not exist.";
            }

            if (!_validSubjectIds.Contains(subjectId))
            {
                return $"Error: Subject ID {subjectId} does not exist.";
            }

            if (score < _minScore || score > _maxScore)
            {
                return $"Error: Score must be between {_minScore} and {_maxScore}.";
            }

            _grades.Add((studentId, subjectId, score));
            _logger.LogInformation($"Grade added successfully for Student ID {studentId}, Subject ID {subjectId}, Score {score}.");
            return "Grade added successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while adding a grade.");
            return "Error: An unexpected error occurred. Please try again later.";
        }
    }
}