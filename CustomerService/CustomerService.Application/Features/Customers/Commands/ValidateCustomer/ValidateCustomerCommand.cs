using MediatR;

namespace CustomerService.Application.Features.Customers.Commands.ValidateCustomer;

public record ValidateCustomerCommand(string Name, string Surname, string CitizenId) : IRequest<ValidateCustomerResult>;

public record ValidateCustomerResult(bool IsValid);
