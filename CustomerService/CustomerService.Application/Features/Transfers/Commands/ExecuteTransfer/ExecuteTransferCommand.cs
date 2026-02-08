using MediatR;

namespace CustomerService.Application.Features.Transfers.Commands.ExecuteTransfer;

public record ExecuteTransferCommand(
    Guid SourceAccountId,
    Guid DestinationAccountId,
    decimal Amount,
    string? Description) : IRequest<ExecuteTransferResult>;

public record ExecuteTransferResult(Guid TransferId, decimal NewSourceBalance);
