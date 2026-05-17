namespace EduConnect.Data.Entities;

public class FacultyEntity
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
}
