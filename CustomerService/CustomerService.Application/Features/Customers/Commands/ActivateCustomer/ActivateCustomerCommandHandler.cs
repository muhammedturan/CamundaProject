using CustomerService.Application.Common.Interfaces;
using MediatR;

namespace CustomerService.Application.Features.Customers.Commands.ActivateCustomer;

public class ActivateCustomerCommandHandler : IRequestHandler<ActivateCustomerCommand, bool>
{
    private readonly IDapperRepository _repository;

    public ActivateCustomerCommandHandler(IDapperRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(ActivateCustomerCommand request, CancellationToken cancellationToken)
    {
        var rows = await _repository.ExecuteAsync(
            @"UPDATE CUSTOMERS SET IS_ACTIVE = 1, UPDATED_AT = @UpdatedAt WHERE ID = @Id",
            new { Id = request.CustomerId, UpdatedAt = DateTime.UtcNow });

        return rows > 0;
    }
}
