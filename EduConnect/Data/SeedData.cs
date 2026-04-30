using EduConnect.Models;
using EduConnect.Models.Enums;

namespace EduConnect.Data;

public static class SeedData
{
    private static readonly Guid AdminId = Guid.Parse("00000001-0000-0000-0000-000000000001");

    private static readonly Guid Faculty1Id = Guid.Parse("00000001-0000-0000-0000-000000000002");
    private static readonly Guid Faculty2Id = Guid.Parse("00000001-0000-0000-0000-000000000003");
    private static readonly Guid Faculty3Id = Guid.Parse("00000001-0000-0000-0000-000000000004");

    private static readonly Guid Student1Id = Guid.Parse("00000001-0000-0000-0000-000000000005");
    private static readonly Guid Student2Id = Guid.Parse("00000001-0000-0000-0000-000000000006");
    private static readonly Guid Student3Id = Guid.Parse("00000001-0000-0000-0000-000000000007");
    private static readonly Guid Student4Id = Guid.Parse("00000001-0000-0000-0000-000000000008");

    private static readonly Guid Course1Id = Guid.Parse("00000002-0000-0000-0000-000000000001");
    private static readonly Guid Course2Id = Guid.Parse("00000002-0000-0000-0000-000000000002");
    private static readonly Guid Course3Id = Guid.Parse("00000002-0000-0000-0000-000000000003");
    private static readonly Guid Course4Id = Guid.Parse("00000002-0000-0000-0000-000000000004");
    private static readonly Guid Course5Id = Guid.Parse("00000002-0000-0000-0000-000000000005");
    private static readonly Guid Course6Id = Guid.Parse("00000002-0000-0000-0000-000000000006");

    private static readonly Guid Enrollment1Id = Guid.Parse("00000003-0000-0000-0000-000000000001");
    private static readonly Guid Enrollment2Id = Guid.Parse("00000003-0000-0000-0000-000000000002");
    private static readonly Guid Enrollment3Id = Guid.Parse("00000003-0000-0000-0000-000000000003");
    private static readonly Guid Enrollment4Id = Guid.Parse("00000003-0000-0000-0000-000000000004");

    private static readonly Guid Grade1Id = Guid.Parse("00000004-0000-0000-0000-000000000001");
    private static readonly Guid Grade2Id = Guid.Parse("00000004-0000-0000-0000-000000000002");
    private static readonly Guid Grade3Id = Guid.Parse("00000004-0000-0000-0000-000000000003");
    private static readonly Guid Grade4Id = Guid.Parse("00000004-0000-0000-0000-000000000004");

    public static readonly List<Person> Users = new()
    {
        // Admin
        new Admin
        {
            Id = AdminId,
            FullName = "Admin User",
            Email = "admin@edu.com",
            PasswordHash = "admin123",
            Role = UserRole.Admin
        },
        // Faculty
        new Faculty
        {
            Id = Faculty1Id,
            FullName = "Hafiz Obiadullah Khan",
            Email = "faculty1@edu.com",
            PasswordHash = "faculty123",
            Role = UserRole.Faculty,
            Department = "Computer Science",
            AssignedCourseIds = new List<Guid> { Course1Id, Course2Id }
        },
        new Faculty
        {
            Id = Faculty2Id,
            FullName = "Prof. Atif Aslam",
            Email = "faculty2@edu.com",
            PasswordHash = "faculty123",
            Role = UserRole.Faculty,
            Department = "Mathematics",
            AssignedCourseIds = new List<Guid> { Course3Id, Course4Id }
        },
        new Faculty
        {
            Id = Faculty3Id,
            FullName = "Mam Amna Tariq",
            Email = "faculty3@edu.com",
            PasswordHash = "faculty123",
            Role = UserRole.Faculty,
            Department = "Mathematics",
            AssignedCourseIds = new List<Guid> { Course5Id, Course6Id }
        },
        // Students
        new Student
        {
            Id = Student1Id,
            FullName = "Muhammad Bilal",
            Email = "student1@edu.com",
            PasswordHash = "student123",
            Role = UserRole.Student,
            Semester = 4
        },
        new Student
        {
            Id = Student2Id,
            FullName = "Usaidullah Rehan",
            Email = "student2@edu.com",
            PasswordHash = "student123",
            Role = UserRole.Student,
            Semester = 3
        },
        new Student
        {
            Id = Student3Id,
            FullName = "Sajjad Ejaz",
            Email = "student3@edu.com",
            PasswordHash = "student123",
            Role = UserRole.Student,
            Semester = 2
        },
        new Student
        {
            Id = Student4Id,
            FullName = "Abdullah Abdullah",
            Email = "student4@edu.com",
            PasswordHash = "student123",
            Role = UserRole.Student,
            Semester = 1
        }
    };

    public static readonly List<Course> Courses = new()
    {
        new Course
        {
            Id = Course1Id,
            Code = "CS-101",
            Title = "Introduction to Programming",
            CreditHours = 4,
            MaxCapacity = 3,
            FacultyId = Faculty1Id,
            EnrolledStudentIds = new List<Guid> { Student1Id, Student2Id }
        },
        new Course
        {
            Id = Course2Id,
            Code = "CS-201",
            Title = "Data Structures",
            CreditHours = 3,
            MaxCapacity = 3,
            FacultyId = Faculty1Id,
            EnrolledStudentIds = new List<Guid> { Student1Id }
        },
        new Course
        {
            Id = Course3Id,
            Code = "MATH-101",
            Title = "Calculus I",
            CreditHours = 4,
            MaxCapacity = 4,
            FacultyId = Faculty2Id,
            EnrolledStudentIds = new List<Guid> { Student2Id, Student3Id }
        },
        new Course
        {
            Id = Course4Id,
            Code = "MATH-201",
            Title = "Linear Algebra",
            CreditHours = 3,
            MaxCapacity = 3,
            FacultyId = Faculty2Id,
            EnrolledStudentIds = new List<Guid>()
        },
        new Course
        {
            Id = Course5Id,
            Code = "CS-301",
            Title = "Database Systems",
            CreditHours = 3,
            MaxCapacity = 3,
            FacultyId = Faculty3Id,
            EnrolledStudentIds = new List<Guid>()
        },
        new Course
        {
            Id = Course6Id,
            Code = "PHYS-101",
            Title = "Physics I",
            CreditHours = 4,
            MaxCapacity = 4,
            FacultyId = Faculty3Id,
            EnrolledStudentIds = new List<Guid> { Student4Id }
        }
    };

    public static readonly List<Enrollment> Enrollments = new()
    {
        new Enrollment
        {
            Id = Enrollment1Id,
            StudentId = Student1Id,
            CourseId = Course1Id,
            Semester = "Spring 2026",
            State = EnrollmentState.Active,
            EnrolledAt = DateTime.Now
        },
        new Enrollment
        {
            Id = Enrollment2Id,
            StudentId = Student1Id,
            CourseId = Course2Id,
            Semester = "Spring 2026",
            State = EnrollmentState.Active,
            EnrolledAt = DateTime.Now
        },
        new Enrollment
        {
            Id = Enrollment3Id,
            StudentId = Student2Id,
            CourseId = Course1Id,
            Semester = "Spring 2026",
            State = EnrollmentState.Active,
            EnrolledAt = DateTime.Now
        },
        new Enrollment
        {
            Id = Enrollment4Id,
            StudentId = Student2Id,
            CourseId = Course3Id,
            Semester = "Spring 2026",
            State = EnrollmentState.Active,
            EnrolledAt = DateTime.Now
        }
    };

    public static readonly List<GradeRecord> GradeRecords = new()
    {
        new GradeRecord
        {
            Id = Grade1Id,
            StudentId = Student1Id,
            CourseId = Course1Id,
            Marks = 88
        },
        new GradeRecord
        {
            Id = Grade2Id,
            StudentId = Student1Id,
            CourseId = Course2Id,
            Marks = 92
        },
        new GradeRecord
        {
            Id = Grade3Id,
            StudentId = Student2Id,
            CourseId = Course1Id,
            Marks = 75
        },
        new GradeRecord
        {
            Id = Grade4Id,
            StudentId = Student2Id,
            CourseId = Course3Id,
            Marks = 82
        }
    };

    public static readonly List<Notification> Notifications = new();
}
