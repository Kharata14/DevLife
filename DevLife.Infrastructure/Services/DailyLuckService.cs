using DevLife.Application.Interfaces;
using DevLife.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Infrastructure.Services;
public class DailyLuckService : IDailyLuckService
{
    public ZodiacSign GetLuckySignOfTheDay()
    {
        var totalZodiacs = Enum.GetNames(typeof(ZodiacSign)).Length;
        var dayOfYear = DateTime.UtcNow.DayOfYear;
        var signIndex = dayOfYear % totalZodiacs;
        return (ZodiacSign)signIndex;
    }
}
