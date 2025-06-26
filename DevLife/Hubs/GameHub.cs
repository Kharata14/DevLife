using DevLife.Application.Features.BugChase.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DevLife.Api.Hubs;

[Authorize]
public class GameHub : Hub
{
    private readonly IMediator _mediator;

    public GameHub(IMediator mediator)
    {
        _mediator = mediator;
    }
    public override async Task OnConnectedAsync()
    {
        var leaderboard = await _mediator.Send(new GetLeaderboardQuery());
        await Clients.Caller.SendAsync("InitialLeaderboard", leaderboard);

        await base.OnConnectedAsync();
    }
}