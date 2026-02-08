using MediatR;

namespace CustomerService.Application.Features.Kyc.Queries.CheckBlacklist;

public record CheckBlacklistQuery(string CitizenId) : IRequest<CheckBlacklistResult>;

public record CheckBlacklistResult(bool IsBlacklisted);
