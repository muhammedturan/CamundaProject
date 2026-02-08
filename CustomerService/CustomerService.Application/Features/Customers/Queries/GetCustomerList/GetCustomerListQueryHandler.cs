using CustomerService.Application.Common.Interfaces;
using CustomerService.Application.Features.Customers.Dtos;
using MediatR;

namespace CustomerService.Application.Features.Customers.Queries.GetCustomerList;

public class GetCustomerListQueryHandler : IRequestHandler<GetCustomerListQuery, IEnumerable<CustomerDto>>
{
    private readonly IDapperRepository _repository;

    public GetCustomerListQueryHandler(IDapperRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CustomerDto>> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
    {
        return await _repository.QueryAsync<CustomerDto>(
            @"SELECT ID, NAME, SURNAME, CITIZEN_ID, EMAIL, PHONE, IS_ACTIVE, CREATED_AT
              FROM CUSTOMERS
              ORDER BY CREATED_AT DESC");
    }
}
