using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Domain.Entities;
public class UserAchievement
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string AchievementId { get; set; }
    public DateTime UnlockedAt { get; set; } = DateTime.UtcNow;
}
