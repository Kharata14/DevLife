using DevLife.Application.Interfaces;
using DevLife.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DevLife.Application.Features.CodeRoast.Queries
{
    public class GetSubmissionResultQueryHandler : IRequestHandler<GetSubmissionResultQuery, CodeRoastSubmission>
    {
        private readonly ICodeRoastSubmissionRepository _submissionRepository;

        public GetSubmissionResultQueryHandler(ICodeRoastSubmissionRepository submissionRepository)
        {
            _submissionRepository = submissionRepository;
        }

        public async Task<CodeRoastSubmission> Handle(GetSubmissionResultQuery request, CancellationToken cancellationToken)
        {
            return await _submissionRepository.GetByIdAsync(request.SubmissionId);
        }
    }
}