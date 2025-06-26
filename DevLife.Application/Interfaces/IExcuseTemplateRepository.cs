using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Interfaces;
public interface IExcuseTemplateRepository
{
    Task<string> GetRandomTemplateAsync(string category, CancellationToken cancellationToken = default);
}
