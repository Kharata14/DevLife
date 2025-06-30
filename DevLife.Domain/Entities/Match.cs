using System;
namespace DevLife.Domain.Entities
{
    public class Match
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string MatchedUserProfileId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}