using DevLife.Application.Features.GitHub.Analysis.Dtos;
using DevLife.Application.Interfaces;
using MediatR;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DevLife.Application.Features.GitHub.Analysis.Queries
{
    public class GetAnalysisResultQueryHandler : IRequestHandler<GetAnalysisResultQuery, AnalysisResultDto>
    {
        private readonly IGitHubAnalysisJobRepository _jobRepository;

        public GetAnalysisResultQueryHandler(IGitHubAnalysisJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<AnalysisResultDto> Handle(GetAnalysisResultQuery request, CancellationToken cancellationToken)
        {
            var job = await _jobRepository.GetByIdAsync(request.JobId);
            if (job == null)
            {
                return null;
            }

            return new AnalysisResultDto
            {
                JobId = job.Id,
                Status = job.Status,
                CreatedAt = job.CreatedAt,
                CompletedAt = job.CompletedAt,
                Result = string.IsNullOrEmpty(job.ResultJson) ? null : JsonDocument.Parse(job.ResultJson).RootElement
            };
        }
    }
}