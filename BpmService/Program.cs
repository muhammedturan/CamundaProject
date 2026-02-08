using BpmService.Workers;
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

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

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
