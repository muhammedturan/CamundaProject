using MediatR;

namespace CustomerService.Application.Features.Customers.Commands.SendNotification;

public record SendNotificationCommand(string Name, string? Email) : IRequest<SendNotificationResult>;

public record SendNotificationResult(bool NotificationSent);
