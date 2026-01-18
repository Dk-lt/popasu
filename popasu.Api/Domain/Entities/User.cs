using Domain.Enums;

namespace Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Role Role { get; private set; }
    public string Login { get; private set; }
    public string Password { get; private set; }

    public User(
        Guid id,
        string name,
        Role role,
        string login,
        string password)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        
        if (string.IsNullOrWhiteSpace(login))
            throw new ArgumentException("Login cannot be null or empty.", nameof(login));
        
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));

        Id = id;
        Name = name;
        Role = role;
        Login = login;
        Password = password;
    }

    public bool Authorize(string login, string password)
    {
        if (string.IsNullOrWhiteSpace(login))
            throw new ArgumentException("Login cannot be null or empty.", nameof(login));
        
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));

        return Login.Equals(login, StringComparison.Ordinal) &&
               Password.Equals(password, StringComparison.Ordinal);
    }

    public List<MaterialItem> ViewMaterialItems()
    {
        // Note: In a complete domain model, User would access material items
        // through ManagementSystem or another aggregate root.
        // This method signature matches the specification.
        return new List<MaterialItem>();
    }
}

