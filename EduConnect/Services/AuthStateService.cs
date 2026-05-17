using EduConnect.Data;
using EduConnect.Data.Entities;
using EduConnect.Interfaces;
using EduConnect.Models;
using EduConnect.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Services;

public class AuthStateService : IAuthStateService
{
    private Person? _currentUser;
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public Person? CurrentUser
    {
        get => _currentUser;
        private set
        {
            _currentUser = value;
            OnAuthStateChanged?.Invoke();
        }
    }

    public event Action? OnAuthStateChanged;

    public AuthStateService(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public bool Login(string email, string password)
    {
        using var context = _dbContextFactory.CreateDbContext();

        // Try Student login
        var student = context.Students.FirstOrDefault(s => s.Email == email && s.PasswordHash == password);
        if (student != null)
        {
            CurrentUser = new Student
            {
                Id = student.Id,
                FullName = student.FullName,
                Email = student.Email,
                PasswordHash = student.PasswordHash,
                Role = UserRole.Student,
                Semester = student.Semester
            };
            return true;
        }

        // Try Faculty login
        var faculty = context.Faculty.FirstOrDefault(f => f.Email == email && f.PasswordHash == password);
        if (faculty != null)
        {
            var assignedCourseIds = context.Courses
                .Where(c => c.FacultyId == faculty.Id)
                .Select(c => c.Id)
                .ToList();

            CurrentUser = new Faculty
            {
                Id = faculty.Id,
                FullName = faculty.FullName,
                Email = faculty.Email,
                PasswordHash = faculty.PasswordHash,
                Role = UserRole.Faculty,
                Department = faculty.Department,
                AssignedCourseIds = assignedCourseIds
            };
            return true;
        }

        // Try Admin login
        var admin = context.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password && u.Role == "Admin");
        if (admin != null)
        {
            CurrentUser = new Admin
            {
                Id = admin.Id,
                FullName = admin.FullName,
                Email = admin.Email,
                PasswordHash = admin.PasswordHash,
                Role = UserRole.Admin
            };
            return true;
        }

        return false;
    }

    public void Logout()
    {
        CurrentUser = null;
    }

    public bool IsInRole(UserRole role)
    {
        return CurrentUser?.Role == role;
    }

    public void RegisterUser(Person person)
    {
        if (person == null)
            return;

        using var context = _dbContextFactory.CreateDbContext();

        if (person is Student student)
        {
            if (context.Students.Any(s => s.Email == student.Email))
                return;

            var newStudent = new StudentEntity
            {
                FullName = student.FullName,
                Email = student.Email,
                PasswordHash = student.PasswordHash,
                Semester = student.Semester
            };
            context.Students.Add(newStudent);
            context.SaveChanges();
        }
        else if (person is Faculty fac)
        {
            if (context.Faculty.Any(f => f.Email == fac.Email))
                return;

            var newFaculty = new FacultyEntity
            {
                FullName = fac.FullName,
                Email = fac.Email,
                PasswordHash = fac.PasswordHash,
                Department = fac.Department
            };
            context.Faculty.Add(newFaculty);
            context.SaveChanges();
        }
        else if (person is Admin admin)
        {
            if (context.Users.Any(u => u.Email == admin.Email))
                return;

            var newAdmin = new UserEntity
            {
                FullName = admin.FullName,
                Email = admin.Email,
                PasswordHash = admin.PasswordHash,
                Role = "Admin"
            };
            context.Users.Add(newAdmin);
            context.SaveChanges();
        }
    }

    public IEnumerable<Person> GetAllUsers()
    {
        using var context = _dbContextFactory.CreateDbContext();
        var users = new List<Person>();

        var students = context.Students.AsNoTracking().ToList();
        users.AddRange(students.Select(s => new Student
        {
            Id = s.Id,
            FullName = s.FullName,
            Email = s.Email,
            PasswordHash = s.PasswordHash,
            Role = UserRole.Student,
            Semester = s.Semester
        }));

        var faculty = context.Faculty.AsNoTracking().ToList();
        var courseAssignments = context.Courses.AsNoTracking()
            .GroupBy(c => c.FacultyId)
            .ToDictionary(g => g.Key, g => g.Select(c => c.Id).ToList());

        users.AddRange(faculty.Select(f => new Faculty
        {
            Id = f.Id,
            FullName = f.FullName,
            Email = f.Email,
            PasswordHash = f.PasswordHash,
            Role = UserRole.Faculty,
            Department = f.Department,
            AssignedCourseIds = courseAssignments.GetValueOrDefault(f.Id) ?? new List<int>()
        }));

        var admins = context.Users.AsNoTracking().Where(u => u.Role == "Admin").ToList();
        users.AddRange(admins.Select(a => new Admin
        {
            Id = a.Id,
            FullName = a.FullName,
            Email = a.Email,
            PasswordHash = a.PasswordHash,
            Role = UserRole.Admin
        }));

        return users;
    }
}
