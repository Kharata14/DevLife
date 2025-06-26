using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.BugChase.Notifications;
public class GamePlayedEvent : INotification
{
    public Guid UserId { get; set; }
    public int Score { get; set; }
}
