using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Features.Dashboard.Queries;

public class DashboardResponse
{
    public string ?WelcomeMessage { get; set; }
    public string ?DailyHoroscope { get; set; }
    public string ?LuckyTechnology { get; set; }
    public int CurrentPoints { get; set; }
}
