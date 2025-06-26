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

    [HttpGet("daily-challenge")]
    public async Task<IActionResult> GetDailyChallenge()
    {
        var query = new GetDailyChallengeQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("daily-challenge/bet")]
    public async Task<IActionResult> PlaceDailyChallengeBet([FromBody] PlaceDailyChallengeBetCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
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
    [HttpGet("leaderboard")]
    public async Task<IActionResult> GetLeaderboard()
    {
        var query = new GetCasinoLeaderboardQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}