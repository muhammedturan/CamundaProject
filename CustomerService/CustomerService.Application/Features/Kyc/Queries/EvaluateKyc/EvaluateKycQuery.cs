using MediatR;

namespace CustomerService.Application.Features.Kyc.Queries.EvaluateKyc;

public record EvaluateKycQuery(bool IdentityValid, bool IsBlacklisted, bool AddressValid) : IRequest<EvaluateKycResult>;

public record EvaluateKycResult(bool KycPassed);
