using CustomerService.Application.Common.Interfaces;
using MediatR;

namespace CustomerService.Application.Features.Transfers.Queries.GetTransfersByAccountId;

public class GetTransfersByAccountIdQueryHandler : IRequestHandler<GetTransfersByAccountIdQuery, IEnumerable<TransferDto>>
{
    private readonly IDapperRepository _repository;

    public GetTransfersByAccountIdQueryHandler(IDapperRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TransferDto>> Handle(GetTransfersByAccountIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.QueryAsync<TransferDto>(
            @"SELECT ID, SOURCE_ACCOUNT_ID, DESTINATION_ACCOUNT_ID, AMOUNT, CURRENCY, DESCRIPTION, STATUS, CREATED_AT
              FROM TRANSFERS
              WHERE SOURCE_ACCOUNT_ID = @AccountId OR DESTINATION_ACCOUNT_ID = @AccountId
              ORDER BY CREATED_AT DESC",
            new { request.AccountId });
    }
}
