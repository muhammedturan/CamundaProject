using MediatR;

namespace CustomerService.Application.Features.Customers.Commands.LogDuplicate;

public record LogDuplicateCommand(string? Name, string? Surname) : IRequest<LogDuplicateResult>;

public record LogDuplicateResult(bool DuplicateHandled);
