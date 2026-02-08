using CustomerService.Application.Common.Interfaces;
using CustomerService.Application.Features.Customers.Dtos;
using MediatR;

namespace CustomerService.Application.Features.Customers.Queries.GetCustomerByCitizenId;

public class GetCustomerByCitizenIdQueryHandler : IRequestHandler<GetCustomerByCitizenIdQuery, CustomerDto?>
{
    private readonly IDapperRepository _repository;

    public GetCustomerByCitizenIdQueryHandler(IDapperRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomerDto?> Handle(GetCustomerByCitizenIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.QueryFirstOrDefaultAsync<CustomerDto>(
            @"SELECT ID, NAME, SURNAME, CITIZEN_ID, EMAIL, PHONE, IS_ACTIVE, CREATED_AT
              FROM CUSTOMERS
              WHERE CITIZEN_ID = @CitizenId",
            new { request.CitizenId });
    }
}
