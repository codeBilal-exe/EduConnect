using EduConnect.Data;
using EduConnect.Interfaces;
using EduConnect.Models;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Services;

public class GradeService : IGradeService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    public event Action? OnGradesSubmitted;

    public GradeService(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public void SubmitGrade(GradeRecord record)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var existing = context.GradeRecords
            .FirstOrDefault(g => g.StudentId == record.StudentId && g.CourseId == record.CourseId);

        if (existing != null)
        {
            existing.Marks = record.Marks;
        }
        else
        {
            context.GradeRecords.Add(new GradeRecord
            {
                StudentId = record.StudentId,
                CourseId = record.CourseId,
                Marks = record.Marks
            });
        }

        context.SaveChanges();
        OnGradesSubmitted?.Invoke();
    }

    public List<GradeRecord> GetGradesForStudent(int studentId)
    {
        using var context = _dbContextFactory.CreateDbContext();
        return context.GradeRecords
            .Where(g => g.StudentId == studentId)
            .AsNoTracking()
            .ToList();
    }

    public List<GradeRecord> GetGradesForCourse(int courseId)
    {
        using var context = _dbContextFactory.CreateDbContext();
        return context.GradeRecords
            .Where(g => g.CourseId == courseId)
            .AsNoTracking()
            .ToList();
    }
}

