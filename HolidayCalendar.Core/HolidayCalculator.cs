using static HolidayCalendar.Core.Constants;

namespace HolidayCalendar.Core;

public static class HolidayCalculator
{
    public static DateTime CalculateFixedHoliday(int year, int month, int day)
    {
        ValidateDateYear(year);

        return new DateTime(year, month, day);
    }

    public static DateTime CalculateNthWeekdayOfMonth(int year, int month, DayOfWeek dayOfWeek, int occurrence)
    {
        ValidateDateYear(year);

        if (occurrence is < 1 or > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(occurrence), occurrence,
                "Occurrence must be between 1 and 5.");
        }

        var firstOfMonth = new DateTime(year, month, 1);
        var dayOffset = ((int)dayOfWeek - (int)firstOfMonth.DayOfWeek + 7) % 7;
        var result = firstOfMonth.AddDays(dayOffset + 7 * (occurrence - 1));

        if (result.Month != month)
        {
            throw new ArgumentOutOfRangeException(nameof(occurrence), occurrence,
                "The requested weekday occurrence does not exist in the specified month.");
        }

        return result;
    }

    public static DateTime CalculateLastWeekdayOfMonth(int year, int month, DayOfWeek dayOfWeek)
    {
        ValidateDateYear(year);

        var lastOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
        var dayOffset = ((int)lastOfMonth.DayOfWeek - (int)dayOfWeek + 7) % 7;

        return lastOfMonth.AddDays(-dayOffset);
    }

    public static DateTime CalculateObservedHoliday(DateTime actualDate)
    {
        return actualDate.DayOfWeek switch
        {
            DayOfWeek.Saturday => actualDate.AddDays(-1),
            DayOfWeek.Sunday => actualDate.AddDays(1),
            _ => actualDate
        };
    }

    public static DateTime CalculateMartinLutherKingJrDay(int year)
    {
        ValidateFederalHolidayYear(year, FederalMartinLutherKingJrDayStartYear, "Martin Luther King Jr. Day");
        return CalculateNthWeekdayOfMonth(year, January, DayOfWeek.Monday, 3);
    }

    public static DateTime CalculateNewYearsDay(int year)
    {
        ValidateFederalHolidayYear(year, FederalNewYearsDayStartYear, "New Year's Day");
        return CalculateFixedHoliday(year, January, 1);
    }

    public static DateTime CalculateObservedNewYearsDay(int year)
    {
        return CalculateObservedHoliday(CalculateNewYearsDay(year));
    }

    public static DateTime CalculatePresidentsDay(int year)
    {
        ValidateFederalHolidayYear(year, FederalPresidentsDayStartYear, "Presidents Day");

        if (year < HistoricalPresidentsDayMondayObservanceStartYear)
        {
            return CalculateFixedHoliday(year, February, 22);
        }

        return CalculateNthWeekdayOfMonth(year, February, DayOfWeek.Monday, 3);
    }

    public static DateTime CalculateJuneteenth(int year)
    {
        ValidateFederalHolidayYear(year, FederalJuneteenthStartYear, "Juneteenth");
        return CalculateFixedHoliday(year, June, 19);
    }

    public static DateTime CalculateObservedJuneteenth(int year)
    {
        return CalculateObservedHoliday(CalculateJuneteenth(year));
    }

    public static DateTime CalculateMemorialDay(int year)
    {
        ValidateFederalHolidayYear(year, HistoricalMemorialDayStartYear, "Memorial Day");

        if (year < HistoricalMemorialDayMondayObservanceStartYear)
        {
            return CalculateFixedHoliday(year, May, 30);
        }

        return CalculateLastWeekdayOfMonth(year, May, DayOfWeek.Monday);
    }

    public static DateTime CalculateIndependenceDay(int year)
    {
        ValidateFederalHolidayYear(year, FederalIndependenceDayStartYear, "Independence Day");
        return CalculateFixedHoliday(year, July, 4);
    }

    public static DateTime CalculateObservedIndependenceDay(int year)
    {
        return CalculateObservedHoliday(CalculateIndependenceDay(year));
    }

    public static DateTime CalculateLaborDay(int year)
    {
        ValidateFederalHolidayYear(year, FederalLaborDayStartYear, "Labor Day");
        return CalculateNthWeekdayOfMonth(year, September, DayOfWeek.Monday, 1);
    }

    public static DateTime CalculateColumbusDay(int year)
    {
        ValidateFederalHolidayYear(year, FederalColumbusDayStartYear, "Columbus Day");

        if (year < HistoricalColumbusDayMondayObservanceStartYear)
        {
            return CalculateFixedHoliday(year, October, 12);
        }

        return CalculateNthWeekdayOfMonth(year, October, DayOfWeek.Monday, 2);
    }

    public static DateTime CalculateVeteransDay(int year)
    {
        ValidateFederalHolidayYear(year, FederalVeteransDayStartYear, "Veterans Day");

        if (year >= FederalVeteransDayMondayObservanceStartYear && year <= FederalVeteransDayMondayObservanceEndYear)
        {
            return CalculateNthWeekdayOfMonth(year, October, DayOfWeek.Monday, 4);
        }

        return CalculateFixedHoliday(year, November, 11);
    }

    public static DateTime CalculateObservedVeteransDay(int year)
    {
        return CalculateObservedHoliday(CalculateVeteransDay(year));
    }

    public static DateTime CalculateThanksgiving(int year)
    {
        ValidateFederalHolidayYear(year, FederalThanksgivingStartYear, "Thanksgiving");
        return CalculateNthWeekdayOfMonth(year, November, DayOfWeek.Thursday, 4);
    }

    public static DateTime CalculateChristmasDay(int year)
    {
        ValidateFederalHolidayYear(year, FederalChristmasDayStartYear, "Christmas Day");
        return CalculateFixedHoliday(year, December, 25);
    }

    public static DateTime CalculateObservedChristmasDay(int year)
    {
        return CalculateObservedHoliday(CalculateChristmasDay(year));
    }

    public static DateTime CalculateEasterSunday(int year)
    {
        // This specific algorithm is accurate for all years after 1582 (the start of the Gregorian calendar)
        // and remains valid for thousands of years.
        if (year < MinGregorianYear)
        {
            throw new ArgumentOutOfRangeException(nameof(year), year,
                $"Gregorian Easter calculations are only supported for years {MinGregorianYear} and later.");
        }
        
        // The Golden Number is the position of a year within the 19-year Metonic cycle. It is based on
        // the astronomical discovery that the Moon's phases repeat on nearly the same calendar dates
        // every 19 years.
        var aMetonicCycle = year % MetonicCycleYears;
        
        var bCentury = year / 100;
        var cYearInCentury = year % 100;
        var dLeapYear = bCentury / 4;
        var eSkippedLeapYear = bCentury % 4;
        
        // Lunar Corrections: These apply the "lunar equation" (correcting the Metonic cycle's slight inaccuracy) and
        // the "solar equation" (adjusting for skipped leap years) to keep the "ecclesiastical moon" in sync with
        // the actual moon.
        var fLunarEquation = (bCentury + LunarEquationNumerator) / LunarEquationDenominator;
        var gSolarEquation = (bCentury - fLunarEquation + 1) / 3;
        
        // Represents the "age" of the moon on January 1st—essentially how many days have passed since the last new moon
        var hEpact = (MetonicCycleYears * aMetonicCycle + bCentury - dLeapYear - gSolarEquation + HistoricalOffset) % 30;
        
        // (Day of the Week): These variables determine the day of the week for the Paschal full moon so the
        // algorithm can "find" the following Sunday.
        
        // This is the quotient of the year within the century divided by 4 (c / 4). It tracks how many leap years
        // have occurred in the current century (e.g., for 2026, c is 26, so i is 6)
        var iCenturyQuadrant = cYearInCentury / 4;
        
        // This is the remainder of the year within the century divided by 4 (c % 4). It identifies the year's
        // position in the 4-year leap year cycle. For 2026, k is 2.
        var kLeapYearRemainder = cYearInCentury % 4;
        
        // This is the most critical of the three. It calculates the "offset" needed to reach the next Sunday
        // after the full moon
        var lDominicalAdjustment = (AuxiliaryConstant + 2 * eSkippedLeapYear + 2 * iCenturyQuadrant - hEpact - kLeapYearRemainder) % 7;
        
        // A rare correction factor used to ensure Easter never falls on the same date twice in the same 19-year
        // cycle if the epact is 24 or 25.
        var mCycleCorrection = (aMetonicCycle + 11 * hEpact + PreventSameDateTwice * lDominicalAdjustment) / RefinementFactor;
    
        // Calculate month and day
        var month = (hEpact + lDominicalAdjustment - 7 * mCycleCorrection + FinalDateMapping) / 31;
        var day = (hEpact + lDominicalAdjustment - 7 * mCycleCorrection + FinalDateMapping) % 31 + 1;

        return new DateTime(year, month, day);
    }

    public static DateTime CalculateGoodFriday(int year)
    {
        return CalculateEasterSunday(year).AddDays(-2);
    }

    public static DateTime CalculatePentecostSunday(int year)
    {
        return CalculateEasterSunday(year).AddDays(49);
    }

    private static void ValidateDateYear(int year)
    {
        if (year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year)
        {
            throw new ArgumentOutOfRangeException(nameof(year), year,
                $"Year must be between {DateTime.MinValue.Year} and {DateTime.MaxValue.Year}.");
        }
    }

    private static void ValidateFederalHolidayYear(int year, int firstSupportedYear, string holidayName)
    {
        ValidateDateYear(year);

        if (year < firstSupportedYear)
        {
            throw new ArgumentOutOfRangeException(nameof(year), year,
                $"{holidayName} is only supported for federal holiday calculations in {firstSupportedYear} and later.");
        }
    }
}
