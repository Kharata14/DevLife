using DevLife.Application.Interfaces;
using DevLife.Domain.Entities;
using DevLife.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DevLife.Infrastructure.Repositories
{
    public class CodeRoastSubmissionRepository : ICodeRoastSubmissionRepository
    {
        private readonly ApplicationDbContext _context;

        public CodeRoastSubmissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CodeRoastSubmission?> GetByIdAsync(Guid submissionId)
        {
            return await _context.CodeRoastSubmissions
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == submissionId);
        }
    }
}