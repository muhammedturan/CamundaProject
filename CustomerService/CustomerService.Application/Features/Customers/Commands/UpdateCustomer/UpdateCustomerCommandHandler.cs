using CustomerService.Application.Common.Interfaces;
using MediatR;

namespace CustomerService.Application.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, bool>
{
    private readonly IDapperRepository _repository;

    public UpdateCustomerCommandHandler(IDapperRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var rows = await _repository.ExecuteAsync(
            @"UPDATE CUSTOMERS
              SET NAME = @Name, SURNAME = @Surname, EMAIL = @Email, PHONE = @Phone, UPDATED_AT = @UpdatedAt
              WHERE ID = @Id",
            new { request.Id, request.Name, request.Surname, request.Email, request.Phone, UpdatedAt = DateTime.UtcNow });

        return rows > 0;
    }
}
