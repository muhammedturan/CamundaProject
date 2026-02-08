using CustomerService.Application.Common.Interfaces;
using CustomerService.Domain.Aggregates;
using MediatR;

namespace CustomerService.Application.Features.Accounts.Commands.CreateAccount;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Guid>
{
    private readonly IDapperRepository _repository;

    public CreateAccountCommandHandler(IDapperRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = Account.Create(request.CustomerId, request.AccountType, request.Currency);

        await _repository.ExecuteAsync(
            @"INSERT INTO ACCOUNTS (ID, CUSTOMER_ID, ACCOUNT_NUMBER, ACCOUNT_TYPE, BALANCE, CURRENCY, IS_ACTIVE, CREATED_AT)
              VALUES (@Id, @CustomerId, @AccountNumber, @AccountType, @Balance, @Currency, @IsActive, @CreatedAt)",
            new
            {
                account.Id,
                request.CustomerId,
                account.AccountNumber,
                AccountType = (int)request.AccountType,
                account.Balance,
                request.Currency,
                IsActive = true,
                account.CreatedAt
            });

        return account.Id;
    }
}
