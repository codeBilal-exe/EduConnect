using EduConnect.Models.Enums;

namespace EduConnect.Models;

// LSP: Admin can be used anywhere Person is expected
public class Admin : Person
{
    public override string GetRole() => "Admin";
}
