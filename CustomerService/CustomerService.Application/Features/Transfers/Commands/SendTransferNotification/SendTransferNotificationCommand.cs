using MediatR;

namespace CustomerService.Application.Features.Transfers.Commands.SendTransferNotification;

public record SendTransferNotificationCommand(Guid? TransferId, decimal Amount) : IRequest<SendTransferNotificationResult>;

public record SendTransferNotificationResult(bool TransferNotificationSent);
