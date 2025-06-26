using DevLife.Application.Features.BugChase.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.BugChase.Notifications;
public class ScoreSubmittedNotification : INotification
{
    public List<LeaderboardEntryDto> Leaderboard { get; set; }
}
