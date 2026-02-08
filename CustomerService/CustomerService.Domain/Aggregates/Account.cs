using CustomerService.Core.Entities;
using CustomerService.Domain.Enums;

namespace CustomerService.Domain.Aggregates;

public class Account : AuditEntity
{
    public Guid CustomerId { get; private set; }
    public string AccountNumber { get; private set; } = string.Empty;
    public AccountType AccountType { get; private set; }
    public decimal Balance { get; private set; }
    public string Currency { get; private set; } = "TRY";
    public bool IsActive { get; private set; }

    private Account() { }

    public static Account Create(
        Guid customerId,
        AccountType accountType,
        string currency = "TRY")
    {
        return new Account
        {
            CustomerId = customerId,
            AccountNumber = GenerateAccountNumber(),
            AccountType = accountType,
            Balance = 0,
            Currency = currency,
            IsActive = true
        };
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Yatırılacak tutar sıfırdan büyük olmalı", nameof(amount));

        Balance += amount;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Çekilecek tutar sıfırdan büyük olmalı", nameof(amount));

        if (Balance < amount)
            throw new InvalidOperationException("Yetersiz bakiye");

        Balance -= amount;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    private static string GenerateAccountNumber()
    {
        var random = new Random();
        return $"TR{random.NextInt64(1000000000000000, 9999999999999999)}";
    }
}
