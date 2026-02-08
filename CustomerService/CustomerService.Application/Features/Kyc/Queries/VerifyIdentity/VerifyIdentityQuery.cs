using MediatR;

namespace CustomerService.Application.Features.Kyc.Queries.VerifyIdentity;

public record VerifyIdentityQuery(string CitizenId) : IRequest<VerifyIdentityResult>;

public record VerifyIdentityResult(bool IdentityValid);
