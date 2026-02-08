using CustomerService.Core.Entities;

namespace CustomerService.Domain.Aggregates;

public class Customer : AuditEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Surname { get; private set; } = string.Empty;
    public string CitizenId { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public bool IsActive { get; private set; }

    private Customer() { }

    public static Customer Create(
        string name,
        string surname,
        string citizenId,
        string? email = null,
        string? phone = null)
    {
        return new Customer
        {
            Name = name,
            Surname = surname,
            CitizenId = citizenId,
            Email = email,
            Phone = phone,
            IsActive = true
        };
    }

    public void Update(string name, string surname, string? email, string? phone)
    {
        Name = name;
        Surname = surname;
        Email = email;
        Phone = phone;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}
