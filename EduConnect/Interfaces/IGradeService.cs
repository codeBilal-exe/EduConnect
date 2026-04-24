using EduConnect.Models;

namespace EduConnect.Interfaces;

// SRP: Only manages grades
public interface IGradeService
{
    void SubmitGrade(GradeRecord record);
    List<GradeRecord> GetGradesForStudent(Guid studentId);
    List<GradeRecord> GetGradesForCourse(Guid courseId);
    void MarkNotificationAsRead(Guid notificationId);

    event Action? OnGradesSubmitted;
}
