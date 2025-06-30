using System;

namespace DevLife.Domain.Entities
{
    public class GitHubAnalysisJob
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string RepositoryOwner { get; set; }
        public string RepositoryName { get; set; }
        public string Status { get; set; }
        public string? ResultJson { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
    }
}