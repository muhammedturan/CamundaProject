using CustomerService.Application.Features.Customers.Dtos;
using MediatR;

namespace CustomerService.Application.Features.Customers.Queries.GetCustomerByCitizenId;

public record GetCustomerByCitizenIdQuery(string CitizenId) : IRequest<CustomerDto?>;
