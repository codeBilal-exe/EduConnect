using EduConnect.Data;
using EduConnect.Interfaces;
using EduConnect.Models;

namespace EduConnect.Services;

// SRP: Only manages grades
// DIP: Implements IGradeService abstraction
public class GradeService : IGradeService
{
    private readonly List<GradeRecord> _gradeRecords;
    public event Action? OnGradesSubmitted;

    public GradeService()
    {
        _gradeRecords = SeedData.GradeRecords;
    }

    // SRP: Submitting grades is a focused operation
    public void SubmitGrade(GradeRecord record)
    {
        var existing = _gradeRecords.FirstOrDefault(g => g.StudentId == record.StudentId && g.CourseId == record.CourseId);
        if (existing != null)
        {
            existing.Marks = record.Marks;
        }
        else
        {
            _gradeRecords.Add(record);
        }

        OnGradesSubmitted?.Invoke();
    }

    public List<GradeRecord> GetGradesForStudent(Guid studentId)
    {
        return _gradeRecords
            .Where(g => g.StudentId == studentId)
            .ToList();
    }

    public List<GradeRecord> GetGradesForCourse(Guid courseId)
    {
        return _gradeRecords
            .Where(g => g.CourseId == courseId)
            .ToList();
    }
}

