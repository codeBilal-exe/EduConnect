using EduConnect.Models.Enums;

namespace EduConnect.Models;

// SRP: Person is only responsible for representing the base user structure
public abstract class Person
{
    public Guid Id { get; set; } = Guid.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Student;

    public abstract string GetRole();
}
