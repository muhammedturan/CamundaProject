using MediatR;

namespace CustomerService.Application.Features.Transfers.Queries.GetTransfersByAccountId;

public record GetTransfersByAccountIdQuery(Guid AccountId) : IRequest<IEnumerable<TransferDto>>;

public record TransferDto(
    Guid Id,
    Guid SourceAccountId,
    Guid DestinationAccountId,
    decimal Amount,
    string Currency,
    string? Description,
    string Status,
    DateTime CreatedAt);
