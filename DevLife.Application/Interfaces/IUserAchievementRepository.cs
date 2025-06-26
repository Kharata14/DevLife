using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Interfaces;
public interface IUserAchievementRepository
{
    Task<bool> HasAchievementAsync(Guid userId, string achievementId);
    Task AwardAchievementAsync(Guid userId, string achievementId);
}