using CustomerService.Application.Features.Accounts.Dtos;
using MediatR;

namespace CustomerService.Application.Features.Accounts.Queries.GetAccountsByCustomerId;

public record GetAccountsByCustomerIdQuery(Guid CustomerId) : IRequest<IEnumerable<AccountDto>>;
