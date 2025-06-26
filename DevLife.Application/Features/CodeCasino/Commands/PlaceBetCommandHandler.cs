using DevLife.Application.Features.CodeCasino.Dtos;
using DevLife.Application.Features.CodeCasino.Queries;
using DevLife.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DevLife.Application.Features.CodeCasino.Commands;
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
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("leaderboard")]
    public async Task<IActionResult> GetLeaderboard()
    {
        var query = new GetCasinoLeaderboardQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
