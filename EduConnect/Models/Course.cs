using EduConnect.Models.Enums;

namespace EduConnect.Models;

public class Course
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int CreditHours { get; set; } = 0;
    public int MaxCapacity { get; set; } = 0;
    public Guid FacultyId { get; set; } = Guid.Empty;
    public List<Guid> EnrolledStudentIds { get; set; } = new();

    // OCP: Computed property allows new enrollment statuses without changing code
    public EnrollmentStatus EnrollmentStatus
    {
        get
        {
            int enrolled = EnrolledStudentIds.Count;
            if (enrolled >= MaxCapacity)
                return EnrollmentStatus.Full;
            if (enrolled >= MaxCapacity * 0.8)
                return EnrollmentStatus.AlmostFull;
            return EnrollmentStatus.Open;
        }
    }

    public int EnrollmentCount => EnrolledStudentIds.Count;
}
