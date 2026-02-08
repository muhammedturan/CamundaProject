using MediatR;
using Microsoft.Extensions.Logging;

namespace CustomerService.Application.Features.Transfers.Commands.SendTransferNotification;

public class SendTransferNotificationCommandHandler : IRequestHandler<SendTransferNotificationCommand, SendTransferNotificationResult>
{
    private readonly ILogger<SendTransferNotificationCommandHandler> _logger;

    public SendTransferNotificationCommandHandler(ILogger<SendTransferNotificationCommandHandler> logger)
    {
        _logger = logger;
    }

    public Task<SendTransferNotificationResult> Handle(SendTransferNotificationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Transfer bildirimi gonderildi: TransferId={TransferId}, Tutar={Amount:F2} TL",
            request.TransferId, request.Amount);

        return Task.FromResult(new SendTransferNotificationResult(true));
    }
}
