using DevLife.Application.Features.CodeRoast.Commands;
using DevLife.Application.Features.CodeRoast.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevLife.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CodeRoastController : ControllerBase
{
    private readonly IMediator _mediator;

    public CodeRoastController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("code-roast-challenge")]
    public async Task<IActionResult> GetChallenge([FromQuery] string difficulty = "Junior")
    {
        var query = new GetRoastChallengeQuery { Difficulty = difficulty };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("code-roast-submission")]
    public async Task<IActionResult> SubmitSolution([FromBody] SubmitSolutionCommand command)
    {
        var submissionId = await _mediator.Send(command);
        return AcceptedAtAction(nameof(GetSubmissionResult), new { id = submissionId }, new { submissionId });
    }

    [HttpGet("code-roast-submission/{id:guid}")]
    public async Task<IActionResult> GetSubmissionResult(Guid id)
    {
        var query = new GetSubmissionResultQuery { SubmissionId = id };
        var result = await _mediator.Send(query);
        if (result == null) return NotFound();
        return Ok(result);
    }
}
