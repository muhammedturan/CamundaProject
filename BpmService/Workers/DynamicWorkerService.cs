using System.Text.Json;
using Zeebe.Client;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;

namespace BpmService.Workers;

public class DynamicWorkerService : BackgroundService
{
    private readonly IZeebeClient _zeebeClient;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<DynamicWorkerService> _logger;

    public DynamicWorkerService(
        IZeebeClient zeebeClient,
        IHttpClientFactory httpClientFactory,
        ILogger<DynamicWorkerService> logger)
    {
        _zeebeClient = zeebeClient;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _zeebeClient.NewWorker()
            .JobType("dynamic-rest-handler")
            .Handler(HandleJob)
            .MaxJobsActive(5)
            .Timeout(TimeSpan.FromSeconds(30))
            .Open();

        _logger.LogInformation("Dynamic REST Worker started");

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task HandleJob(IJobClient client, IJob job)
    {
        var headers = JsonSerializer.Deserialize<Dictionary<string, string>>(job.CustomHeaders)
                      ?? new Dictionary<string, string>();

        var url = headers.GetValueOrDefault("url", "");
        var method = headers.GetValueOrDefault("method", "POST");
        var errorCode = headers.GetValueOrDefault("errorCode");

        if (string.IsNullOrWhiteSpace(url))
        {
            _logger.LogError("Job [{JobType}] has no 'url' header", job.Type);
            await client.NewFailCommand(job.Key)
                .Retries(0)
                .ErrorMessage("Missing 'url' custom header")
                .Send();
            return;
        }

        _logger.LogInformation("[{ElementId}] {Method} {Url}", job.ElementId, method, url);

        try
        {
            var httpClient = _httpClientFactory.CreateClient("ServiceClient");
            var request = new HttpRequestMessage(new HttpMethod(method), url)
            {
                Content = new StringContent(job.Variables, System.Text.Encoding.UTF8, "application/json")
            };

            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("[{ElementId}] completed -> {Result}", job.ElementId, content);

                await client.NewCompleteJobCommand(job.Key)
                    .Variables(content)
                    .Send();
            }
            else if (!string.IsNullOrEmpty(errorCode))
            {
                _logger.LogWarning("[{ElementId}] BPMN Error {ErrorCode}: {Status} {Content}",
                    job.ElementId, errorCode, response.StatusCode, content);

                await client.NewThrowErrorCommand(job.Key)
                    .ErrorCode(errorCode)
                    .ErrorMessage(content)
                    .Send();
            }
            else
            {
                _logger.LogError("[{ElementId}] HTTP {Status}: {Content}",
                    job.ElementId, response.StatusCode, content);

                await client.NewFailCommand(job.Key)
                    .Retries(job.Retries - 1)
                    .ErrorMessage($"HTTP {response.StatusCode}: {content}")
                    .Send();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[{ElementId}] Exception calling {Url}", job.ElementId, url);
            await client.NewFailCommand(job.Key)
                .Retries(job.Retries - 1)
                .ErrorMessage(ex.Message)
                .Send();
        }
    }
}
