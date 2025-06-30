using DevLife.Application.Features.GitHub.Analysis.Dtos;
using System.Threading.Tasks;

namespace DevLife.Application.Interfaces
{
    public interface ICardGenerationService
    {
        Task<byte[]?> GenerateCardAsync(PersonaResultDto persona);
    }
}