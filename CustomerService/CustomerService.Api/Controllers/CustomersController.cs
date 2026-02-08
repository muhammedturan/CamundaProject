using CustomerService.Application.Common.Exceptions;
using CustomerService.Application.Features.Customers.Commands.ActivateCustomer;
using CustomerService.Application.Features.Customers.Commands.CreateCustomer;
using CustomerService.Application.Features.Customers.Commands.DeleteCustomer;
using CustomerService.Application.Features.Customers.Commands.LogDuplicate;
using CustomerService.Application.Features.Customers.Commands.SendNotification;
using CustomerService.Application.Features.Customers.Commands.UpdateCustomer;
using CustomerService.Application.Features.Customers.Commands.ValidateCustomer;
using CustomerService.Application.Features.Customers.Queries.CheckDuplicateName;
using CustomerService.Application.Features.Customers.Queries.GetCustomerByCitizenId;
using CustomerService.Application.Features.Customers.Queries.GetCustomerById;
using CustomerService.Application.Features.Customers.Queries.GetCustomerList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _mediator.Send(new GetCustomerListQuery());
        return Ok(customers);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var customer = await _mediator.Send(new GetCustomerByIdQuery(id));
        return customer is null ? NotFound() : Ok(customer);
    }

    [HttpGet("by-citizen/{citizenId}")]
    public async Task<IActionResult> GetByCitizenId(string citizenId)
    {
        var customer = await _mediator.Send(new GetCustomerByCitizenIdQuery(citizenId));

        return Ok(new
        {
            customerExists = customer != null,
            existingCustomerId = customer?.Id,
            existingCustomerName = customer != null ? $"{customer.Name} {customer.Surname}" : ""
        });
    }

    [HttpGet("check-duplicate")]
    public async Task<IActionResult> CheckDuplicate([FromQuery] string name, [FromQuery] string surname)
    {
        var result = await _mediator.Send(new CheckDuplicateNameQuery(name, surname));

        return Ok(new
        {
            duplicateExists = result.DuplicateExists,
            existingCustomerId = result.ExistingCustomerId,
            existingCitizenId = result.ExistingCitizenId
        });
    }

    [HttpPost("validate")]
    public async Task<IActionResult> Validate([FromBody] ValidateCustomerCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("check-duplicate")]
    public async Task<IActionResult> CheckDuplicatePost([FromBody] CheckDuplicateNameQuery query)
    {
        var result = await _mediator.Send(query);
        if (result.DuplicateExists)
            throw new DuplicateException($"Duplike musteri: {query.Name} {query.Surname}");
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerCommand command)
    {
        var customerId = await _mediator.Send(command);
        return Ok(new { customerId });
    }

    [HttpPost("activate")]
    public async Task<IActionResult> Activate([FromBody] ActivateCustomerCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(new { customerActivated = result });
    }

    [HttpPost("send-notification")]
    public async Task<IActionResult> SendNotification([FromBody] SendNotificationCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("log-duplicate")]
    public async Task<IActionResult> LogDuplicate([FromBody] LogDuplicateCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("reject")]
    public async Task<IActionResult> Reject([FromBody] RejectCustomerRequest request)
    {
        var result = await _mediator.Send(new DeleteCustomerCommand(request.CustomerId));
        return result ? Ok(new { rejected = true }) : NotFound();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerRequest request)
    {
        var result = await _mediator.Send(new UpdateCustomerCommand(id, request.Name, request.Surname, request.Email, request.Phone));
        return result ? Ok(new { updated = true }) : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteCustomerCommand(id));
        return result ? Ok(new { deleted = true }) : NotFound();
    }
}

public record UpdateCustomerRequest(string Name, string Surname, string? Email, string? Phone);
public record RejectCustomerRequest(Guid CustomerId);
