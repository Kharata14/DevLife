using MediatR;
using System;
using DevLife.Application.Features.GitHub.Analysis.Dtos;


namespace DevLife.Application.Features.GitHub.Analysis.Queries
{
    public class GetAnalysisResultQuery : IRequest<AnalysisResultDto>
    {
        public Guid JobId { get; set; }
    }
}