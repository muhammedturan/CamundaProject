using CustomerService.Application.Common.Interfaces;
using CustomerService.Application.Features.Customers.Dtos;
using MediatR;

namespace CustomerService.Application.Features.Customers.Queries.GetCustomerById;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
{
    private readonly IDapperRepository _repository;

    public GetCustomerByIdQueryHandler(IDapperRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.QueryFirstOrDefaultAsync<CustomerDto>(
            @"SELECT ID, NAME, SURNAME, CITIZEN_ID, EMAIL, PHONE, IS_ACTIVE, CREATED_AT
              FROM CUSTOMERS
              WHERE ID = @Id",
            new { request.Id });
    }
}
