using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Zeebe.Client;

namespace BpmService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProcessController : ControllerBase
{
    private readonly IZeebeClient _zeebeClient;

    public ProcessController(IZeebeClient zeebeClient)
    {
        _zeebeClient = zeebeClient;
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartProcess([FromBody] StartProcessRequest request)
    {
        var variables = JsonSerializer.Serialize(request.Variables);

        var result = await _zeebeClient.NewCreateProcessInstanceCommand()
            .BpmnProcessId(request.ProcessId)
            .LatestVersion()
            .Variables(variables)
            .WithResult()
            .Send();

        var outputVariables = JsonSerializer.Deserialize<JsonElement>(result.Variables);

        return Ok(new
        {
            processInstanceKey = result.ProcessInstanceKey,
            processId = request.ProcessId,
            variables = outputVariables
        });
    }

    [HttpPost("deploy")]
    public async Task<IActionResult> Deploy([FromQuery] string? fileName)
    {
        var workflowsDir = Path.Combine(AppContext.BaseDirectory, "Workflows");

        if (!Directory.Exists(workflowsDir))
            return NotFound(new { message = $"Workflows klasoru bulunamadi: {workflowsDir}" });

        var files = string.IsNullOrEmpty(fileName)
            ? Directory.GetFiles(workflowsDir, "*.bpmn")
            : new[] { Path.Combine(workflowsDir, fileName) };

        if (files.Length == 0)
            return NotFound(new { message = "Deploy edilecek BPMN dosyasi bulunamadi" });

        var deployed = new List<string>();
        foreach (var file in files)
        {
            if (!System.IO.File.Exists(file)) continue;

            await _zeebeClient.NewDeployCommand()
                .AddResourceFile(file)
                .Send();

            deployed.Add(Path.GetFileName(file));
        }

        return Ok(new { message = "BPMN dosyalari deploy edildi", files = deployed });
    }
}

public record StartProcessRequest(string ProcessId, Dictionary<string, object> Variables);
