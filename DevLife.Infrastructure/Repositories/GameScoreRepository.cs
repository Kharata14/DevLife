using DevLife.Application.Features.BugChase.Dtos;
using DevLife.Application.Interfaces;
using DevLife.Domain.Entities;
using DevLife.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Infrastructure.Repositories;
public class GameScoreRepository : IGameScoreRepository
{
    private readonly ApplicationDbContext _context;

    public GameScoreRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task AddBugChaseScoreAsync(BugChaseGameScore score)
    {
        await _context.BugChaseGameScores.AddAsync(score);
        await _context.SaveChangesAsync();
    }
    public async Task<List<LeaderboardEntryDto>> GetLeaderboardAsync(int topN = 10)
    {
        var leaderboard = await _context.BugChaseGameScores
            .GroupBy(s => s.UserId)
            .Select(g => new
            {
                UserId = g.Key,
                HighScore = g.Max(s => s.Score)
            })
            .OrderByDescending(x => x.HighScore)
            .Take(topN)
            .Join(
                _context.Users,
                score => score.UserId,
                user => user.Id,
                (score, user) => new LeaderboardEntryDto
                {
                    Username = user.Username,
                    HighScore = score.HighScore
                })
            .ToListAsync();

        return leaderboard;
    }
    public async Task<int> GetHighScoreForUserAsync(Guid userId)
    {
        var userScores = _context.BugChaseGameScores
            .Where(s => s.UserId == userId);

        if (!await userScores.AnyAsync())
        {
            return 0;
        }

        return await userScores.MaxAsync(s => s.Score);
    }
}