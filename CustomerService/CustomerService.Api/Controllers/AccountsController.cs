using CustomerService.Application.Features.Accounts.Commands.CheckAccountLimit;
using CustomerService.Application.Features.Accounts.Commands.CreateAccount;
using CustomerService.Application.Features.Accounts.Commands.SendAccountNotification;
using CustomerService.Application.Features.Accounts.Commands.VerifyCustomer;
using CustomerService.Application.Features.Accounts.Queries.GetAccountsByCustomerId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("by-customer/{customerId:guid}")]
    public async Task<IActionResult> GetByCustomerId(Guid customerId)
    {
        var accounts = await _mediator.Send(new GetAccountsByCustomerIdQuery(customerId));
        return Ok(accounts);
    }

    [HttpPost("verify-customer")]
    public async Task<IActionResult> VerifyCustomer([FromBody] VerifyCustomerCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("check-limit")]
    public async Task<IActionResult> CheckLimit([FromBody] CheckAccountLimitCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAccountCommand command)
    {
        var accountId = await _mediator.Send(command);
        return Ok(new { accountId });
    }

    [HttpPost("send-notification")]
    public async Task<IActionResult> SendNotification([FromBody] SendAccountNotificationCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
