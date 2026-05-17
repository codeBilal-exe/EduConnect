using EduConnect.Models.Enums;

namespace EduConnect.Models;

public class Enrollment
{
    public int Id { get; set; } = 0;
    public required int StudentId { get; set; }
    public required int CourseId { get; set; }
    public required string Semester { get; set; }
    public required EnrollmentState State { get; set; }
    public required DateTime EnrolledAt { get; set; }
}
