using EduConnect.Models.Enums;

namespace EduConnect.Models;

// LSP: Faculty can be used anywhere Person is expected
public class Faculty : Person
{
    public string Department { get; set; } = string.Empty;

    // ISP: Faculty manages their own assigned course IDs, not grades
    public List<int> AssignedCourseIds { get; set; } = new();

    public override string GetRole() => "Faculty";
}
