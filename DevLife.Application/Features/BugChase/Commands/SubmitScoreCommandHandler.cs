using DevLife.Application.Features.BugChase.Notifications;
using DevLife.Application.Features.BugChase.Queries;
using DevLife.Application.Interfaces;
using DevLife.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.BugChase.Commands;
public class SubmitScoreCommandHandler : IRequestHandler<SubmitScoreCommand>
{
    private readonly IGameScoreRepository _scoreRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMediator _mediator;

    public SubmitScoreCommandHandler(
        IGameScoreRepository scoreRepository,
        IHttpContextAccessor httpContextAccessor,
        IMediator mediator)
    {
        _scoreRepository = scoreRepository;
        _httpContextAccessor = httpContextAccessor;
        _mediator = mediator;
    }
    public async Task Handle(SubmitScoreCommand request, CancellationToken cancellationToken)
    {
        var userIdString = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out var userId))
        {
            return;
        }
        var newScore = new BugChaseGameScore
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Score = request.Score,
            PlayedAt = DateTime.UtcNow
        };
        await _scoreRepository.AddBugChaseScoreAsync(newScore);
        await _mediator.Publish(new GamePlayedEvent { UserId = userId, Score = request.Score }, cancellationToken);
    }
}