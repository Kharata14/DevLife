using DevLife.Application.Features.CodeRoast.Dtos;
using System.Threading.Tasks;

namespace DevLife.Application.Interfaces
{
    public interface IAiCodeRoastService
    {
        Task<string> GetRoastAsync(string challengeDescription, string userCode, CodeExecutionResultDto executionResult);
    }
}