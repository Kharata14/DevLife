using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.BugChase.Dtos;
public class LeaderboardEntryDto
{
    public string Username { get; set; }
    public int HighScore { get; set; }
}