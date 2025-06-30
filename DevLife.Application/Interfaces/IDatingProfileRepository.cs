using DevLife.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevLife.Application.Interfaces
{
    public interface IDatingProfileRepository
    {
        Task<List<DatingProfile>> GetPotentialMatchesAsync(User currentUser, List<string> alreadySwipedIds);
    }
}