using MediatR;

namespace CustomerService.Application.Features.Accounts.Commands.SendAccountNotification;

public record SendAccountNotificationCommand(Guid? AccountId) : IRequest<SendAccountNotificationResult>;

public record SendAccountNotificationResult(bool AccountNotificationSent);
