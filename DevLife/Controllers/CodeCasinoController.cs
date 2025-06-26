using DevLife.Application.Features.CodeCasino.Commands;
using DevLife.Application.Features.CodeCasino.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevLife.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CodeCasinoController : ControllerBase
{
    private readonly IMediator _mediator;

    public CodeCasinoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("challenge")]
    public async Task<IActionResult> GetChallenge()
    {
        var query = new GetCasinoChallengeQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("bet")]
    public async Task<IActionResult> PlaceBet([FromBody] PlaceBetCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}