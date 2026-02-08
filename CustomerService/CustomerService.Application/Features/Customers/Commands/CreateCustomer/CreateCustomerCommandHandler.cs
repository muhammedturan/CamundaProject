using CustomerService.Application.Common.Interfaces;
using CustomerService.Domain.Aggregates;
using MediatR;

namespace CustomerService.Application.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly IDapperRepository _repository;

    public CreateCustomerCommandHandler(IDapperRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = Customer.Create(
            request.Name,
            request.Surname,
            request.CitizenId,
            request.Email,
            request.Phone);

        await _repository.ExecuteAsync(
            @"INSERT INTO CUSTOMERS (ID, NAME, SURNAME, CITIZEN_ID, EMAIL, PHONE, IS_ACTIVE, CREATED_AT)
              VALUES (@Id, @Name, @Surname, @CitizenId, @Email, @Phone, @IsActive, @CreatedAt)",
            new
            {
                customer.Id,
                request.Name,
                request.Surname,
                request.CitizenId,
                request.Email,
                request.Phone,
                IsActive = true,
                customer.CreatedAt
            });

        return customer.Id;
    }
}
