using CustomerService.Application.Features.Transfers.Commands.ExecuteTransfer;
using CustomerService.Application.Features.Transfers.Commands.SendTransferNotification;
using CustomerService.Application.Features.Transfers.Commands.ValidateTransfer;
using CustomerService.Application.Features.Transfers.Queries.GetTransfersByAccountId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransfersController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransfersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("validate")]
    public async Task<IActionResult> Validate([FromBody] ValidateTransferCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("execute")]
    public async Task<IActionResult> Execute([FromBody] ExecuteTransferCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("send-notification")]
    public async Task<IActionResult> SendNotification([FromBody] SendTransferNotificationCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("by-account/{accountId:guid}")]
    public async Task<IActionResult> GetByAccountId(Guid accountId)
    {
        var transfers = await _mediator.Send(new GetTransfersByAccountIdQuery(accountId));
        return Ok(transfers);
    }
}
