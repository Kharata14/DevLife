using MediatR;
using System;

namespace DevLife.Application.Features.GitHub.Analysis.Notifications
{
    public class AnalysisStartedEvent : INotification
    {
        public Guid JobId { get; set; }
        public Guid UserId { get; set; }
        public string Owner { get; set; }
        public string RepoName { get; set; }
    }
}