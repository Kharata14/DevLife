using MediatR;
using System;

namespace DevLife.Application.Features.GitHub.Analysis.Commands
{
    public class StartAnalysisCommand : IRequest<Guid>
    {
        public string Owner { get; set; }
        public string RepoName { get; set; }
    }
}