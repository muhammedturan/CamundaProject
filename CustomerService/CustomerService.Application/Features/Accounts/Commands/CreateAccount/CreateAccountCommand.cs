using CustomerService.Domain.Enums;
using MediatR;

namespace CustomerService.Application.Features.Accounts.Commands.CreateAccount;

public record CreateAccountCommand(
    Guid CustomerId,
    AccountType AccountType = AccountType.Vadesiz,
    string Currency = "TRY") : IRequest<Guid>;
