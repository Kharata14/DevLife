using DevLife.Application.Features.Dating.Commands;
using DevLife.Application.Features.Dating.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DatingController : ControllerBase
{
    private readonly IMediator _mediator;
    public DatingController(IMediator mediator) => _mediator = mediator;

    [HttpPut("my-profile")]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateDatingProfileCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet("profiles")]
    public async Task<IActionResult> GetPotentialMatches()
    {
        var query = new GetPotentialMatchesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

}