using DevLife.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevLife.Application.Common;
 public static class ZodiacCalculator
{
    public static ZodiacSign Calculate(DateTime dateOfBirth)
    {
        int month = dateOfBirth.Month;
        int day = dateOfBirth.Day;

        if ((month == 3 && day >= 21) || (month == 4 && day <= 19)) return ZodiacSign.Aries;
        if ((month == 4 && day >= 20) || (month == 5 && day <= 20)) return ZodiacSign.Taurus;
        if ((month == 5 && day >= 21) || (month == 6 && day <= 20)) return ZodiacSign.Gemini;
        if ((month == 6 && day >= 21) || (month == 7 && day <= 22)) return ZodiacSign.Cancer;
        if ((month == 7 && day >= 23) || (month == 8 && day <= 22)) return ZodiacSign.Leo;
        if ((month == 8 && day >= 23) || (month == 9 && day <= 22)) return ZodiacSign.Virgo;
        if ((month == 9 && day >= 23) || (month == 10 && day <= 22)) return ZodiacSign.Libra;
        if ((month == 10 && day >= 23) || (month == 11 && day <= 21)) return ZodiacSign.Scorpio;
        if ((month == 11 && day >= 22) || (month == 12 && day <= 21)) return ZodiacSign.Sagittarius;
        if ((month == 12 && day >= 22) || (month == 1 && day <= 19)) return ZodiacSign.Capricorn;
        if ((month == 1 && day >= 20) || (month == 2 && day <= 18)) return ZodiacSign.Aquarius;
        return ZodiacSign.Pisces;
    }
}
