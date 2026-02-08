using CustomerService.Application.Common.Interfaces;
using MediatR;

namespace CustomerService.Application.Features.Customers.Commands.DeleteCustomer;

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
{
    private readonly IDapperRepository _repository;

    public DeleteCustomerCommandHandler(IDapperRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var rows = await _repository.ExecuteAsync(
            @"UPDATE CUSTOMERS SET IS_ACTIVE = 0, UPDATED_AT = @UpdatedAt WHERE ID = @Id",
            new { request.Id, UpdatedAt = DateTime.UtcNow });

        return rows > 0;
    }
}
