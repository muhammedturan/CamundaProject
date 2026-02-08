using CustomerService.Application.Common.Interfaces;
using MediatR;

namespace CustomerService.Application.Features.Accounts.Commands.CheckAccountLimit;

public class CheckAccountLimitCommandHandler : IRequestHandler<CheckAccountLimitCommand, CheckAccountLimitResult>
{
    private const int MaxAccounts = 5;
    private readonly IDapperRepository _repository;

    public CheckAccountLimitCommandHandler(IDapperRepository repository)
    {
        _repository = repository;
    }

    public async Task<CheckAccountLimitResult> Handle(CheckAccountLimitCommand request, CancellationToken cancellationToken)
    {
        var accountCount = await _repository.ExecuteScalarAsync<int>(
            @"SELECT COUNT(*) FROM ACCOUNTS WHERE CUSTOMER_ID = @CustomerId AND IS_ACTIVE = 1",
            new { request.CustomerId });

        var withinLimit = accountCount < MaxAccounts;
        return new CheckAccountLimitResult(withinLimit, accountCount);
    }
}
