using EduConnect.Interfaces;
using EduConnect.Models.Enums;

namespace EduConnect.Models;

// LSP: Student can be used anywhere Person is expected
public class Student : Person, IValidatable
{
    public int Semester { get; set; }

    // CGPA is computed - not stored directly. See StudentService.ComputeCGPA()

    public override string GetRole() => "Student";

    public ValidationResult Validate()
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(FullName))
            errors.Add("Full name is required.");

        if (string.IsNullOrWhiteSpace(Email))
            errors.Add("Email is required.");
        else if (!Email.Contains("@"))
            errors.Add("Email format is invalid.");

        if (Semester < 1 || Semester > 8)
            errors.Add("Semester must be between 1 and 8.");

        return new ValidationResult(errors.Count == 0, errors);
    }
}
