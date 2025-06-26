using DevLife.Application.Features.CodeRoast.Notifications;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace DevLife.Application.Features.CodeRoast.Commands
{
    public class SubmitSolutionCommandHandler : IRequestHandler<SubmitSolutionCommand, Guid>
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SubmitSolutionCommandHandler(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<Guid> Handle(SubmitSolutionCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var submissionId = Guid.NewGuid();
            _ = _mediator.Publish(new SolutionSubmittedEvent
            {
                SubmissionId = submissionId,
                UserId = userId,
                ChallengeId = request.ChallengeId,
                SourceCode = request.SourceCode
            }, cancellationToken);
            return Task.FromResult(submissionId);
        }
    }
}