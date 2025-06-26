using DevLife.Application.Features.BugChase.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Interfaces;
public interface IAchievementChecker
{
    Task<string?> Check(GamePlayedEvent gameEvent);
}