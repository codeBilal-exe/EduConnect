# EduConnect

EduConnect is an academic management portal built with ASP.NET Core 8 and Blazor Interactive Server. It supports three roles: Admin, Faculty, and Student.

The app uses SQL Server through Entity Framework Core. Course enrollment, grade records, notifications, users, students, faculty, and courses are stored in `EduConnectDB`.

## Features

- Role-based login for Admin, Faculty, and Student users.
- Admin dashboard for managing students and courses.
- Faculty dashboard for assigned courses and grade entry.
- Student dashboard for enrollment, enrolled courses, grades, and CGPA.
- Course capacity tracking with Open, AlmostFull, and Full states.
- Notifications for enrollment changes, grade posting, and announcements.
- SQL Server database setup script with seed data.

## Technology Stack

| Area | Technology |
| --- | --- |
| Framework | ASP.NET Core 8 |
| UI | Blazor Interactive Server |
| Database | SQL Server |
| ORM | Entity Framework Core |
| Styling | Bootstrap |
| Language | C# |

## Project Structure

```text
EduConnect/
  Components/
    Auth/                 Route guards and navigation auth helpers
    Layout/               App layouts
    Pages/                Admin, Faculty, Student, login, dashboard pages
    Shared/               Reusable UI components
  Data/
    AppDbContext.cs       EF Core database context
    Entities/             Database-specific user/student/faculty entities
  Interfaces/             Service contracts
  Models/                 Domain models and enums
  Services/               Database-backed business logic
  Program.cs              App startup and dependency injection
EduConnectSQL.sql         SQL Server database setup and seed data
README.md
```

## Prerequisites

- .NET 8 SDK
- SQL Server or SQL Server Express
- `sqlcmd` or SQL Server Management Studio

The current connection string in `EduConnect/appsettings.json` is:

```json
"DefaultConnection": "Server=BEAST\\SQLEXPRESS;Database=EduConnectDB;Trusted_Connection=True;TrustServerCertificate=True;"
```

If your SQL Server instance name is different, update this value before running the app.

## Database Setup

Run the SQL script from the repository root:

```powershell
sqlcmd -S "BEAST\SQLEXPRESS" -E -i EduConnectSQL.sql
```

The script will:

- Create `EduConnectDB` if it does not already exist.
- Drop and recreate the assignment tables.
- Seed admin, faculty, student, course, enrollment, and grade data.

Important: running the script resets the database tables and deletes existing data.

## Run the App

From the repository root:

```powershell
dotnet build EduConnect\EduConnect.sln
dotnet run --project EduConnect\EduConnect.csproj
```

Default local URLs are configured in `EduConnect/Properties/launchSettings.json`:

- `http://localhost:5153`
- `https://localhost:7088`

Open the login page:

```text
http://localhost:5153/login
```

## Default Accounts

| Role | Email | Password |
| --- | --- | --- |
| Admin | `admin@edu.com` | `admin123` |
| Faculty | `faculty1@edu.com` | `faculty123` |
| Faculty | `faculty2@edu.com` | `faculty123` |
| Faculty | `faculty3@edu.com` | `faculty123` |
| Student | `student1@edu.com` | `student123` |
| Student | `student2@edu.com` | `student123` |
| Student | `student3@edu.com` | `student123` |
| Student | `student4@edu.com` | `student123` |

## Database Tables

| Table | Purpose |
| --- | --- |
| `Users` | Admin login accounts |
| `Students` | Student login/profile data |
| `Faculty` | Faculty login/profile data |
| `Courses` | Course catalog and faculty assignment |
| `Enrollments` | Active and dropped student course enrollments |
| `GradeRecords` | Marks for students in courses |
| `Notifications` | Student notifications |

## Main Services

| Service | Responsibility |
| --- | --- |
| `AuthStateService` | Login, logout, current user state |
| `StudentService` | Student CRUD, search, CGPA calculation |
| `CourseService` | Course CRUD, enrollment, drop course |
| `GradeService` | Submit and retrieve grade records |
| `NotificationService` | Send, list, and mark notifications |

Service IDs use SQL integer identity values, not GUIDs.

Example interface methods:

```csharp
public interface ICourseService : IRepository<Course>
{
    List<Course> GetAvailableCourses();
    List<Course> GetCoursesByFaculty(int facultyId);
    bool CourseCodeExists(string code, int? excludeId = null);
    void EnrollStudent(int courseId, int studentId);
    void DropCourse(int courseId, int studentId);
}
```

```csharp
public interface IStudentService : IRepository<Student>
{
    List<Student> Search(string term);
    double ComputeCGPA(int studentId);
}
```

## License

This project is for educational assignment use.
