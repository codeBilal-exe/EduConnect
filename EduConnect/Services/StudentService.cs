using EduConnect.Data;
using EduConnect.Data.Entities;
using EduConnect.Interfaces;
using EduConnect.Models;
using EduConnect.Models.Enums;
using EduConnect.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Services;

public class StudentService : IStudentService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly IGradeService _gradeService;
    private readonly ICourseService _courseService;

    public event Action? OnStudentUpdated;

    public StudentService(IDbContextFactory<AppDbContext> dbContextFactory, IGradeService gradeService, ICourseService courseService)
    {
        _dbContextFactory = dbContextFactory;
        _gradeService = gradeService;
        _courseService = courseService;
    }

    private static Student MapToStudent(StudentEntity entity)
    {
        return new Student
        {
            Id = entity.Id,
            FullName = entity.FullName,
            Email = entity.Email,
            PasswordHash = entity.PasswordHash,
            Role = UserRole.Student,
            Semester = entity.Semester
        };
    }

    public Student? GetById(int id)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var student = context.Students.FirstOrDefault(s => s.Id == id);
        if (student == null)
            return null;

        return MapToStudent(student);
    }

    public List<Student> GetAll()
    {
        using var context = _dbContextFactory.CreateDbContext();
        var students = context.Students.AsNoTracking().ToList();
        return students.Select(MapToStudent).ToList();
    }

    public void Add(Student entity)
    {
        using var context = _dbContextFactory.CreateDbContext();
        if (context.Students.Any(s => s.Email == entity.Email))
            return;

        context.Students.Add(new StudentEntity
        {
            FullName = entity.FullName,
            Email = entity.Email,
            PasswordHash = entity.PasswordHash,
            Semester = entity.Semester
        });

        context.SaveChanges();
    }

    public void Update(Student entity)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var existing = context.Students.FirstOrDefault(s => s.Id == entity.Id);
        if (existing == null)
            return;

        existing.FullName = entity.FullName;
        existing.Email = entity.Email;
        existing.Semester = entity.Semester;

        context.SaveChanges();
        OnStudentUpdated?.Invoke();
    }

    public void Delete(int id)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var student = context.Students.FirstOrDefault(s => s.Id == id);
        if (student == null)
            return;

        var hasActiveEnrollments = context.Enrollments.Any(e => e.StudentId == id && e.State == EnrollmentState.Active);
        if (hasActiveEnrollments)
            throw new StudentHasActiveEnrollmentsException(student.FullName);

        var notifications = context.Notifications.Where(n => n.StudentId == id).ToList();
        if (notifications.Any())
            context.Notifications.RemoveRange(notifications);

        var enrollments = context.Enrollments.Where(e => e.StudentId == id).ToList();
        if (enrollments.Any())
            context.Enrollments.RemoveRange(enrollments);

        var gradeRecords = context.GradeRecords.Where(g => g.StudentId == id).ToList();
        if (gradeRecords.Any())
            context.GradeRecords.RemoveRange(gradeRecords);

        context.Students.Remove(student);
        context.SaveChanges();

        OnStudentUpdated?.Invoke();
    }

    public List<Student> Search(string term)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var students = context.Students
            .Where(s => s.FullName.Contains(term))
            .AsNoTracking()
            .ToList();

        return students.Select(MapToStudent).ToList();
    }

    public double ComputeCGPA(int studentId)
    {
        var grades = _gradeService.GetGradesForStudent(studentId);
        if (grades.Count == 0)
            return 0.0;

        var courses = _courseService.GetAll();
        var totalWeightedPoints = 0.0;
        var totalCreditHours = 0;

        foreach (var grade in grades)
        {
            var course = courses.FirstOrDefault(c => c.Id == grade.CourseId);
            if (course != null)
            {
                totalWeightedPoints += grade.GradePoint * course.CreditHours;
                totalCreditHours += course.CreditHours;
            }
        }

        return totalCreditHours > 0 ? totalWeightedPoints / totalCreditHours : 0.0;
    }
}
