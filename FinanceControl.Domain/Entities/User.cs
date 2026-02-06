using FinanceControl.Domain.Exceptions;

namespace FinanceControl.Domain.Entities;

public class User : EntityBase
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    
    // Navigation properties
    private readonly List<Account> _accounts = new();
    public IReadOnlyCollection<Account> Accounts => _accounts.AsReadOnly();
    public Category? Categories { get; private set; }
    
    // EF Core
    private User()
    { 
        Name = string.Empty;
        Email = string.Empty;
        PasswordHash = string.Empty;
    }
    
    public User(string name, string email, string passwordHash) 
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("User name cannot be empty");

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email cannot be empty");

        if (!IsValidEmail(email))
            throw new DomainException("Invalid email format");

        Name = name;
        Email = email.ToLowerInvariant();
        PasswordHash = passwordHash;
        SetCreatedBy(Id);
    } 

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("User name cannot be empty");

        Name = newName;
        SetUpdatedAt(Id);
    }

    public void UpdatePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new DomainException("Password hash cannot be empty");

        PasswordHash = newPasswordHash;
        SetUpdatedAt(Id);
    }

    public void RegisterLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        SetUpdatedAt(Id);
    }

    public override void Activate(Guid userId)
    {
        if (IsDeleted)
            throw new DomainException("Cannot activate a deleted user. Restore it first.");

        if (IsActive)
            throw new DomainException("Account is already active");
        
        base.Activate(userId);
    }

    public override void Deactivate(Guid userId)
    {
        if (IsDeleted)
            throw new DomainException("Cannot deactivate a deleted user");
        
        if (!IsActive)
            throw new DomainException("Account is already inactive");

        base.Deactivate(userId);
    }
    
    public override void Delete(Guid userId)
    {
        if (IsDeleted)
            throw new DomainException("User is already deleted");
        
        if (!IsActive)
            throw new DomainException("Cannot delete an inactive user. Activate it first.");
        
        base.Delete(userId);    
    }
    
    public override void Restore(Guid userId)
    {
        if (!IsDeleted)
            throw new DomainException("User is not deleted");

        base.Restore(userId);
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}