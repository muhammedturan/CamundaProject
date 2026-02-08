using CustomerService.Application.Common.Interfaces;
using MediatR;

namespace CustomerService.Application.Features.Transfers.Commands.ValidateTransfer;

public class ValidateTransferCommandHandler : IRequestHandler<ValidateTransferCommand, ValidateTransferResult>
{
    private readonly IDapperRepository _repository;

    public ValidateTransferCommandHandler(IDapperRepository repository)
    {
        _repository = repository;
    }

    public async Task<ValidateTransferResult> Handle(ValidateTransferCommand request, CancellationToken cancellationToken)
    {
        var source = await _repository.QueryFirstOrDefaultAsync<AccountRow>(
            "SELECT ID, BALANCE, IS_ACTIVE FROM ACCOUNTS WHERE ID = @Id",
            new { Id = request.SourceAccountId });

        if (source == null)
            return new ValidateTransferResult(false, 0, "Kaynak hesap bulunamadi");

        if (!source.IsActive)
            return new ValidateTransferResult(false, source.Balance, "Kaynak hesap aktif degil");

        var dest = await _repository.QueryFirstOrDefaultAsync<AccountRow>(
            "SELECT ID, BALANCE, IS_ACTIVE FROM ACCOUNTS WHERE ID = @Id",
            new { Id = request.DestinationAccountId });

        if (dest == null)
            return new ValidateTransferResult(false, source.Balance, "Hedef hesap bulunamadi");

        if (!dest.IsActive)
            return new ValidateTransferResult(false, source.Balance, "Hedef hesap aktif degil");

        if (request.Amount <= 0)
            return new ValidateTransferResult(false, source.Balance, "Transfer tutari sifirdan buyuk olmalidir");

        if (source.Balance < request.Amount)
            return new ValidateTransferResult(false, source.Balance, $"Yetersiz bakiye. Mevcut: {source.Balance:F2}, Talep: {request.Amount:F2}");

        return new ValidateTransferResult(true, source.Balance, null);
    }

    private record AccountRow(Guid Id, decimal Balance, bool IsActive);
}
