using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CustomerService.Infrastructure.Data;

public static class InitialMigration
{
    public static async Task Run(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not found.");

        await EnsureDatabaseExists(connectionString);

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        await CreateCustomersTable(connection);
        await CreateAccountsTable(connection);
        await CreateTransfersTable(connection);

        Console.WriteLine("Database migration completed.");
    }

    private static async Task EnsureDatabaseExists(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        var dbName = builder.InitialCatalog;
        builder.InitialCatalog = "master";

        await using var connection = new SqlConnection(builder.ConnectionString);
        await connection.OpenAsync();

        var sql = $"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '{dbName}') CREATE DATABASE [{dbName}]";
        await using var cmd = new SqlCommand(sql, connection);
        await cmd.ExecuteNonQueryAsync();
    }

    private static async Task CreateCustomersTable(SqlConnection connection)
    {
        var sql = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CUSTOMERS')
            BEGIN
                CREATE TABLE CUSTOMERS (
                    ID UNIQUEIDENTIFIER PRIMARY KEY,
                    NAME NVARCHAR(100) NOT NULL,
                    SURNAME NVARCHAR(100) NOT NULL,
                    CITIZEN_ID NVARCHAR(11) NOT NULL,
                    EMAIL NVARCHAR(200),
                    PHONE NVARCHAR(20),
                    IS_ACTIVE BIT NOT NULL DEFAULT 1,
                    CREATED_AT DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                    UPDATED_AT DATETIME2,
                    CONSTRAINT UQ_CUSTOMERS_CITIZEN_ID UNIQUE (CITIZEN_ID)
                );

                CREATE INDEX IDX_CUSTOMERS_CITIZEN_ID ON CUSTOMERS(CITIZEN_ID);
                CREATE INDEX IDX_CUSTOMERS_NAME_SURNAME ON CUSTOMERS(NAME, SURNAME);
            END";

        await using var cmd = new SqlCommand(sql, connection);
        await cmd.ExecuteNonQueryAsync();
    }

    private static async Task CreateAccountsTable(SqlConnection connection)
    {
        var sql = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ACCOUNTS')
            BEGIN
                CREATE TABLE ACCOUNTS (
                    ID UNIQUEIDENTIFIER PRIMARY KEY,
                    CUSTOMER_ID UNIQUEIDENTIFIER NOT NULL,
                    ACCOUNT_NUMBER NVARCHAR(20) NOT NULL,
                    ACCOUNT_TYPE INT NOT NULL,
                    BALANCE DECIMAL(18,2) NOT NULL DEFAULT 0,
                    CURRENCY NVARCHAR(3) NOT NULL DEFAULT 'TRY',
                    IS_ACTIVE BIT NOT NULL DEFAULT 1,
                    CREATED_AT DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                    UPDATED_AT DATETIME2,
                    CONSTRAINT FK_ACCOUNTS_CUSTOMER FOREIGN KEY (CUSTOMER_ID) REFERENCES CUSTOMERS(ID),
                    CONSTRAINT UQ_ACCOUNTS_NUMBER UNIQUE (ACCOUNT_NUMBER)
                );

                CREATE INDEX IDX_ACCOUNTS_CUSTOMER_ID ON ACCOUNTS(CUSTOMER_ID);
            END";

        await using var cmd = new SqlCommand(sql, connection);
        await cmd.ExecuteNonQueryAsync();
    }

    private static async Task CreateTransfersTable(SqlConnection connection)
    {
        var sql = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TRANSFERS')
            BEGIN
                CREATE TABLE TRANSFERS (
                    ID UNIQUEIDENTIFIER PRIMARY KEY,
                    SOURCE_ACCOUNT_ID UNIQUEIDENTIFIER NOT NULL,
                    DESTINATION_ACCOUNT_ID UNIQUEIDENTIFIER NOT NULL,
                    AMOUNT DECIMAL(18,2) NOT NULL,
                    CURRENCY NVARCHAR(3) NOT NULL DEFAULT 'TRY',
                    DESCRIPTION NVARCHAR(500),
                    STATUS NVARCHAR(20) NOT NULL DEFAULT 'COMPLETED',
                    CREATED_AT DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                    CONSTRAINT FK_TRANSFERS_SOURCE FOREIGN KEY (SOURCE_ACCOUNT_ID) REFERENCES ACCOUNTS(ID),
                    CONSTRAINT FK_TRANSFERS_DEST FOREIGN KEY (DESTINATION_ACCOUNT_ID) REFERENCES ACCOUNTS(ID)
                );

                CREATE INDEX IDX_TRANSFERS_SOURCE ON TRANSFERS(SOURCE_ACCOUNT_ID);
                CREATE INDEX IDX_TRANSFERS_DEST ON TRANSFERS(DESTINATION_ACCOUNT_ID);
            END";

        await using var cmd = new SqlCommand(sql, connection);
        await cmd.ExecuteNonQueryAsync();
    }
}
