using CustomerService.Application.Common.Interfaces;
using CustomerService.Application.Features.Accounts.Dtos;
using MediatR;

namespace CustomerService.Application.Features.Accounts.Queries.GetAccountsByCustomerId;

public class GetAccountsByCustomerIdQueryHandler : IRequestHandler<GetAccountsByCustomerIdQuery, IEnumerable<AccountDto>>
{
    private readonly IDapperRepository _repository;

    public GetAccountsByCustomerIdQueryHandler(IDapperRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AccountDto>> Handle(GetAccountsByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.QueryAsync<AccountDto>(
            @"SELECT ID, CUSTOMER_ID, ACCOUNT_NUMBER, ACCOUNT_TYPE, BALANCE, CURRENCY, IS_ACTIVE, CREATED_AT
              FROM ACCOUNTS
              WHERE CUSTOMER_ID = @CustomerId",
            new { request.CustomerId });
    }
}
