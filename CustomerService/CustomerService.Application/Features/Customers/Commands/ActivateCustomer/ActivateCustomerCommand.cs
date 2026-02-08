using MediatR;

namespace CustomerService.Application.Features.Customers.Commands.ActivateCustomer;

public record ActivateCustomerCommand(Guid CustomerId) : IRequest<bool>;
