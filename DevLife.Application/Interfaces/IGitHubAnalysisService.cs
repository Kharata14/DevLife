using System.Threading.Tasks;

namespace DevLife.Application.Interfaces
{
    public interface IGitHubAnalysisService
    {
        Task<string> AnalyzeRepositoryAsync(string owner, string repoName, string accessToken);
    }
}