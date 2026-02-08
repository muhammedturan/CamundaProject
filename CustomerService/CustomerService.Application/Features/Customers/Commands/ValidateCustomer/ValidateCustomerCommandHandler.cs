using MediatR;

namespace CustomerService.Application.Features.Customers.Commands.ValidateCustomer;

public class ValidateCustomerCommandHandler : IRequestHandler<ValidateCustomerCommand, ValidateCustomerResult>
{
    public Task<ValidateCustomerResult> Handle(ValidateCustomerCommand request, CancellationToken cancellationToken)
    {
        var isValid = !string.IsNullOrWhiteSpace(request.Name)
                      && !string.IsNullOrWhiteSpace(request.Surname)
                      && !string.IsNullOrWhiteSpace(request.CitizenId)
                      && request.CitizenId.Length == 11;

        return Task.FromResult(new ValidateCustomerResult(isValid));
    }
}
