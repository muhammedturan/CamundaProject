using MediatR;

namespace CustomerService.Application.Features.Kyc.Queries.VerifyAddress;

public record VerifyAddressQuery(string? Address) : IRequest<VerifyAddressResult>;

public record VerifyAddressResult(bool AddressValid);
