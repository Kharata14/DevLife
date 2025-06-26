using DevLife.Application.Features.BugChase.Notifications;
using DevLife.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.Achievements;
public class GamePlayedEventHandler : INotificationHandler<GamePlayedEvent>
{
    private readonly IEnumerable<IAchievementChecker> _checkers;
    private readonly IUserAchievementRepository _achievementRepository;

    public GamePlayedEventHandler(
        IEnumerable<IAchievementChecker> checkers,
        IUserAchievementRepository achievementRepository)
    {
        _checkers = checkers;
        _achievementRepository = achievementRepository;
    }

    public async Task Handle(GamePlayedEvent notification, CancellationToken cancellationToken)
    {
        foreach (var checker in _checkers)
        {
            var newAchievementId = await checker.Check(notification);

            if (!string.IsNullOrEmpty(newAchievementId))
            {
                await _achievementRepository.AwardAchievementAsync(notification.UserId, newAchievementId);
            }
        }
    }
}