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
public class UserAchievementRepository : IUserAchievementRepository
{
    private readonly ApplicationDbContext _context;

    public UserAchievementRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AwardAchievementAsync(Guid userId, string achievementId)
    {
        var alreadyExists = await HasAchievementAsync(userId, achievementId);
        if (!alreadyExists)
        {
            var userAchievement = new UserAchievement
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                AchievementId = achievementId
            };
            await _context.UserAchievements.AddAsync(userAchievement);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> HasAchievementAsync(Guid userId, string achievementId)
    {
        return await _context.UserAchievements
            .AnyAsync(ua => ua.UserId == userId && ua.AchievementId == achievementId);
    }
}