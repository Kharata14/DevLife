using DevLife.Application.Features.BugChase.Dtos;
using DevLife.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Interfaces;
public interface IGameScoreRepository
{
    Task AddBugChaseScoreAsync(BugChaseGameScore score);
    Task<List<LeaderboardEntryDto>> GetLeaderboardAsync(int topN = 10);
    Task<int> GetHighScoreForUserAsync(Guid userId);
}
