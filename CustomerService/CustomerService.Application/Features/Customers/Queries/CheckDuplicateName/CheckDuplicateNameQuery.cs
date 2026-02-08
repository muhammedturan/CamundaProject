using MediatR;

namespace CustomerService.Application.Features.Customers.Queries.CheckDuplicateName;

public record CheckDuplicateNameQuery(string Name, string Surname) : IRequest<CheckDuplicateNameResult>;

public record CheckDuplicateNameResult(
    bool DuplicateExists,
    Guid? ExistingCustomerId,
    string? ExistingCitizenId);
