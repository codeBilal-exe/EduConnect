using EduConnect.Models.Enums;

namespace EduConnect.Models;

public class Enrollment
{
    public required Guid Id { get; init; }
    public required Guid StudentId { get; set; }
    public required Guid CourseId { get; set; }
    public required string Semester { get; set; }
    public required EnrollmentState State { get; set; }
    public required DateTime EnrolledAt { get; set; }
}
