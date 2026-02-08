using CustomerService.Domain.Enums;

namespace CustomerService.Application.Features.Accounts.Dtos;

public record AccountDto(
    Guid Id,
    Guid CustomerId,
    string AccountNumber,
    AccountType AccountType,
    decimal Balance,
    string Currency,
    bool IsActive,
    DateTime CreatedAt);
