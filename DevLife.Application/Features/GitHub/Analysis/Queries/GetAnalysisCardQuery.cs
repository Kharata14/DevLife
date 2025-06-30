using MediatR;
using System;

namespace DevLife.Application.Features.GitHub.Analysis.Queries
{
    public class GetAnalysisCardQuery : IRequest<byte[]>
    {
        public Guid JobId { get; set; }
    }
}