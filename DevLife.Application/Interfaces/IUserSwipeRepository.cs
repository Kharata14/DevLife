using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace DevLife.Application.Interfaces
{
    public interface IUserSwipeRepository
    {
        Task<List<string>> GetSwipedProfileIdsAsync(Guid userId);
    }
}