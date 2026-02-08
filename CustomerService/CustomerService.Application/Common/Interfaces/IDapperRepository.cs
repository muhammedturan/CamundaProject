using System.Data;

namespace CustomerService.Application.Common.Interfaces;

public interface IDapperRepository
{
    Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null);
    Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null);
    Task<int> ExecuteAsync(string sql, object? param = null, IDbTransaction? transaction = null);
    Task<T?> ExecuteScalarAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null);
    IDbConnection GetConnection();
}
