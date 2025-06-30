// In: Infrastructure/Repositories/UserSwipeRepository.cs
using DevLife.Application.Interfaces;
using DevLife.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevLife.Infrastructure.Repositories
{
    public class UserSwipeRepository : IUserSwipeRepository
    {
        private readonly ApplicationDbContext _context;
        public UserSwipeRepository(ApplicationDbContext context) => _context = context;

        public async Task<List<string>> GetSwipedProfileIdsAsync(Guid userId)
        {
            return await _context.UserSwipes
                .Where(s => s.UserId == userId)
                .Select(s => s.SwipedProfileId)
                .ToListAsync();
        }
    }
}