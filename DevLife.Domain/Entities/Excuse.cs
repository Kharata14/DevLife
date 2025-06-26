using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Domain.Entities;
public class Excuse
{
    public string Text { get; set; }
    public string Category { get; set; }
    public double BelievabilityScore { get; set; }
}

