using MediatR;

namespace CustomerService.Application.Features.Accounts.Commands.CheckAccountLimit;

public record CheckAccountLimitCommand(Guid CustomerId) : IRequest<CheckAccountLimitResult>;

public record CheckAccountLimitResult(bool WithinLimit, int CurrentAccountCount);
