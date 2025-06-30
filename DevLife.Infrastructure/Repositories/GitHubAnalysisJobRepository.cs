using DevLife.Application.Interfaces;
using DevLife.Domain.Entities;
using DevLife.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DevLife.Infrastructure.Repositories
{
    public class GitHubAnalysisJobRepository : IGitHubAnalysisJobRepository
    {
        private readonly ApplicationDbContext _context;

        public GitHubAnalysisJobRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GitHubAnalysisJob?> GetByIdAsync(Guid jobId)
        {
            return await _context.GitHubAnalysisJobs.AsNoTracking().FirstOrDefaultAsync(j => j.Id == jobId);
        }
    }
}