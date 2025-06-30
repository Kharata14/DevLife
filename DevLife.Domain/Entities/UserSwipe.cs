using System;
namespace DevLife.Domain.Entities
{
    public class UserSwipe
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string SwipedProfileId { get; set; }
        public bool DidLike { get; set; }
        public DateTime SwipeDate { get; set; } = DateTime.UtcNow;
    }
}