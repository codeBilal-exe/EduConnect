using EduConnect.Data;
using EduConnect.Interfaces;
using EduConnect.Models;
using EduConnect.Models.Exceptions;
using EduConnect.Models.Enums;

namespace EduConnect.Services;

// SRP: Only manages course data and enrollments
// DIP: Implements ICourseService abstraction
public class CourseService : ICourseService
{
    private readonly List<Course> _courses;
    private readonly List<Enrollment> _enrollments;

    public event Action? OnEnrollmentChanged;

    public CourseService()
    {
        _courses = new List<Course>(SeedData.Courses);
        _enrollments = new List<Enrollment>(SeedData.Enrollments);
    }

    public Course? GetById(Guid id)
    {
        return _courses.FirstOrDefault(c => c.Id == id);
    }

    public List<Course> GetAll()
    {
        return new List<Course>(_courses);
    }

    public void Add(Course entity)
    {
        if (!_courses.Any(c => c.Id == entity.Id))
        {
            _courses.Add(entity);
        }
    }

    public void Update(Course entity)
    {
        var existing = GetById(entity.Id);
        if (existing != null)
        {
            existing.Code = entity.Code;
            existing.Title = entity.Title;
            existing.CreditHours = entity.CreditHours;
            existing.MaxCapacity = entity.MaxCapacity;
            existing.FacultyId = entity.FacultyId;
        }
    }

    public void Delete(Guid id)
    {
        var course = GetById(id);
        if (course != null)
        {
            _courses.Remove(course);
        }
    }

    // OCP: New enrollment statuses can be computed without modifying this
    public List<Course> GetAvailableCourses()
    {
        return _courses
            .Where(c => c.EnrollmentStatus != EnrollmentStatus.Full)
            .ToList();
    }

    public List<Course> GetCoursesByFaculty(Guid facultyId)
    {
        return _courses
            .Where(c => c.FacultyId == facultyId)
            .ToList();
    }

    // SRP: Enrollment logic is concentrated here, not in components
    public void EnrollStudent(Guid courseId, Guid studentId)
    {
        var course = GetById(courseId);
        if (course == null)
            throw new InvalidOperationException("Course not found.");

        if (course.EnrollmentStatus == EnrollmentStatus.Full)
            throw new CourseFullException(course.Title);

        if (course.EnrolledStudentIds.Contains(studentId))
            throw new InvalidOperationException("Student is already enrolled in this course.");

        course.EnrolledStudentIds.Add(studentId);

        var enrollment = new Enrollment
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            CourseId = courseId,
            Semester = "Spring 2026",
            State = EnrollmentState.Active,
            EnrolledAt = DateTime.Now
        };

        _enrollments.Add(enrollment);
        OnEnrollmentChanged?.Invoke();
    }

    // SRP: Drop logic is concentrated here, not in components
    public void DropCourse(Guid courseId, Guid studentId)
    {
        var course = GetById(courseId);
        if (course == null)
            throw new InvalidOperationException("Course not found.");

        var enrollment = _enrollments.FirstOrDefault(e => e.StudentId == studentId && e.CourseId == courseId);
        if (enrollment == null)
            throw new InvalidOperationException("Enrollment not found.");

        if (enrollment.State != EnrollmentState.Active)
            throw new InvalidOperationException("Can only drop active courses.");

        enrollment.State = EnrollmentState.Dropped;
        course.EnrolledStudentIds.Remove(studentId);
        OnEnrollmentChanged?.Invoke();
    }
}
