namespace CustomerService.Application.Features.Customers.Dtos;

public record CustomerDto(
    Guid Id,
    string Name,
    string Surname,
    string CitizenId,
    string? Email,
    string? Phone,
    bool IsActive,
    DateTime CreatedAt);
