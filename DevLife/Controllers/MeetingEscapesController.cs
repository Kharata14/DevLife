using DevLife.Application.Features.MeetingEscapes.Commands;
using DevLife.Application.Features.MeetingEscapes.Queries;
using DevLife.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevLife.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MeetingEscapesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MeetingEscapesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("meeting-generate")]
    public async Task<IActionResult> GenerateExcuse([FromQuery] MeetingType meetingType)
    {
        var query = new GenerateExcuseQuery { MeetingType = meetingType };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("meeting-favorites")]
    public async Task<IActionResult> SaveFavorite([FromBody] ExcuseDto excuse)
    {
        var command = new SaveFavoriteExcuseCommand { Excuse = excuse };
        await _mediator.Send(command);
        return Ok();
    }

    [HttpGet("meeting-escape-favorites")]
    public async Task<IActionResult> GetFavorites()
    {
        var query = new GetFavoriteExcusesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}

