using EduConnect.Models;

namespace EduConnect.Interfaces;

// ISP: Extends IRepository but segregates grade methods to IGradeService
// SRP: Only manages course data and enrollments
public interface ICourseService : IRepository<Course>
{
    List<Course> GetAvailableCourses();
    List<Course> GetCoursesByFaculty(Guid facultyId);
    bool CourseCodeExists(string code, Guid? excludeId = null);
    void EnrollStudent(Guid courseId, Guid studentId);
    void DropCourse(Guid courseId, Guid studentId);

    event Action? OnEnrollmentChanged;
}
