using FluentValidation;
using MediatR;
using CustomerService.Api.Middleware;
using CustomerService.Application.Common.Behaviors;
using CustomerService.Application.Common.Interfaces;
using CustomerService.Application.Features.Customers.Commands.CreateCustomer;
using CustomerService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dapper Repository
builder.Services.AddScoped<IDapperRepository, DapperRepository>();

// MediatR + Validation Pipeline
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).Assembly);
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(typeof(CreateCustomerCommand).Assembly);

var app = builder.Build();

// Migration
if (app.Environment.IsDevelopment())
{
    await CustomerService.Infrastructure.Data.InitialMigration.Run(app.Configuration);

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

Console.WriteLine(@"
====================================================
  CustomerService API (Pure REST)
====================================================
  Swagger: http://localhost:5100/swagger
  Customers:
    GET    /api/customers
    GET    /api/customers/{id}
    GET    /api/customers/by-citizen/{citizenId}
    GET    /api/customers/check-duplicate?name=&surname=
    POST   /api/customers
    POST   /api/customers/validate
    POST   /api/customers/check-duplicate
    POST   /api/customers/activate
    POST   /api/customers/send-notification
    POST   /api/customers/log-duplicate
    POST   /api/customers/reject
    PUT    /api/customers/{id}
    DELETE /api/customers/{id}
  Accounts:
    GET    /api/accounts/by-customer/{customerId}
    POST   /api/accounts
    POST   /api/accounts/verify-customer
    POST   /api/accounts/check-limit
    POST   /api/accounts/send-notification
  KYC:
    POST   /api/kyc/verify-identity
    POST   /api/kyc/check-blacklist
    POST   /api/kyc/verify-address
    POST   /api/kyc/evaluate
  Transfers:
    GET    /api/transfers/by-account/{accountId}
    POST   /api/transfers/validate
    POST   /api/transfers/execute
    POST   /api/transfers/send-notification
====================================================
");

app.Run();
