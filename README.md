# 🎓 EduConnect

A modern, full-featured **Educational Management System** built with ASP.NET Core and Blazor. EduConnect streamlines course management, student enrollment, grade tracking, and institutional communications in one integrated platform.

---

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [SOLID Principles](#solid-principles)
- [API Overview](#api-overview)
- [Database Models](#database-models)
- [Authentication & Authorization](#authentication--authorization)
- [Contributing](#contributing)
- [License](#license)

---

## 🎯 Overview

EduConnect is a comprehensive educational management platform designed for educational institutions to:

- **Manage Courses** - Create, update, and organize course offerings with capacity management
- **Handle Enrollments** - Streamline student enrollment with automatic validation and conflict detection
- **Track Grades** - Submit, manage, and analyze student grades with automatic GPA calculation
- **Manage Users** - Support multiple user roles (Students, Faculty, Admins) with role-based access control
- **Send Notifications** - Real-time notifications for enrollment changes, grade submissions, and announcements

The system emphasizes clean architecture, maintainability, and adherence to SOLID design principles.

---

## ✨ Features

### 👤 User Management

- **Multiple Roles**: Student, Faculty, Admin
- **Role-Based Access Control**: Each role has specific permissions and features
- **User Authentication**: Secure login with email and password
- **User Registration**: Faculty and Admin can register new users

### 📚 Course Management (Faculty & Admin)

- Create and manage course offerings
- Set course capacity and credit hours
- Assign faculty to courses
- Track enrollment status (Open, Almost Full, Full)
- View course enrollment lists

### 🎓 Student Dashboard

- View enrolled courses
- Track grades and academic performance
- Calculate and monitor GPA/CGPA
- View course details and faculty information
- Drop courses (when permitted)

### 📊 Grade Management

- Submit grades for courses (Faculty)
- View grade history and transcripts (Student)
- Automatic letter grade calculation (A, B, C, D, F)
- CGPA computation based on course credit hours
- Grade notifications

### 📢 Notifications

- Real-time enrollment alerts
- Grade submission notifications
- Announcement broadcasts
- Unread notification tracking
- Mark notifications as read

### 📈 Admin Features

- Student management (add, edit, delete)
- Course management and assignments
- Grade reporting and analytics
- View all users and their roles
- Send system announcements

---

## 🛠️ Technology Stack

### Backend

- **Framework**: ASP.NET Core 8.0
- **UI**: Blazor Interactive Server Components
- **Language**: C# 12
- **Dependency Injection**: Built-in DI container
- **Data**: In-memory collections (can be extended with Entity Framework)

### Frontend

- **Blazor** - Server-side interactive components
- **Bootstrap 5** - Responsive UI framework
- **CSS 3** - Modern styling

### Architecture Pattern

- **Service Layer**: Business logic separated from components
- **Repository Pattern**: Data access abstraction
- **Dependency Injection**: Loose coupling and testability
- **SOLID Principles**: Clean, maintainable code

---

## 🏗️ Architecture

### Layered Architecture

```
┌─────────────────────────────────────────┐
│         Razor Components (UI)           │
│  - Pages, Shared Components, Layouts    │
└─────────────────────┬───────────────────┘
                      │
┌─────────────────────▼───────────────────┐
│         Service Layer                   │
│  - StudentService                       │
│  - CourseService                        │
│  - GradeService                         │
│  - AuthStateService                     │
│  - NotificationService                  │
└─────────────────────┬───────────────────┘
                      │
┌─────────────────────▼───────────────────┐
│         Data Layer                      │
│  - SeedData (in-memory database)        │
│  - Models (Entity classes)              │
└─────────────────────────────────────────┘
```

### Design Principles

- **SOLID Compliance**: 97% adherence to SOLID principles
- **Single Responsibility**: Each service has one reason to change
- **Open/Closed**: Extensible through interfaces and events
- **Liskov Substitution**: Proper inheritance hierarchies
- **Interface Segregation**: Focused, minimal interfaces
- **Dependency Inversion**: Depend on abstractions, not implementations

---

## 📁 Project Structure

```
EduConnect/
├── Components/                      # Blazor components and pages
│   ├── Pages/                       # Application pages
│   │   ├── Login.razor              # Login page
│   │   ├── Dashboard.razor          # Main dashboard
│   │   ├── CourseCatalog.razor      # Course listing
│   │   ├── Notifications.razor      # Notifications center
│   │   └── Admin/                   # Admin-only pages
│   │       ├── StudentList.razor
│   │       ├── CourseManagement.razor
│   │       ├── GradeReport.razor
│   │       └── SendAnnouncement.razor
│   ├── Shared/                      # Shared UI components
│   │   ├── AdminDashboard.razor
│   │   ├── StudentDashboard.razor
│   │   ├── FacultyDashboard.razor
│   │   ├── CourseCard.razor
│   │   ├── StudentCard.razor
│   │   ├── AlertBox.razor
│   │   └── ConfirmDialog.razor
│   ├── Auth/                        # Authentication components
│   │   ├── AuthGuard.razor
│   │   ├── NavBar.razor
│   │   └── RedirectToLogin.razor
│   ├── Layout/                      # Layout templates
│   │   ├── MainLayout.razor
│   │   └── MainLayout.razor.css
│   ├── App.razor                    # App shell
│   ├── Routes.razor                 # Route definitions
│   └── _Imports.razor               # Global imports
│
├── Services/                        # Business logic layer
│   ├── StudentService.cs            # Student management
│   ├── CourseService.cs             # Course management
│   ├── GradeService.cs              # Grade management
│   ├── AuthStateService.cs          # Authentication state
│   └── NotificationService.cs       # Notifications
│
├── Interfaces/                      # Service contracts
│   ├── IRepository.cs               # Generic repository
│   ├── IStudentService.cs
│   ├── ICourseService.cs
│   ├── IGradeService.cs
│   ├── IAuthStateService.cs
│   ├── INotificationService.cs
│   └── IValidatable.cs
│
├── Models/                          # Data models
│   ├── Person.cs                    # Base user class
│   ├── Student.cs
│   ├── Faculty.cs
│   ├── Admin.cs
│   ├── Course.cs
│   ├── Enrollment.cs
│   ├── GradeRecord.cs
│   ├── Notification.cs
│   ├── Enums/
│   │   ├── UserRole.cs
│   │   ├── EnrollmentStatus.cs
│   │   ├── EnrollmentState.cs
│   │   ├── NotificationType.cs
│   │   └── AlertType.cs
│   └── Exceptions/
│       ├── CourseFullException.cs
│       └── StudentHasActiveEnrollmentsException.cs
│
├── Data/                            # Data access layer
│   └── SeedData.cs                  # Sample data initialization
│
├── wwwroot/                         # Static files
│   ├── app.css
│   └── bootstrap/
│
├── Properties/                      # Configuration
│   └── launchSettings.json
│
├── Program.cs                       # Application entry point
├── EduConnect.csproj               # Project file
├── appsettings.json                # Configuration
└── README.md                        # This file
```

---

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 / VS Code (recommended)
- Browser with WebSocket support (Chrome, Firefox, Edge, Safari)

### Installation

1. **Clone the repository**

   ```bash
   cd EduConnect
   ```

2. **Restore dependencies**

   ```bash
   dotnet restore
   ```

3. **Build the project**

   ```bash
   dotnet build
   ```

4. **Run the application**

   ```bash
   dotnet run
   ```

5. **Open in browser**
   ```
   https://localhost:7000
   ```

### Default Users for Testing

**Admin Account:**

- Email: admin@educonnect.com
- Password: admin123

**Faculty Account:**

- Email: faculty@educonnect.com
- Password: faculty123

**Student Account:**

- Email: student@educonnect.com
- Password: student123

---

## 📐 SOLID Principles

This project exemplifies SOLID design principles for educational purposes. See [SOLID_COMPLIANCE_CHECKLIST.md](SOLID_COMPLIANCE_CHECKLIST.md) for detailed analysis.

### Compliance Score: **97%** ✅

- **S**ingle Responsibility: 93% - Each service has clear responsibilities
- **O**pen/Closed: 100% - Extensible through interfaces and events
- **L**iskov Substitution: 100% - Proper inheritance hierarchies (Student, Faculty, Admin → Person)
- **I**nterface Segregation: 93% - Focused service interfaces
- **D**ependency Inversion: 100% - Interface-based dependency injection

---

## 🔌 API Overview

### StudentService

```csharp
public interface IStudentService : IRepository<Student>
{
    List<Student> Search(string term);           // Search students by name
    double ComputeCGPA(Guid studentId);          // Calculate cumulative GPA
}
```

### CourseService

```csharp
public interface ICourseService : IRepository<Course>
{
    List<Course> GetAvailableCourses();          // Get non-full courses
    List<Course> GetCoursesByFaculty(Guid facultyId);
    void EnrollStudent(Guid courseId, Guid studentId);
    void DropCourse(Guid courseId, Guid studentId);
    event Action OnEnrollmentChanged;
}
```

### GradeService

```csharp
public interface IGradeService
{
    void SubmitGrade(GradeRecord record);
    List<GradeRecord> GetGradesForStudent(Guid studentId);
    List<GradeRecord> GetGradesForCourse(Guid courseId);
    event Action OnGradesSubmitted;
}
```

### AuthStateService

```csharp
public interface IAuthStateService
{
    Person? CurrentUser { get; }
    bool Login(string email, string password);
    void Logout();
    bool IsInRole(UserRole role);
    void RegisterUser(Person person);
    IEnumerable<Person> GetAllUsers();
    event Action OnAuthStateChanged;
}
```

### NotificationService

```csharp
public interface INotificationService
{
    void SendNotification(Notification notification);
    List<Notification> GetNotificationsForUser(Guid userId);
    void MarkAsRead(Guid notificationId);
    int GetUnreadCount(Guid userId);
    event Action<Notification> OnNewNotification;
}
```

---

## 💾 Database Models

### User Hierarchy

```
Person (abstract)
├── Student
│   └── Semester: int
├── Faculty
│   └── Department: string
│   └── AssignedCourseIds: List<Guid>
└── Admin
```

### Core Models

**Course**

- Id, Code, Title, CreditHours
- MaxCapacity, EnrolledStudentIds
- EnrollmentStatus (computed: Open/AlmostFull/Full)

**Enrollment**

- Id, StudentId, CourseId
- Semester, State (Active/Dropped/Completed)
- EnrolledAt timestamp

**GradeRecord**

- Id, StudentId, CourseId, Marks
- LetterGrade (computed: A/B/C/D/F)
- GradePoint (computed: 4.0/3.0/2.0/1.0/0.0)

**Notification**

- Id, UserId, Message, Type
- IsRead, CreatedAt timestamp

---

## 🔐 Authentication & Authorization

### Role-Based Access Control

| Feature            | Student | Faculty | Admin |
| ------------------ | ------- | ------- | ----- |
| View Courses       | ✅      | ✅      | ✅    |
| Enroll in Course   | ✅      | ❌      | ❌    |
| Submit Grades      | ❌      | ✅      | ✅    |
| View Grades        | ✅      | ✅      | ✅    |
| Manage Users       | ❌      | ❌      | ✅    |
| Manage Courses     | ❌      | ✅      | ✅    |
| Send Announcements | ❌      | ❌      | ✅    |
| View Reports       | ❌      | ✅      | ✅    |

### Authentication Flow

1. User navigates to login page
2. Enters email and password
3. AuthStateService validates credentials
4. Session state updated (cascading parameter)
5. Navigation guards prevent unauthorized access
6. User redirected to appropriate dashboard

---

## 📊 Key Features in Detail

### Grade Calculation

- **Letter Grade**: Based on marks (85+=A, 70+=B, 55+=C, 45+=D, <45=F)
- **Grade Points**: Converted to GPA scale (A=4.0, B=3.0, C=2.0, D=1.0, F=0.0)
- **CGPA**: Weighted average across all courses using credit hours
  ```
  CGPA = Σ(GradePoint × CreditHours) / Σ(CreditHours)
  ```

### Enrollment Management

- Automatic validation of course capacity
- Prevents double enrollment in same course
- Tracks enrollment state (Active/Dropped/Completed)
- Cascading updates through event notifications

### Real-Time Notifications

- Event-driven architecture for real-time updates
- Subscribers notified of enrollment and grade changes
- UI automatically refreshes via Blazor events
- Unread count tracking for notification bell

---

## 🧪 Testing

### Unit Testing Strategy

The clean architecture supports easy unit testing:

```csharp
// Example: Mock dependencies
var mockGradeService = new Mock<IGradeService>();
var mockCourseService = new Mock<ICourseService>();
var studentService = new StudentService(mockGradeService.Object, mockCourseService.Object);

// Test CGPA calculation
var cgpa = studentService.ComputeCGPA(studentId);
Assert.Equal(3.5, cgpa);
```

### Integration Testing

- Service layer integration tests
- Component integration with services
- End-to-end user workflows

---

## 🚀 Performance Considerations

### Current Limitations

- **In-Memory Storage**: Data lost on application restart
- **No Async/Await**: Synchronous operations
- **No Caching**: All queries traverse collections

### Future Improvements

1. **Implement Entity Framework Core** - Database persistence
2. **Add Async/Await Patterns** - Scalability
3. **Implement Caching** - Query optimization
4. **Add Query Indexing** - Faster searches
5. **Load Testing** - Performance benchmarks

---

## 📝 Configuration Files

### appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

### appsettings.Development.json

Enhanced logging and debug settings for development environment.

---

## 🤝 Contributing

### Code Style

- Follow C# naming conventions (PascalCase for public, camelCase for private)
- Use meaningful variable names
- Add XML documentation to public methods
- Keep methods focused and testable

### SOLID Principles

All contributions must maintain or improve SOLID compliance:

- Single responsibility of each class
- Open for extension, closed for modification
- Proper use of abstraction and inheritance
- Interface segregation
- Dependency injection

### Pull Request Process

1. Create feature branch: `git checkout -b feature/description`
2. Make changes following code style
3. Update documentation
4. Ensure all tests pass
5. Submit pull request with clear description

---

## 📚 Learning Resources

### SOLID Principles in This Project

See [SOLID_ANNOTATIONS_GUIDE.md](SOLID_ANNOTATIONS_GUIDE.md) for code examples demonstrating each SOLID principle with actual project code.

See [SOLID_QUICK_REFERENCE.md](SOLID_QUICK_REFERENCE.md) for a quick reference guide.

### ASP.NET Core & Blazor

- [Microsoft Blazor Documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/)
- [Dependency Injection in .NET](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)

---

## 📄 License

This project is provided for educational purposes as part of the EduConnect learning system.

---

## 📧 Support

For issues, questions, or suggestions:

- Review the documentation in the project
- Check SOLID compliance checklist for architecture questions
- Refer to the code annotations guide for implementation examples

---

## 🎉 Thank You

Thank you for using EduConnect! This project demonstrates modern software architecture principles applied to a real-world educational management system.

**Key Achievements:**

- ✅ 97% SOLID Compliance
- ✅ Clean, maintainable codebase
- ✅ Extensible service architecture
- ✅ Role-based access control
- ✅ Event-driven notifications
- ✅ Comprehensive grade management

