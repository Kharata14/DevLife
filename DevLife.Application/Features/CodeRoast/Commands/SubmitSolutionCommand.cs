using MediatR;
using System;

namespace DevLife.Application.Features.CodeRoast.Commands
{
    public class SubmitSolutionCommand : IRequest<Guid>
    {
        public string ChallengeId { get; set; }
        public string SourceCode { get; set; }
    }
}