using MediatR;

namespace CustomerService.Application.Features.Kyc.Queries.VerifyIdentity;

public class VerifyIdentityQueryHandler : IRequestHandler<VerifyIdentityQuery, VerifyIdentityResult>
{
    public Task<VerifyIdentityResult> Handle(VerifyIdentityQuery request, CancellationToken cancellationToken)
    {
        var identityValid = request.CitizenId is { Length: 11 } && request.CitizenId.All(char.IsDigit);
        return Task.FromResult(new VerifyIdentityResult(identityValid));
    }
}
