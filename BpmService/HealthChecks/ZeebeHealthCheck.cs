using Microsoft.Extensions.Diagnostics.HealthChecks;
using Zeebe.Client;

namespace BpmService.HealthChecks;

/// <summary>
/// Zeebe gateway bağlantısını kontrol eden health check.
/// </summary>
public class ZeebeHealthCheck : IHealthCheck
{
    private readonly IZeebeClient _zeebeClient;

    public ZeebeHealthCheck(IZeebeClient zeebeClient)
    {
        _zeebeClient = zeebeClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var topology = await _zeebeClient.TopologyRequest().Send(cancellationToken);

            var data = new Dictionary<string, object>
            {
                ["gatewayVersion"] = topology.GatewayVersion ?? "unknown",
                ["brokers"] = topology.Brokers?.Count ?? 0
            };

            if (topology.Brokers is { Count: > 0 })
            {
                data["partitions"] = topology.Brokers
                    .SelectMany(b => b.Partitions)
                    .Select(p => new { p.PartitionId, Role = p.Role.ToString() })
                    .ToList();
            }

            return HealthCheckResult.Healthy("Zeebe bağlantısı aktif", data);
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Zeebe bağlantısı başarısız", ex);
        }
    }
}
