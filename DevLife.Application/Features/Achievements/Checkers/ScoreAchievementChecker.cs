using DevLife.Application.Features.BugChase.Notifications;
using DevLife.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.Achievements.Checkers;
public class ScoreAchievementChecker : IAchievementChecker
{
    private const string AchievementId = "BUG_SQUASHER_10K";
    private const int RequiredScore = 10000;
    private readonly IUserAchievementRepository _achievementRepository;

    public ScoreAchievementChecker(IUserAchievementRepository achievementRepository)
    {
        _achievementRepository = achievementRepository;
    }

    public async Task<string?> Check(GamePlayedEvent gameEvent)
    {
        if (gameEvent.Score < RequiredScore)
        {
            return null;
        }

        bool alreadyHas = await _achievementRepository.HasAchievementAsync(gameEvent.UserId, AchievementId);

        return alreadyHas ? null : AchievementId;
    }
}