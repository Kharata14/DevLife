using System;

namespace DevLife.Domain.Entities
{
    public class CodeRoastSubmission
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ChallengeId { get; set; }
        public string SubmittedCode { get; set; }
        public string Status { get; set; }
        public string? ExecutionOutput { get; set; }
        public string? AiRoast { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
    }
}