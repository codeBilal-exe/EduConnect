using EduConnect.Data.Entities;
using EduConnect.Models;
using EduConnect.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<StudentEntity> Students => Set<StudentEntity>();
    public DbSet<FacultyEntity> Faculty => Set<FacultyEntity>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<GradeRecord> GradeRecords => Set<GradeRecord>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentEntity>()
            .Property(s => s.PasswordHash)
            .HasColumnName("Password");

        modelBuilder.Entity<FacultyEntity>()
            .Property(f => f.PasswordHash)
            .HasColumnName("Password");

        modelBuilder.Entity<Enrollment>()
            .Property(e => e.State)
            .HasConversion<string>();

        modelBuilder.Entity<Notification>()
            .Property(n => n.Type)
            .HasConversion<string>();

        modelBuilder.Entity<Course>()
            .Ignore(c => c.EnrolledStudentIds)
            .Ignore(c => c.EnrollmentStatus)
            .Ignore(c => c.EnrollmentCount);
    }
}
