using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.CodeCasino.Commands;
public class PlaceBetResultDto
{
    public bool WasCorrect { get; set; }
    public string Explanation { get; set; }
    public int PointsChange { get; set; }
    public int NewTotalPoints { get; set; }
}