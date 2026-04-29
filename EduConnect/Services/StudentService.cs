using EduConnect.Data;
using EduConnect.Interfaces;
using EduConnect.Models;
using EduConnect.Models.Exceptions;

namespace EduConnect.Services;

// SRP: Only manages student data
// DIP: Implements IStudentService abstraction
public class StudentService : IStudentService
{
    private readonly List<Person> _users;
    private readonly IGradeService _gradeService;
    private readonly ICourseService _courseService;

    public event Action? OnStudentUpdated;

    public StudentService(IGradeService gradeService, ICourseService courseService)
    {
        _gradeService = gradeService;
        _courseService = courseService;
        _users = SeedData.Users;
    }

    private IEnumerable<Student> _students => _users.OfType<Student>();

    public Student? GetById(Guid id)
    {
        return _students.FirstOrDefault(s => s.Id == id);
    }

    public List<Student> GetAll()
    {
        return _students.ToList();
    }


    public void Add(Student entity)
    {
        if (!_users.Any(s => s.Id == entity.Id))
        {
            _users.Add(entity);
        }
    }

    public void Update(Student entity)
    {
        var existing = GetById(entity.Id);
        if (existing != null)
        {
            existing.FullName = entity.FullName;
            existing.Email = entity.Email;
            existing.Semester = entity.Semester;
            OnStudentUpdated?.Invoke();
        }
    }

    public void Delete(Guid id)
    {
        var student = GetById(id);
        if (student != null)
        {
            var hasActiveEnrollments = _courseService.GetAll().Any(c => c.EnrolledStudentIds.Contains(id));
            if (hasActiveEnrollments)
            {
                throw new StudentHasActiveEnrollmentsException(student.FullName);
            }
            _users.Remove(student);
        }
    }


    // ISP: Search is a specialized operation for students
    public List<Student> Search(string term)
    {
        return _students
            .Where(s => s.FullName.Contains(term, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    // SRP: CGPA computation uses service dependencies (grade and course data)
    public double ComputeCGPA(Guid studentId)
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
