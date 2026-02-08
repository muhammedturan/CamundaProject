using CustomerService.Application.Common.Interfaces;
using CustomerService.Application.Features.Customers.Dtos;
using MediatR;

namespace CustomerService.Application.Features.Accounts.Commands.VerifyCustomer;

public class VerifyCustomerCommandHandler : IRequestHandler<VerifyCustomerCommand, VerifyCustomerResult>
{
    private readonly IDapperRepository _repository;

    public VerifyCustomerCommandHandler(IDapperRepository repository)
    {
        _repository = repository;
    }

    public async Task<VerifyCustomerResult> Handle(VerifyCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _repository.QueryFirstOrDefaultAsync<CustomerDto>(
            @"SELECT ID, NAME, SURNAME, CITIZEN_ID, EMAIL, PHONE, IS_ACTIVE, CREATED_AT
              FROM CUSTOMERS
              WHERE CITIZEN_ID = @CitizenId",
            new { request.CitizenId });

        var customerFound = customer != null && customer.IsActive;
        return new VerifyCustomerResult(customerFound, customer?.Id ?? Guid.Empty);
    }
}
