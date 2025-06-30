using DevLife.Application.Features.GitHub.Dtos;
using System.Threading.Tasks;

namespace DevLife.Application.Interfaces
{
    public interface IGitHubService
    {
        Task<string> GetAccessTokenAsync(string temporaryCode);
        Task<List<GitHubRepoDto>> GetUserRepositoriesAsync(string accessToken);
    }
}