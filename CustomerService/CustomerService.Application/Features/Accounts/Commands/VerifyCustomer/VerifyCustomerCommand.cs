using MediatR;

namespace CustomerService.Application.Features.Accounts.Commands.VerifyCustomer;

public record VerifyCustomerCommand(string CitizenId) : IRequest<VerifyCustomerResult>;

public record VerifyCustomerResult(bool CustomerFound, Guid CustomerId);
