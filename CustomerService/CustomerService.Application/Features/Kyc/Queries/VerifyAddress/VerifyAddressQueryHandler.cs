using MediatR;

namespace CustomerService.Application.Features.Kyc.Queries.VerifyAddress;

public class VerifyAddressQueryHandler : IRequestHandler<VerifyAddressQuery, VerifyAddressResult>
{
    public Task<VerifyAddressResult> Handle(VerifyAddressQuery request, CancellationToken cancellationToken)
    {
        var addressValid = !string.IsNullOrWhiteSpace(request.Address);
        return Task.FromResult(new VerifyAddressResult(addressValid));
    }
}
