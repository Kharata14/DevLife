// In: Application/Interfaces/ICodeRoastSubmissionRepository.cs
using DevLife.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace DevLife.Application.Interfaces
{
    public interface ICodeRoastSubmissionRepository
    {
        Task<CodeRoastSubmission?> GetByIdAsync(Guid submissionId);
    }
}