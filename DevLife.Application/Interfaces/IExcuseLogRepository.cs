using DevLife.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Interfaces;
public interface IExcuseLogRepository
{
    Task LogAsync(ExcuseLog log, CancellationToken cancellationToken = default);
}
