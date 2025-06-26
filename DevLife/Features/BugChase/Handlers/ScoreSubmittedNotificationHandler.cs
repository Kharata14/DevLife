using DevLife.Api.Hubs;
using DevLife.Application.Features.BugChase.Commands;
using DevLife.Application.Features.BugChase.Notifications;
using DevLife.Application.Features.BugChase.Queries;
using DevLife.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace DevLife.Api.Features.BugChase.Handlers;
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
        var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        await _scoreRepository.AddBugChaseScoreAsync(new Domain.Entities.BugChaseGameScore
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Score = request.Score
        });

        var leaderboard = await _mediator.Send(new GetLeaderboardQuery(), cancellationToken);

        await _mediator.Publish(new ScoreSubmittedNotification { Leaderboard = leaderboard }, cancellationToken);
    }
}