using MediatR;

namespace CustomerService.Application.Features.Customers.Commands.DeleteCustomer;

public record DeleteCustomerCommand(Guid Id) : IRequest<bool>;
