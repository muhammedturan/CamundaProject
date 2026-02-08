using MediatR;

namespace CustomerService.Application.Features.Customers.Commands.UpdateCustomer;

public record UpdateCustomerCommand(
    Guid Id,
    string Name,
    string Surname,
    string? Email,
    string? Phone) : IRequest<bool>;
