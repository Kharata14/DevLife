using DevLife.Application.Features.CodeRoast.Dtos;
using System.Threading.Tasks;

namespace DevLife.Application.Interfaces
{
    public interface ICodeExecutionService
    {
        Task<CodeExecutionResultDto> ExecuteCodeAsync(string sourceCode, string language);
    }
}