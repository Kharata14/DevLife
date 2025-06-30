using DevLife.Application.Features.BugChase.Commands;
using DevLife.Application.Features.BugChase.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevLife.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BugChaseController : ControllerBase
{
    private readonly IMediator _mediator;

    public BugChaseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("my-high-score")]
    public async Task<IActionResult> GetMyHighScore()
    {
        var query = new GetMyHighScoreQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("bug-chase-eaderboard")]
    public async Task<IActionResult> GetLeaderboard()
    {
        var query = new GetLeaderboardQuery();
        var leaderboard = await _mediator.Send(query);
        return Ok(leaderboard);
    }

    [HttpPost("bug-chase-score")]
    public async Task<IActionResult> SubmitScore([FromBody] SubmitScoreCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "Score submitted successfully!" });
    }
}