using MediatR;
using Microsoft.Extensions.Logging;

namespace CustomerService.Application.Features.Accounts.Commands.SendAccountNotification;

public class SendAccountNotificationCommandHandler : IRequestHandler<SendAccountNotificationCommand, SendAccountNotificationResult>
{
    private readonly ILogger<SendAccountNotificationCommandHandler> _logger;

    public SendAccountNotificationCommandHandler(ILogger<SendAccountNotificationCommandHandler> logger)
    {
        _logger = logger;
    }

    public async Task<SendAccountNotificationResult> Handle(SendAccountNotificationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Send Account Notification: {AccountId}", request.AccountId?.ToString() ?? "N/A");

        await Task.Delay(100, cancellationToken);

        return new SendAccountNotificationResult(true);
    }
}
