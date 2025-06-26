using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.CodeCasino.Dtos;
public class CasinoLeaderboardEntryDto
{
    public string Username { get; set; }
    public int Points { get; set; }
    public int LongestStreak { get; set; }
}