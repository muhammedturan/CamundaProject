using CustomerService.Application.Features.Customers.Dtos;
using MediatR;

namespace CustomerService.Application.Features.Customers.Queries.GetCustomerById;

public record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerDto?>;
