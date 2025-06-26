using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Domain.Entities;
public class BugChaseGameScore
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public int Score { get; set; }
    public DateTime PlayedAt { get; set; } = DateTime.UtcNow;
}

