using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Domain.Entities;
public class ExcuseLog
{
    public Guid Id { get; set; }
    public string ExcuseText { get; set; }
    public string Category { get; set; }
    public Guid UserId { get; set; }
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
}