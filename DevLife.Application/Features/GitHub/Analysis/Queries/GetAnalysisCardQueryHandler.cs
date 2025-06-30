// In: Application/Features/GitHub/Analysis/Queries/GetAnalysisCardQueryHandler.cs
using DevLife.Application.Features.GitHub.Analysis.Dtos;
using DevLife.Application.Interfaces;
using MediatR;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DevLife.Application.Features.GitHub.Analysis.Queries
{
    public class GetAnalysisCardQueryHandler : IRequestHandler<GetAnalysisCardQuery, byte[]>
    {
        private readonly IGitHubAnalysisJobRepository _jobRepository;
        private readonly ICardGenerationService _cardService;

        public GetAnalysisCardQueryHandler(IGitHubAnalysisJobRepository jobRepository, ICardGenerationService cardService)
        {
            _jobRepository = jobRepository;
            _cardService = cardService;
        }

        public async Task<byte[]> Handle(GetAnalysisCardQuery request, CancellationToken cancellationToken)
        {
            var job = await _jobRepository.GetByIdAsync(request.JobId);
            if (job?.Status != "Completed" || string.IsNullOrEmpty(job.ResultJson))
            {
                throw new System.InvalidOperationException("Analysis is not completed or result is missing.");
            }

            var persona = JsonSerializer.Deserialize<PersonaResultDto>(job.ResultJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return await _cardService.GenerateCardAsync(persona);
        }
    }
}