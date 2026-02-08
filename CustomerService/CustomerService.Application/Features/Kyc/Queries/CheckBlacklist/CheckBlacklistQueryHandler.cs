using MediatR;

namespace CustomerService.Application.Features.Kyc.Queries.CheckBlacklist;

public class CheckBlacklistQueryHandler : IRequestHandler<CheckBlacklistQuery, CheckBlacklistResult>
{
    private static readonly string[] BlacklistedIds = ["99999999999", "11111111111"];

    public Task<CheckBlacklistResult> Handle(CheckBlacklistQuery request, CancellationToken cancellationToken)
    {
        var isBlacklisted = BlacklistedIds.Contains(request.CitizenId);
        return Task.FromResult(new CheckBlacklistResult(isBlacklisted));
    }
}
