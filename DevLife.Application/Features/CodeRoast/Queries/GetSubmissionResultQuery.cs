using DevLife.Domain.Entities;
using MediatR;
using System;

namespace DevLife.Application.Features.CodeRoast.Queries
{
    public class GetSubmissionResultQuery : IRequest<CodeRoastSubmission>
    {
        public Guid SubmissionId { get; set; }
    }
}