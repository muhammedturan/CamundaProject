using MediatR;
using Microsoft.Extensions.Logging;

namespace CustomerService.Application.Features.Customers.Commands.LogDuplicate;

public class LogDuplicateCommandHandler : IRequestHandler<LogDuplicateCommand, LogDuplicateResult>
{
    private readonly ILogger<LogDuplicateCommandHandler> _logger;

    public LogDuplicateCommandHandler(ILogger<LogDuplicateCommandHandler> logger)
    {
        _logger = logger;
    }

    public Task<LogDuplicateResult> Handle(LogDuplicateCommand request, CancellationToken cancellationToken)
    {
        _logger.LogWarning("Duplike musteri tespit edildi: {Name} {Surname}", request.Name ?? "N/A", request.Surname ?? "N/A");
        return Task.FromResult(new LogDuplicateResult(true));
    }
}
