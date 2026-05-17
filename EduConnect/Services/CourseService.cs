using EduConnect.Data;
using EduConnect.Interfaces;
using EduConnect.Models;
using EduConnect.Models.Exceptions;
using EduConnect.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Services;

public class CourseService : ICourseService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly INotificationService _notificationService;

    public event Action? OnEnrollmentChanged;

    public CourseService(IDbContextFactory<AppDbContext> dbContextFactory, INotificationService notificationService)
    {
        _dbContextFactory = dbContextFactory;
        _notificationService = notificationService;
    }

    private static Course MapToCourse(Course source, IReadOnlyDictionary<int, List<int>> enrollmentMap)
    {
        return new Course
        {
            Id = source.Id,
            Code = source.Code,
            Title = source.Title,
            CreditHours = source.CreditHours,
            MaxCapacity = source.MaxCapacity,
            FacultyId = source.FacultyId,
            EnrolledStudentIds = enrollmentMap.TryGetValue(source.Id, out var enrolled) ? new List<int>(enrolled) : new List<int>()
        };
    }

    public Course? GetById(int id)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var course = context.Courses.AsNoTracking().FirstOrDefault(c => c.Id == id);
        if (course == null)
            return null;

        var enrolledStudentIds = context.Enrollments
            .Where(e => e.CourseId == id && e.State == EnrollmentState.Active)
            .Select(e => e.StudentId)
            .ToList();

        return MapToCourse(course, new Dictionary<int, List<int>> { { id, enrolledStudentIds } });
    }

    public List<Course> GetAll()
    {
        using var context = _dbContextFactory.CreateDbContext();
        var courses = context.Courses.AsNoTracking().ToList();
        var courseIds = courses.Select(c => c.Id).ToList();

        var enrollmentMap = context.Enrollments
            .Where(e => courseIds.Contains(e.CourseId) && e.State == EnrollmentState.Active)
            .AsNoTracking()
            .GroupBy(e => e.CourseId)
            .ToDictionary(g => g.Key, g => g.Select(e => e.StudentId).ToList());

        return courses.Select(c => MapToCourse(c, enrollmentMap)).ToList();
    }

    public void Add(Course entity)
    {
        using var context = _dbContextFactory.CreateDbContext();
        if (context.Courses.Any(c => c.Id == entity.Id))
            return;

        context.Courses.Add(new Course
        {
            Id = entity.Id,
            Code = entity.Code,
            Title = entity.Title,
            CreditHours = entity.CreditHours,
            MaxCapacity = entity.MaxCapacity,
            FacultyId = entity.FacultyId
        });

        context.SaveChanges();
    }

    public void Update(Course entity)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var existing = context.Courses.FirstOrDefault(c => c.Id == entity.Id);
        if (existing == null)
            return;

        existing.Code = entity.Code;
        existing.Title = entity.Title;
        existing.CreditHours = entity.CreditHours;
        existing.MaxCapacity = entity.MaxCapacity;
        existing.FacultyId = entity.FacultyId;

        context.SaveChanges();
    }

    public void Delete(int id)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var course = context.Courses.FirstOrDefault(c => c.Id == id);
        if (course == null)
            return;

        var enrollments = context.Enrollments.Where(e => e.CourseId == id).ToList();
        if (enrollments.Any())
            context.Enrollments.RemoveRange(enrollments);

        var gradeRecords = context.GradeRecords.Where(g => g.CourseId == id).ToList();
        if (gradeRecords.Any())
            context.GradeRecords.RemoveRange(gradeRecords);

        context.Courses.Remove(course);
        context.SaveChanges();
    }

    public List<Course> GetAvailableCourses()
    {
        return GetAll()
            .Where(c => c.EnrollmentStatus != EnrollmentStatus.Full)
            .ToList();
    }

    public List<Course> GetCoursesByFaculty(int facultyId)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var courses = context.Courses.AsNoTracking().Where(c => c.FacultyId == facultyId).ToList();
        var courseIds = courses.Select(c => c.Id).ToList();

        var enrollmentMap = context.Enrollments
            .Where(e => courseIds.Contains(e.CourseId) && e.State == EnrollmentState.Active)
            .AsNoTracking()
            .GroupBy(e => e.CourseId)
            .ToDictionary(g => g.Key, g => g.Select(e => e.StudentId).ToList());

        return courses.Select(c => MapToCourse(c, enrollmentMap)).ToList();
    }

    public bool CourseCodeExists(string code, int? excludeId = null)
    {
        using var context = _dbContextFactory.CreateDbContext();
        return context.Courses.Any(c => c.Code == code && c.Id != excludeId);
    }

    public void EnrollStudent(int courseId, int studentId)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var course = context.Courses.FirstOrDefault(c => c.Id == courseId);
        if (course == null)
            throw new InvalidOperationException("Course not found.");

        var activeEnrollmentCount = context.Enrollments.Count(e => e.CourseId == courseId && e.State == EnrollmentState.Active);
        if (activeEnrollmentCount >= course.MaxCapacity)
            throw new CourseFullException(course.Title);

        var existingEnrollment = context.Enrollments.FirstOrDefault(e =>
            e.StudentId == studentId &&
            e.CourseId == courseId);

        if (existingEnrollment?.State == EnrollmentState.Active)
            throw new InvalidOperationException("Student is already enrolled in this course in the current semester.");

        if (existingEnrollment != null)
        {
            existingEnrollment.State = EnrollmentState.Active;
            existingEnrollment.Semester = "Spring 2026";
            existingEnrollment.EnrolledAt = DateTime.Now;
        }
        else
        {
            context.Enrollments.Add(new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                Semester = "Spring 2026",
                State = EnrollmentState.Active,
                EnrolledAt = DateTime.Now
            });
        }

        context.SaveChanges();

        _notificationService.SendNotification(new Notification
        {
            StudentId = studentId,
            Message = $"You have successfully enrolled in {course.Code}: {course.Title}.",
            Type = NotificationType.Enrollment,
            CreatedAt = DateTime.Now,
            IsRead = false
        });

        OnEnrollmentChanged?.Invoke();
    }

    public void DropCourse(int courseId, int studentId)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var course = context.Courses.FirstOrDefault(c => c.Id == courseId);
        if (course == null)
            throw new InvalidOperationException("Course not found.");

        var enrollment = context.Enrollments.FirstOrDefault(e => e.StudentId == studentId && e.CourseId == courseId && e.State == EnrollmentState.Active);
        if (enrollment == null)
            throw new InvalidOperationException("Enrollment not found.");

        enrollment.State = EnrollmentState.Dropped;

        context.SaveChanges();

        _notificationService.SendNotification(new Notification
        {
            StudentId = studentId,
            Message = $"You have dropped the course {course.Code}: {course.Title}.",
            Type = NotificationType.Enrollment,
            CreatedAt = DateTime.Now,
            IsRead = false
        });

        OnEnrollmentChanged?.Invoke();
    }
}
