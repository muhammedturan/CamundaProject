using CustomerService.Application.Features.Customers.Dtos;
using MediatR;

namespace CustomerService.Application.Features.Customers.Queries.GetCustomerList;

public record GetCustomerListQuery : IRequest<IEnumerable<CustomerDto>>;
