using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Domain.Entities;
public class CasinoGame
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public int BetAmount { get; set; }
    public int Payout { get; set; }
    public string ChosenSnippetId { get; set; }
    public bool WasCorrect { get; set; }
    public DateTime PlayedAt { get; set; } = DateTime.UtcNow;
}