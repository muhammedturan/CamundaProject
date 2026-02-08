using System.Data;
using CustomerService.Application.Common.Interfaces;
using MediatR;

namespace CustomerService.Application.Features.Transfers.Commands.ExecuteTransfer;

public class ExecuteTransferCommandHandler : IRequestHandler<ExecuteTransferCommand, ExecuteTransferResult>
{
    private readonly IDapperRepository _repository;

    public ExecuteTransferCommandHandler(IDapperRepository repository)
    {
        _repository = repository;
    }

    public async Task<ExecuteTransferResult> Handle(ExecuteTransferCommand request, CancellationToken cancellationToken)
    {
        var transferId = Guid.NewGuid();

        using var connection = _repository.GetConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            ExecuteNonQuery(connection, transaction,
                "UPDATE ACCOUNTS SET BALANCE = BALANCE - @Amount, UPDATED_AT = GETUTCDATE() WHERE ID = @Id",
                ("@Amount", request.Amount), ("@Id", request.SourceAccountId));

            ExecuteNonQuery(connection, transaction,
                "UPDATE ACCOUNTS SET BALANCE = BALANCE + @Amount, UPDATED_AT = GETUTCDATE() WHERE ID = @Id",
                ("@Amount", request.Amount), ("@Id", request.DestinationAccountId));

            ExecuteNonQuery(connection, transaction,
                @"INSERT INTO TRANSFERS (ID, SOURCE_ACCOUNT_ID, DESTINATION_ACCOUNT_ID, AMOUNT, DESCRIPTION, STATUS, CREATED_AT)
                  VALUES (@Id, @SourceAccountId, @DestinationAccountId, @Amount, @Description, 'COMPLETED', GETUTCDATE())",
                ("@Id", transferId),
                ("@SourceAccountId", request.SourceAccountId),
                ("@DestinationAccountId", request.DestinationAccountId),
                ("@Amount", request.Amount),
                ("@Description", (object?)request.Description ?? DBNull.Value));

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }

        var newBalance = await _repository.ExecuteScalarAsync<decimal>(
            "SELECT BALANCE FROM ACCOUNTS WHERE ID = @Id",
            new { Id = request.SourceAccountId });

        return new ExecuteTransferResult(transferId, newBalance);
    }

    private static void ExecuteNonQuery(IDbConnection connection, IDbTransaction transaction, string sql, params (string name, object? value)[] parameters)
    {
        using var cmd = connection.CreateCommand();
        cmd.Transaction = transaction;
        cmd.CommandText = sql;
        foreach (var (name, value) in parameters)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = name;
            param.Value = value ?? DBNull.Value;
            cmd.Parameters.Add(param);
        }
        cmd.ExecuteNonQuery();
    }
}
