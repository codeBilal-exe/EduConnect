using EduConnect.Models;

namespace EduConnect.Interfaces;

// ISP: Extends IRepository but segregates grade methods to IGradeService
// SRP: Only manages course data and enrollments
public interface ICourseService : IRepository<Course>
{
    List<Course> GetAvailableCourses();
    List<Course> GetCoursesByFaculty(int facultyId);
    bool CourseCodeExists(string code, int? excludeId = null);
    void EnrollStudent(int courseId, int studentId);
    void DropCourse(int courseId, int studentId);

    event Action? OnEnrollmentChanged;
}
