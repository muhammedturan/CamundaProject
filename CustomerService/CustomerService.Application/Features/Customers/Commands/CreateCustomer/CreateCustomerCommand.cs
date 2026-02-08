using MediatR;

namespace CustomerService.Application.Features.Customers.Commands.CreateCustomer;

public record CreateCustomerCommand(
    string Name,
    string Surname,
    string CitizenId,
    string? Email,
    string? Phone) : IRequest<Guid>;
