using EduConnect.Models;
using EduConnect.Models.Enums;

namespace EduConnect.Interfaces;

// SRP: Only manages authentication state
// DIP: Components depend on this abstraction, not concrete implementations
public interface IAuthStateService
{
    Person? CurrentUser { get; }
    event Action? OnAuthStateChanged;

    bool Login(string email, string password);
    void Logout();
    bool IsInRole(UserRole role);
    void RegisterUser(Person person);
}
