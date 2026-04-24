using EduConnect.Data;
using EduConnect.Interfaces;
using EduConnect.Models;

namespace EduConnect.Services;

// SRP: Only manages grades
// DIP: Implements IGradeService abstraction
public class GradeService : IGradeService
{
    private readonly List<GradeRecord> _gradeRecords;
    private readonly List<Notification> _notifications;

    public event Action? OnGradesSubmitted;

    public GradeService()
    {
        _gradeRecords = new List<GradeRecord>(SeedData.GradeRecords);
        _notifications = new List<Notification>(SeedData.Notifications);
    }

    // SRP: Submitting grades is a focused operation
    public void SubmitGrade(GradeRecord record)
    {
        var existing = _gradeRecords.FirstOrDefault(g => g.Id == record.Id);
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

    // ISP: Grade service also manages mark-as-read for grade notifications
    public void MarkNotificationAsRead(Guid notificationId)
    {
        var notification = _notifications.FirstOrDefault(n => n.Id == notificationId);
        if (notification != null)
        {
            notification.IsRead = true;
        }
    }
}
