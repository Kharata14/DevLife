using DevLife.Application.Features.GitHub.Analysis.Notifications;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace DevLife.Application.Features.GitHub.Analysis.Commands
{
    public class StartAnalysisCommandHandler : IRequestHandler<StartAnalysisCommand, Guid>
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StartAnalysisCommandHandler(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<Guid> Handle(StartAnalysisCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var jobId = Guid.NewGuid();
            _ = _mediator.Publish(new AnalysisStartedEvent
            {
                JobId = jobId,
                UserId = userId,
                Owner = request.Owner,
                RepoName = request.RepoName
            }, cancellationToken);
            return Task.FromResult(jobId);
        }
    }
}