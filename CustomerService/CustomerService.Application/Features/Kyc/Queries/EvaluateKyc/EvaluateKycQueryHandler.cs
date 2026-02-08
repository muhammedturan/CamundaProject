using MediatR;

namespace CustomerService.Application.Features.Kyc.Queries.EvaluateKyc;

public class EvaluateKycQueryHandler : IRequestHandler<EvaluateKycQuery, EvaluateKycResult>
{
    public Task<EvaluateKycResult> Handle(EvaluateKycQuery request, CancellationToken cancellationToken)
    {
        var kycPassed = request.IdentityValid && !request.IsBlacklisted && request.AddressValid;
        return Task.FromResult(new EvaluateKycResult(kycPassed));
    }
}
