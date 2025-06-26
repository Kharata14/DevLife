using DevLife.Application.Interfaces;
using DevLife.Domain.Entities;
using DevLife.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Infrastructure.Repositories;
public class ExcuseLogRepository : IExcuseLogRepository
{
    private readonly ApplicationDbContext _context;

    public ExcuseLogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task LogAsync(ExcuseLog log, CancellationToken cancellationToken = default)
    {
        await _context.ExcuseLogs.AddAsync(log, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}