using DevLife.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace DevLife.Application.Interfaces
{
    public interface IGitHubAnalysisJobRepository
    {
        Task<GitHubAnalysisJob?> GetByIdAsync(Guid jobId);
    }
}