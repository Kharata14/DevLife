using DevLife.Application.Features.CodeRoast.Dtos;
using System.Threading.Tasks;

namespace DevLife.Application.Interfaces
{
    public interface ICodingChallengeProvider
    {
        Task<CodingChallengeDto> GetChallengeAsync(string language, string difficulty);
    }
}