using MediatR;
using Microsoft.Extensions.Logging;

namespace CustomerService.Application.Features.Customers.Commands.SendNotification;

public class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand, SendNotificationResult>
{
    private readonly ILogger<SendNotificationCommandHandler> _logger;

    public SendNotificationCommandHandler(ILogger<SendNotificationCommandHandler> logger)
    {
        _logger = logger;
    }

    public async Task<SendNotificationResult> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Send Welcome Notification: {Name} -> {Email}", request.Name, request.Email ?? "N/A");

        await Task.Delay(100, cancellationToken);

        return new SendNotificationResult(true);
    }
}
