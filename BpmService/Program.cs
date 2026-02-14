using BpmService.HealthChecks;
using BpmService.Workers;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Zeebe.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Zeebe Client
builder.Services.AddSingleton<IZeebeClient>(sp =>
    ZeebeClient.Builder()
        .UseGatewayAddress(builder.Configuration["Zeebe:GatewayAddress"] ?? "localhost:26500")
        .UsePlainText()
        .Build());

// HttpClient for calling downstream services
builder.Services.AddHttpClient("ServiceClient");

// Dynamic Worker
builder.Services.AddHostedService<DynamicWorkerService>();

// Health Checks
builder.Services.AddHealthChecks()
    .AddCheck<ZeebeHealthCheck>(
        "zeebe",
        failureStatus: HealthStatus.Unhealthy,
        tags: ["zeebe", "ready"]);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

// Health Check Endpoints
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => false, // liveness — sadece uygulama ayakta mı
    ResponseWriter = HealthCheckResponseWriter.WriteResponse
});
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = HealthCheckResponseWriter.WriteResponse
});

Console.WriteLine(@"
====================================================
  BpmService - Workflow Orchestration
====================================================
  Swagger: http://localhost:5200/swagger
  Process:
    POST   /api/process/start    <- BPMN trigger
    POST   /api/process/deploy   <- BPMN deploy
====================================================
");

app.Run();

/// <summary>
/// Health check response'unu JSON formatında yazan yardımcı sınıf.
/// </summary>
static class HealthCheckResponseWriter
{
    public static async Task WriteResponse(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = report.Status.ToString(),
            duration = report.TotalDuration.TotalMilliseconds + "ms",
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                duration = e.Value.Duration.TotalMilliseconds + "ms",
                exception = e.Value.Exception?.Message,
                data = e.Value.Data
            })
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}
