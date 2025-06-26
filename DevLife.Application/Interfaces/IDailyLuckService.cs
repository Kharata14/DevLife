using DevLife.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Interfaces;
public interface IDailyLuckService
{
    ZodiacSign GetLuckySignOfTheDay();
}
