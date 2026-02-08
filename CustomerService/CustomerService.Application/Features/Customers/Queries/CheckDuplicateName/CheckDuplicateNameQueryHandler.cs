using CustomerService.Application.Common.Interfaces;
using MediatR;

namespace CustomerService.Application.Features.Customers.Queries.CheckDuplicateName;

public class CheckDuplicateNameQueryHandler : IRequestHandler<CheckDuplicateNameQuery, CheckDuplicateNameResult>
{
    private readonly IDapperRepository _repository;

    public CheckDuplicateNameQueryHandler(IDapperRepository repository)
    {
        _repository = repository;
    }

    public async Task<CheckDuplicateNameResult> Handle(CheckDuplicateNameQuery request, CancellationToken cancellationToken)
    {
        var existing = await _repository.QueryFirstOrDefaultAsync<ExistingCustomerRow>(
            @"SELECT ID, CITIZEN_ID
              FROM CUSTOMERS
              WHERE NAME = @Name AND SURNAME = @Surname",
            new { request.Name, request.Surname });

        if (existing != null)
            return new CheckDuplicateNameResult(true, existing.Id, existing.CitizenId);

        return new CheckDuplicateNameResult(false, null, null);
    }

    private record ExistingCustomerRow(Guid Id, string CitizenId);
}
