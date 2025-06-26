using MediatR;
using System;

namespace DevLife.Application.Features.CodeRoast.Notifications
{
    public class SolutionSubmittedEvent : INotification
    {
        public Guid SubmissionId { get; set; }
        public Guid UserId { get; set; }
        public string ChallengeId { get; set; }
        public string SourceCode { get; set; }
    }
}