using DevLife.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.Authentication.Commands;
public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
{
    private readonly ISessionService _sessionService;

    public LogoutCommandHandler(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _sessionService.SignOutAsync();
    }
}