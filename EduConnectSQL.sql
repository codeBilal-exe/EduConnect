-- ============================================================
--  EduConnect Simple Database Setup
--  Database: EduConnectDB
-- ============================================================

IF DB_ID('EduConnectDB') IS NULL
BEGIN
    CREATE DATABASE EduConnectDB;
END
GO

USE EduConnectDB;
GO

-- Drop tables in safe order
IF OBJECT_ID('Notifications', 'U') IS NOT NULL DROP TABLE Notifications;
IF OBJECT_ID('GradeRecords',  'U') IS NOT NULL DROP TABLE GradeRecords;
IF OBJECT_ID('Enrollments',   'U') IS NOT NULL DROP TABLE Enrollments;
IF OBJECT_ID('Courses',       'U') IS NOT NULL DROP TABLE Courses;
IF OBJECT_ID('Faculty',       'U') IS NOT NULL DROP TABLE Faculty;
IF OBJECT_ID('Students',      'U') IS NOT NULL DROP TABLE Students;
IF OBJECT_ID('Users',         'U') IS NOT NULL DROP TABLE Users;
GO

-- ============================================================
-- 1. USERS  (Admin accounts only)
-- ============================================================
CREATE TABLE Users (
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    FullName     NVARCHAR(200) NOT NULL,
    Email        NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(500) NOT NULL,
    Role         NVARCHAR(20)  NOT NULL  -- 'Admin', 'Faculty', 'Student'
);
GO

-- ============================================================
-- 2. STUDENTS
-- ============================================================
CREATE TABLE Students (
    Id       INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(200) NOT NULL,
    Email    NVARCHAR(255) NOT NULL UNIQUE,
    Password NVARCHAR(500) NOT NULL,
    Semester INT           NOT NULL CHECK (Semester BETWEEN 1 AND 8)
);
GO

-- ============================================================
-- 3. FACULTY
-- ============================================================
CREATE TABLE Faculty (
    Id         INT IDENTITY(1,1) PRIMARY KEY,
    FullName   NVARCHAR(200) NOT NULL,
    Email      NVARCHAR(255) NOT NULL UNIQUE,
    Password   NVARCHAR(500) NOT NULL,
    Department NVARCHAR(200) NOT NULL
);
GO

-- ============================================================
-- 4. COURSES
-- ============================================================
CREATE TABLE Courses (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Code        NVARCHAR(50)  NOT NULL UNIQUE,
    Title       NVARCHAR(300) NOT NULL,
    CreditHours INT           NOT NULL,
    MaxCapacity INT           NOT NULL,
    FacultyId   INT           NOT NULL REFERENCES Faculty(Id)
);
GO

-- ============================================================
-- 5. ENROLLMENTS
-- ============================================================
CREATE TABLE Enrollments (
    Id         INT IDENTITY(1,1) PRIMARY KEY,
    StudentId  INT          NOT NULL REFERENCES Students(Id),
    CourseId   INT          NOT NULL REFERENCES Courses(Id),
    Semester   NVARCHAR(50) NOT NULL,
    State      NVARCHAR(20) NOT NULL DEFAULT 'Active',  -- 'Active' or 'Dropped'
    EnrolledAt DATETIME     NOT NULL DEFAULT GETDATE()
);
GO

-- ============================================================
-- 6. GRADE RECORDS
-- ============================================================
CREATE TABLE GradeRecords (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    StudentId INT   NOT NULL REFERENCES Students(Id),
    CourseId  INT   NOT NULL REFERENCES Courses(Id),
    Marks     FLOAT NOT NULL CHECK (Marks >= 0 AND Marks <= 100)
);
GO

-- ============================================================
-- 7. NOTIFICATIONS
-- ============================================================
CREATE TABLE Notifications (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    StudentId INT            NOT NULL REFERENCES Students(Id),
    Message   NVARCHAR(1000) NOT NULL,
    Type      NVARCHAR(50)   NOT NULL,  -- 'Enrollment', 'GradePosted', 'Announcement'
    IsRead    BIT            NOT NULL DEFAULT 0,
    CreatedAt DATETIME       NOT NULL DEFAULT GETDATE()
);
GO

-- ============================================================
-- SEED DATA
-- ============================================================

-- Admin
INSERT INTO Users (FullName, Email, PasswordHash, Role) VALUES
('Admin User', 'admin@edu.com', 'admin123', 'Admin');

-- Faculty
INSERT INTO Faculty (FullName, Email, Password, Department) VALUES
('Hafiz Obiadullah Khan', 'faculty1@edu.com', 'faculty123', 'Computer Science'),
('Prof. Atif Aslam',      'faculty2@edu.com', 'faculty123', 'Mathematics'),
('Mam Amna Tariq',        'faculty3@edu.com', 'faculty123', 'Mathematics');

-- Students
INSERT INTO Students (FullName, Email, Password, Semester) VALUES
('Muhammad Bilal',    'student1@edu.com', 'student123', 4),
('Usaidullah Rehan',  'student2@edu.com', 'student123', 3),
('Sajjad Ejaz',       'student3@edu.com', 'student123', 2),
('Abdullah Abdullah', 'student4@edu.com', 'student123', 1);

-- Courses  (FacultyId 1,2,3 match the Faculty rows above)
INSERT INTO Courses (Code, Title, CreditHours, MaxCapacity, FacultyId) VALUES
('CS-101',   'Introduction to Programming', 4, 3, 1),
('CS-201',   'Data Structures',             3, 3, 1),
('MATH-101', 'Calculus I',                  4, 4, 2),
('MATH-201', 'Linear Algebra',              3, 3, 2),
('CS-301',   'Database Systems',            3, 3, 3),
('PHYS-101', 'Physics I',                   4, 4, 3);

-- Enrollments  (StudentId / CourseId match identity order above)
INSERT INTO Enrollments (StudentId, CourseId, Semester, State) VALUES
(1, 1, 'Spring 2026', 'Active'),
(1, 2, 'Spring 2026', 'Active'),
(2, 1, 'Spring 2026', 'Active'),
(2, 3, 'Spring 2026', 'Active');

-- Grade Records
INSERT INTO GradeRecords (StudentId, CourseId, Marks) VALUES
(1, 1, 88),
(1, 2, 92),
(2, 1, 75),
(2, 3, 82);

GO

-- ============================================================
-- VERIFY
-- ============================================================
SELECT 'Users'         AS [Table], COUNT(*) AS [Rows] FROM Users
UNION ALL SELECT 'Students',      COUNT(*) FROM Students
UNION ALL SELECT 'Faculty',       COUNT(*) FROM Faculty
UNION ALL SELECT 'Courses',       COUNT(*) FROM Courses
UNION ALL SELECT 'Enrollments',   COUNT(*) FROM Enrollments
UNION ALL SELECT 'GradeRecords',  COUNT(*) FROM GradeRecords
UNION ALL SELECT 'Notifications', COUNT(*) FROM Notifications;
GO
