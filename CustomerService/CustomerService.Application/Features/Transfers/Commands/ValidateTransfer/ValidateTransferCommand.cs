using MediatR;

namespace CustomerService.Application.Features.Transfers.Commands.ValidateTransfer;

public record ValidateTransferCommand(
    Guid SourceAccountId,
    Guid DestinationAccountId,
    decimal Amount) : IRequest<ValidateTransferResult>;

public record ValidateTransferResult(bool IsValid, decimal SourceBalance, string? ErrorMessage);
