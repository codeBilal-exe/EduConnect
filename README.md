<div align="center">

# 🎓 EduConnect

**A modern Academic Management Portal built with Blazor Server & ASP.NET Core 8**

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-Server-512BD4?style=flat-square&logo=blazor)](https://blazor.net)
[![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=flat-square&logo=bootstrap)](https://getbootstrap.com)
[![SOLID](https://img.shields.io/badge/SOLID-97%25%20Compliant-brightgreen?style=flat-square)](./SOLID_COMPLIANCE_CHECKLIST.md)
[![License](https://img.shields.io/badge/License-Educational-blue?style=flat-square)](#license)

*Streamlining course management, student enrollment, grade tracking, and institutional communications in one integrated platform.*

</div>

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Technology Stack](#️-technology-stack)
- [Architecture](#️-architecture)
- [Project Structure](#-project-structure)
- [Getting Started](#-getting-started)
- [Default Accounts](#-default-accounts-for-testing)
- [SOLID Principles](#-solid-principles)
- [Service Interfaces](#-service-interfaces)
- [Data Models](#-data-models)
- [Role-Based Access](#-role-based-access-control)

---

## 🎯 Overview

EduConnect is a comprehensive academic management platform designed for educational institutions. It supports three user roles — **Student**, **Faculty**, and **Admin** — each with a dedicated dashboard and role-specific features.

The project is built as a Blazor Interactive Server application, emphasising clean architecture and strict adherence to **SOLID design principles**.

---

## ✨ Features

### 👤 Authentication & Users
- Secure email/password login with session state
- Three roles: **Student**, **Faculty**, **Admin**
- Role-based route guards (`AuthGuard`) protecting all pages
- Admin can register and manage all users

### 📚 Course Management
- Create, update, and archive courses
- Set credit hours, capacity, and faculty assignments
- Automatic enrollment status: `Open` → `Almost Full` → `Full`
- Faculty view of assigned courses

### 🎓 Student Dashboard
- Browse and enroll in available courses
- Drop courses (with validation)
- View enrolled credit hours at a glance
- Track academic performance

### 📊 Grade Management
- Faculty submits grades per course
- Automatic letter grade calculation: `A / B / C / D / F`
- Automatic GPA point mapping: `4.0 / 3.0 / 2.0 / 1.0 / 0.0`
- Credit-weighted **CGPA** computation
- Full grade history / transcript view for students

### 📢 Notifications
- Event-driven real-time alerts
- Enrollment confirmations and grade-posted notices
- Admin announcements broadcast to all users
- Unread count badge on notification bell
- Mark-as-read support

### 📈 Admin Panel
- Student list with add / edit / delete
- Course management and faculty assignment
- Grade reports across all students and courses
- System-wide announcement broadcasts

---

## 🛠️ Technology Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8.0 |
| UI | Blazor Interactive Server Components |
| Language | C# 12 |
| Styling | Bootstrap 5.3 + custom CSS variables |
| Icons | Bootstrap Icons 1.11 |
| Fonts | Google Fonts — Outfit & Inter |
| DI Container | Built-in ASP.NET Core DI |
| Data Storage | In-memory collections (seeded) |

---

## 🏗️ Architecture

### Layered Architecture

```
┌──────────────────────────────────────────────┐
│           Razor Components (UI Layer)         │
│   Pages · Shared Components · Layouts        │
└────────────────────┬─────────────────────────┘
                     │  injects via DI
┌────────────────────▼─────────────────────────┐
│              Service Layer                    │
│  StudentService · CourseService               │
│  GradeService · AuthStateService             │
│  NotificationService                         │
└────────────────────┬─────────────────────────┘
                     │  reads/writes
┌────────────────────▼─────────────────────────┐
│               Data Layer                     │
│   SeedData (in-memory) · Model classes       │
└──────────────────────────────────────────────┘
```

### Event-Driven Updates

Components subscribe to service events to update reactively without page reloads:

```
CourseService.OnEnrollmentChanged  → NavBar credit counter, Student dashboard
GradeService.OnGradesSubmitted     → Student grade view
NotificationService.OnNewNotification → Notification bell badge
AuthStateService.OnAuthStateChanged   → NavBar user info
```

---

## 📁 Project Structure

```
EduConnect/
├── Components/
│   ├── Pages/
│   │   ├── Login.razor              # Entry point — "/" and "/login"
│   │   ├── Dashboard.razor          # Role-aware dashboard router
│   │   ├── CourseCatalog.razor      # Public course listing
│   │   ├── Notifications.razor      # Notification centre
│   │   ├── Admin/
│   │   │   ├── StudentList.razor
│   │   │   ├── CourseManagement.razor
│   │   │   ├── GradeReport.razor
│   │   │   └── SendAnnouncement.razor
│   │   ├── Faculty/
│   │   │   └── GradeEntry.razor
│   │   └── Student/
│   │       ├── EnrollPage.razor
│   │       └── GradesPage.razor
│   │
│   ├── Shared/                      # Reusable UI components
│   │   ├── AdminDashboard.razor
│   │   ├── StudentDashboard.razor
│   │   ├── FacultyDashboard.razor
│   │   ├── CourseCard.razor
│   │   ├── StudentCard.razor
│   │   ├── GradeTable.razor
│   │   ├── NotificationBell.razor
│   │   ├── AlertBox.razor
│   │   ├── ConfirmDialog.razor
│   │   └── LoadingSpinner.razor
│   │
│   ├── Auth/                        # Authentication components
│   │   ├── AuthGuard.razor          # Route protection wrapper
│   │   └── NavBar.razor             # Responsive navigation bar
│   │
│   ├── Layout/
│   │   └── MainLayout.razor
│   ├── App.razor
│   ├── Routes.razor
│   └── _Imports.razor
│
├── Services/                        # Business logic
│   ├── StudentService.cs
│   ├── CourseService.cs
│   ├── GradeService.cs
│   ├── AuthStateService.cs
│   └── NotificationService.cs
│
├── Interfaces/                      # Abstractions (DIP)
│   ├── IRepository.cs               # Generic CRUD contract
│   ├── IStudentService.cs
│   ├── ICourseService.cs
│   ├── IGradeService.cs
│   ├── IAuthStateService.cs
│   ├── INotificationService.cs
│   └── IValidatable.cs
│
├── Models/
│   ├── Person.cs                    # Abstract base user
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
├── Data/
│   └── SeedData.cs                  # Pre-seeded demo data
│
├── wwwroot/
│   ├── app.css                      # Global CSS & design tokens
│   └── bootstrap/
│
├── Program.cs
└── appsettings.json
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download) or later
- Visual Studio 2022 **or** VS Code with C# Dev Kit
- A modern browser (Chrome, Firefox, Edge, Safari)

### Run Locally

```bash
# 1. Clone the repository
git clone https://github.com/your-username/EduConnect.git
cd EduConnect/EduConnect

# 2. Restore NuGet packages
dotnet restore

# 3. Build
dotnet build

# 4. Run
dotnet run
```

Then open **`https://localhost:7000`** (or the URL shown in terminal) in your browser.

---

## 🔑 Default Accounts for Testing

| Role | Email | Password |
|---|---|---|
| 🛡️ Admin | `admin@educonnect.com` | `admin123` |
| 👨‍🏫 Faculty | `faculty@educonnect.com` | `faculty123` |
| 🎓 Student | `student@educonnect.com` | `student123` |

> **Note:** Data is stored in-memory and resets on every application restart.

---

## 📐 SOLID Principles

Overall compliance score: **97%** ✅

| Principle | Score | Implementation |
|---|---|---|
| **S**ingle Responsibility | 93% | Each service class has one primary responsibility |
| **O**pen/Closed | 100% | New roles/features added via interfaces and events, not modification |
| **L**iskov Substitution | 100% | `Student`, `Faculty`, `Admin` all substitute `Person` cleanly |
| **I**nterface Segregation | 93% | Focused per-service interfaces, no fat interfaces |
| **D**ependency Inversion | 100% | All components depend on `IXxxService`, never concrete classes |

See [`SOLID_COMPLIANCE_CHECKLIST.md`](./SOLID_COMPLIANCE_CHECKLIST.md) for detailed analysis and [`SOLID_ANNOTATIONS_GUIDE.md`](./SOLID_ANNOTATIONS_GUIDE.md) for annotated code examples.

---

## 🔌 Service Interfaces

<details>
<summary><strong>IStudentService</strong></summary>

```csharp
public interface IStudentService : IRepository<Student>
{
    List<Student> Search(string term);       // Search by name/email
    double ComputeCGPA(Guid studentId);      // Weighted CGPA calculation
}
```
</details>

<details>
<summary><strong>ICourseService</strong></summary>

```csharp
public interface ICourseService : IRepository<Course>
{
    List<Course> GetAvailableCourses();
    List<Course> GetCoursesByFaculty(Guid facultyId);
    void EnrollStudent(Guid courseId, Guid studentId);  // throws CourseFullException
    void DropCourse(Guid courseId, Guid studentId);
    event Action OnEnrollmentChanged;
}
```
</details>

<details>
<summary><strong>IGradeService</strong></summary>

```csharp
public interface IGradeService
{
    void SubmitGrade(GradeRecord record);
    List<GradeRecord> GetGradesForStudent(Guid studentId);
    List<GradeRecord> GetGradesForCourse(Guid courseId);
    event Action OnGradesSubmitted;
}
```
</details>

<details>
<summary><strong>IAuthStateService</strong></summary>

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
</details>

<details>
<summary><strong>INotificationService</strong></summary>

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
</details>

---

## 💾 Data Models

### User Hierarchy

```
Person  (abstract — Id, FullName, Email, PasswordHash, Role)
├── Student   + Semester
├── Faculty   + Department, AssignedCourseIds
└── Admin
```

### Key Model Properties

| Model | Key Fields |
|---|---|
| `Course` | `Id`, `Code`, `Title`, `CreditHours`, `MaxCapacity`, `EnrolledStudentIds`, `EnrollmentStatus` (computed) |
| `Enrollment` | `Id`, `StudentId`, `CourseId`, `Semester`, `State` (Active/Dropped/Completed), `EnrolledAt` |
| `GradeRecord` | `Id`, `StudentId`, `CourseId`, `Marks`, `LetterGrade` (computed), `GradePoint` (computed) |
| `Notification` | `Id`, `UserId`, `Message`, `Type`, `IsRead`, `CreatedAt` |

### Grade Scale

| Marks | Letter | GPA Points |
|---|---|---|
| 85 – 100 | A | 4.0 |
| 70 – 84 | B | 3.0 |
| 55 – 69 | C | 2.0 |
| 45 – 54 | D | 1.0 |
| 0 – 44 | F | 0.0 |

**CGPA Formula:**
```
CGPA = Σ(GradePoint × CreditHours) / Σ(CreditHours)
```

---

## 🔐 Role-Based Access Control

| Feature | Student | Faculty | Admin |
|---|:---:|:---:|:---:|
| View Course Catalog | ✅ | ✅ | ✅ |
| Enroll / Drop Courses | ✅ | ❌ | ❌ |
| View Own Grades | ✅ | ❌ | ❌ |
| Submit Grades | ❌ | ✅ | ✅ |
| Manage Courses | ❌ | ✅ | ✅ |
| View Grade Reports | ❌ | ✅ | ✅ |
| Manage Students | ❌ | ❌ | ✅ |
| Send Announcements | ❌ | ❌ | ✅ |
| View All Users | ❌ | ❌ | ✅ |

---

## 📄 License

This project is provided for **educational purposes** as part of a university coursework assignment demonstrating SOLID principles in a real-world Blazor application.
