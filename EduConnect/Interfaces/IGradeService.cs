using EduConnect.Models;

namespace EduConnect.Interfaces;

// SRP: Only manages grades
public interface IGradeService
{
    void SubmitGrade(GradeRecord record);
    List<GradeRecord> GetGradesForStudent(int studentId);
    List<GradeRecord> GetGradesForCourse(int courseId);

    event Action? OnGradesSubmitted;
}
