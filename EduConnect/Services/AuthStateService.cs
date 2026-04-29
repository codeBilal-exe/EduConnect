using EduConnect.Data;
using EduConnect.Interfaces;
using EduConnect.Models;
using EduConnect.Models.Enums;

namespace EduConnect.Services;

// SRP: Only manages authentication state
// DIP: Implements the IAuthStateService abstraction
public class AuthStateService : IAuthStateService
{
    private Person? _currentUser;
    private readonly List<Person> _users;

    public Person? CurrentUser
    {
        get => _currentUser;
        private set
        {
            _currentUser = value;
            OnAuthStateChanged?.Invoke();
        }
    }

    public event Action? OnAuthStateChanged;

    public AuthStateService()
    {
        _users = SeedData.Users;
    }


    public bool Login(string email, string password)
    {
        var user = _users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);
        if (user != null)
        {
            CurrentUser = user;
            return true;
        }
        return false;
    }

    public void Logout()
    {
        CurrentUser = null;
    }

    public bool IsInRole(UserRole role)
    {
        return CurrentUser?.Role == role;
    }

    public void RegisterUser(Person person)
    {
        if (person != null && !_users.Any(u => u.Id == person.Id || u.Email == person.Email))
        {
            _users.Add(person);
        }
    }

    public IEnumerable<Person> GetAllUsers()
    {
        return _users;
    }
}
