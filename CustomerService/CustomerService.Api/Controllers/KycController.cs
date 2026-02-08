using CustomerService.Application.Features.Kyc.Queries.CheckBlacklist;
using CustomerService.Application.Features.Kyc.Queries.EvaluateKyc;
using CustomerService.Application.Features.Kyc.Queries.VerifyAddress;
using CustomerService.Application.Features.Kyc.Queries.VerifyIdentity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KycController : ControllerBase
{
    private readonly IMediator _mediator;

    public KycController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("verify-identity")]
    public async Task<IActionResult> VerifyIdentity([FromBody] VerifyIdentityQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("check-blacklist")]
    public async Task<IActionResult> CheckBlacklist([FromBody] CheckBlacklistQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("verify-address")]
    public async Task<IActionResult> VerifyAddress([FromBody] VerifyAddressQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("evaluate")]
    public async Task<IActionResult> Evaluate([FromBody] EvaluateKycQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
