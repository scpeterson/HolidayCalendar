using static HolidayCalendar.Core.Constants;

namespace HolidayCalendar.Core;

/// <summary>
/// Provides reusable date rules and holiday calculations for United States' federal holidays and supported religious holidays.
/// </summary>
public static class HolidayCalculator
{
    private static readonly IReadOnlyDictionary<string, Func<int, Holiday>> FederalHolidayFactories =
        new Dictionary<string, Func<int, Holiday>>(StringComparer.OrdinalIgnoreCase)
        {
            [HolidayNames.NewYearsDay] = year => new Holiday(
                HolidayNames.NewYearsDay,
                CalculateNewYearsDay(year),
                CalculateObservedNewYearsDay(year),
                HolidayCategory.Federal),
            [HolidayNames.MartinLutherKingJrDay] = year => new Holiday(
                HolidayNames.MartinLutherKingJrDay,
                CalculateMartinLutherKingJrDay(year),
                CalculateMartinLutherKingJrDay(year),
                HolidayCategory.Federal),
            [HolidayNames.PresidentsDay] = year => new Holiday(
                HolidayNames.PresidentsDay,
                CalculatePresidentsDay(year),
                CalculatePresidentsDay(year),
                HolidayCategory.Federal),
            [HolidayNames.MemorialDay] = year => new Holiday(
                HolidayNames.MemorialDay,
                CalculateMemorialDay(year),
                CalculateMemorialDay(year),
                HolidayCategory.Federal),
            [HolidayNames.Juneteenth] = year => new Holiday(
                HolidayNames.Juneteenth,
                CalculateJuneteenth(year),
                CalculateObservedJuneteenth(year),
                HolidayCategory.Federal),
            [HolidayNames.IndependenceDay] = year => new Holiday(
                HolidayNames.IndependenceDay,
                CalculateIndependenceDay(year),
                CalculateObservedIndependenceDay(year),
                HolidayCategory.Federal),
            [HolidayNames.LaborDay] = year => new Holiday(
                HolidayNames.LaborDay,
                CalculateLaborDay(year),
                CalculateLaborDay(year),
                HolidayCategory.Federal),
            [HolidayNames.ColumbusDay] = year => new Holiday(
                HolidayNames.ColumbusDay,
                CalculateColumbusDay(year),
                CalculateColumbusDay(year),
                HolidayCategory.Federal),
            [HolidayNames.VeteransDay] = year => new Holiday(
                HolidayNames.VeteransDay,
                CalculateVeteransDay(year),
                CalculateObservedVeteransDay(year),
                HolidayCategory.Federal),
            [HolidayNames.Thanksgiving] = year => new Holiday(
                HolidayNames.Thanksgiving,
                CalculateThanksgiving(year),
                CalculateThanksgiving(year),
                HolidayCategory.Federal),
            [HolidayNames.ChristmasDay] = year => new Holiday(
                HolidayNames.ChristmasDay,
                CalculateChristmasDay(year),
                CalculateObservedChristmasDay(year),
                HolidayCategory.Federal)
        };

    private static readonly IReadOnlyDictionary<string, Func<int, Holiday>> ReligiousHolidayFactories =
        new Dictionary<string, Func<int, Holiday>>(StringComparer.OrdinalIgnoreCase)
        {
            [HolidayNames.AshWednesday] = year => new Holiday(
                HolidayNames.AshWednesday,
                CalculateAshWednesday(year),
                CalculateAshWednesday(year),
                HolidayCategory.Religious),
            [HolidayNames.PalmSunday] = year => new Holiday(
                HolidayNames.PalmSunday,
                CalculatePalmSunday(year),
                CalculatePalmSunday(year),
                HolidayCategory.Religious),
            [HolidayNames.MaundyThursday] = year => new Holiday(
                HolidayNames.MaundyThursday,
                CalculateMaundyThursday(year),
                CalculateMaundyThursday(year),
                HolidayCategory.Religious),
            [HolidayNames.GoodFriday] = year => new Holiday(
                HolidayNames.GoodFriday,
                CalculateGoodFriday(year),
                CalculateGoodFriday(year),
                HolidayCategory.Religious),
            [HolidayNames.HolySaturday] = year => new Holiday(
                HolidayNames.HolySaturday,
                CalculateHolySaturday(year),
                CalculateHolySaturday(year),
                HolidayCategory.Religious),
            [HolidayNames.EasterSunday] = year => new Holiday(
                HolidayNames.EasterSunday,
                CalculateEasterSunday(year),
                CalculateEasterSunday(year),
                HolidayCategory.Religious),
            [HolidayNames.EasterMonday] = year => new Holiday(
                HolidayNames.EasterMonday,
                CalculateEasterMonday(year),
                CalculateEasterMonday(year),
                HolidayCategory.Religious),
            [HolidayNames.AscensionDay] = year => new Holiday(
                HolidayNames.AscensionDay,
                CalculateAscensionDay(year),
                CalculateAscensionDay(year),
                HolidayCategory.Religious),
            [HolidayNames.PentecostSunday] = year => new Holiday(
                HolidayNames.PentecostSunday,
                CalculatePentecostSunday(year),
                CalculatePentecostSunday(year),
                HolidayCategory.Religious),
            [HolidayNames.PentecostMonday] = year => new Holiday(
                HolidayNames.PentecostMonday,
                CalculatePentecostMonday(year),
                CalculatePentecostMonday(year),
                HolidayCategory.Religious)
        };

    /// <summary>
    /// Gets a single federal holiday for the supplied year.
    /// </summary>
    /// <param name="name">The holiday name to resolve. Matching is case-insensitive.</param>
    /// <param name="year">The year for which the holiday should be calculated.</param>
    /// <returns>The matching federal holiday entry.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is blank or does not identify a supported federal holiday.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the named holiday is not supported for the requested year.</exception>
    public static Holiday GetFederalHoliday(string name, int year)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        var normalizedName = name.Trim();

        if (FederalHolidayFactories.TryGetValue(normalizedName, out var holidayFactory))
        {
            return holidayFactory(year);
        }

        throw new ArgumentException(
            $"Federal holiday '{name}' is not supported.",
            nameof(name));
    }

    /// <summary>
    /// Gets a single supported religious holiday for the supplied year.
    /// </summary>
    /// <param name="name">The holiday name to resolve. Matching is case-insensitive.</param>
    /// <param name="year">The year for which the holiday should be calculated.</param>
    /// <returns>The matching religious holiday entry.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is blank or does not identify a supported religious holiday.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the named holiday is not supported for the requested year.</exception>
    public static Holiday GetReligiousHoliday(string name, int year)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        var normalizedName = name.Trim();

        if (ReligiousHolidayFactories.TryGetValue(normalizedName, out var holidayFactory))
        {
            return holidayFactory(year);
        }

        throw new ArgumentException(
            $"Religious holiday '{name}' is not supported.",
            nameof(name));
    }

    /// <summary>
    /// Gets the supported United States federal holidays for the supplied year.
    /// </summary>
    /// <param name="year">The year for which holidays should be returned.</param>
    /// <returns>An ordered list of federal holidays for the supplied year.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="year"/> is outside the <see cref="DateTime"/> supported range.</exception>
    public static IReadOnlyList<Holiday> GetFederalHolidays(int year)
    {
        ValidateDateYear(year);

        var holidays = new List<Holiday>();

        AddFederalHolidayIfSupported(holidays, year, HolidayNames.NewYearsDay, FederalNewYearsDayStartYear,
            CalculateNewYearsDay, CalculateObservedNewYearsDay);
        AddFederalHolidayIfSupported(holidays, year, HolidayNames.MartinLutherKingJrDay, FederalMartinLutherKingJrDayStartYear,
            CalculateMartinLutherKingJrDay, CalculateMartinLutherKingJrDay);
        AddFederalHolidayIfSupported(holidays, year, HolidayNames.PresidentsDay, FederalPresidentsDayStartYear,
            CalculatePresidentsDay, CalculatePresidentsDay);
        AddFederalHolidayIfSupported(holidays, year, HolidayNames.MemorialDay, HistoricalMemorialDayStartYear,
            CalculateMemorialDay, CalculateMemorialDay);
        AddFederalHolidayIfSupported(holidays, year, HolidayNames.Juneteenth, FederalJuneteenthStartYear,
            CalculateJuneteenth, CalculateObservedJuneteenth);
        AddFederalHolidayIfSupported(holidays, year, HolidayNames.IndependenceDay, FederalIndependenceDayStartYear,
            CalculateIndependenceDay, CalculateObservedIndependenceDay);
        AddFederalHolidayIfSupported(holidays, year, HolidayNames.LaborDay, FederalLaborDayStartYear,
            CalculateLaborDay, CalculateLaborDay);
        AddFederalHolidayIfSupported(holidays, year, HolidayNames.ColumbusDay, FederalColumbusDayStartYear,
            CalculateColumbusDay, CalculateColumbusDay);
        AddFederalHolidayIfSupported(holidays, year, HolidayNames.VeteransDay, FederalVeteransDayStartYear,
            CalculateVeteransDay, CalculateObservedVeteransDay);
        AddFederalHolidayIfSupported(holidays, year, HolidayNames.Thanksgiving, FederalThanksgivingStartYear,
            CalculateThanksgiving, CalculateThanksgiving);
        AddFederalHolidayIfSupported(holidays, year, HolidayNames.ChristmasDay, FederalChristmasDayStartYear,
            CalculateChristmasDay, CalculateObservedChristmasDay);

        return holidays;
    }

    /// <summary>
    /// Gets the next federal holidays on or after the supplied date, ordered by actual holiday date.
    /// </summary>
    /// <param name="fromDate">The date from which upcoming holidays should be returned.</param>
    /// <param name="count">The number of upcoming holidays to return.</param>
    /// <returns>An ordered list of upcoming federal holidays.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="count"/> is less than 1.</exception>
    public static IReadOnlyList<Holiday> GetUpcomingFederalHolidays(DateTime fromDate, int count)
    {
        return GetUpcomingHolidays(fromDate, count, GetFederalHolidays);
    }

    /// <summary>
    /// Gets the supported religious holidays for the supplied year.
    /// </summary>
    /// <param name="year">The year for which holidays should be returned.</param>
    /// <returns>An ordered list of supported religious holidays for the supplied year.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="year"/> is outside the supported Gregorian range.</exception>
    public static IReadOnlyList<Holiday> GetReligiousHolidays(int year)
    {
        return
        [
            GetReligiousHoliday(HolidayNames.AshWednesday, year),
            GetReligiousHoliday(HolidayNames.PalmSunday, year),
            GetReligiousHoliday(HolidayNames.MaundyThursday, year),
            GetReligiousHoliday(HolidayNames.GoodFriday, year),
            GetReligiousHoliday(HolidayNames.HolySaturday, year),
            GetReligiousHoliday(HolidayNames.EasterSunday, year),
            GetReligiousHoliday(HolidayNames.EasterMonday, year),
            GetReligiousHoliday(HolidayNames.AscensionDay, year),
            GetReligiousHoliday(HolidayNames.PentecostSunday, year)
            ,
            GetReligiousHoliday(HolidayNames.PentecostMonday, year)
        ];
    }

    /// <summary>
    /// Gets the next supported religious holidays on or after the supplied date, ordered by actual holiday date.
    /// </summary>
    /// <param name="fromDate">The date from which upcoming holidays should be returned.</param>
    /// <param name="count">The number of upcoming holidays to return.</param>
    /// <returns>An ordered list of upcoming religious holidays.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="count"/> is less than 1.</exception>
    public static IReadOnlyList<Holiday> GetUpcomingReligiousHolidays(DateTime fromDate, int count)
    {
        return GetUpcomingHolidays(fromDate, count, GetReligiousHolidays);
    }

    /// <summary>
    /// Creates a holiday date from a fixed month and day in the supplied year.
    /// </summary>
    public static DateTime CalculateFixedHoliday(int year, int month, int day)
    {
        ValidateDateYear(year);

        return new DateTime(year, month, day);
    }

    /// <summary>
    /// Calculates the nth occurrence of a day of the week within a month.
    /// </summary>
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

    /// <summary>
    /// Calculates the last occurrence of a day of the week within a month.
    /// </summary>
    public static DateTime CalculateLastWeekdayOfMonth(int year, int month, DayOfWeek dayOfWeek)
    {
        ValidateDateYear(year);

        var lastOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
        var dayOffset = ((int)lastOfMonth.DayOfWeek - (int)dayOfWeek + 7) % 7;

        return lastOfMonth.AddDays(-dayOffset);
    }

    /// <summary>
    /// Calculates the observed holiday date for a fixed-date holiday.
    /// </summary>
    public static DateTime CalculateObservedHoliday(DateTime actualDate)
    {
        return actualDate.DayOfWeek switch
        {
            DayOfWeek.Saturday => actualDate.AddDays(-1),
            DayOfWeek.Sunday => actualDate.AddDays(1),
            _ => actualDate
        };
    }

    /// <summary>
    /// Calculates Ash Wednesday for the supplied year.
    /// </summary>
    public static DateTime CalculateAshWednesday(int year)
    {
        return CalculateEasterSunday(year).AddDays(-46);
    }

    /// <summary>
    /// Calculates Palm Sunday for the supplied year.
    /// </summary>
    public static DateTime CalculatePalmSunday(int year)
    {
        return CalculateEasterSunday(year).AddDays(-7);
    }

    /// <summary>
    /// Calculates Maundy Thursday for the supplied year.
    /// </summary>
    public static DateTime CalculateMaundyThursday(int year)
    {
        return CalculateEasterSunday(year).AddDays(-3);
    }

    /// <summary>
    /// Calculates Martin Luther King Jr. Day for the supplied year.
    /// </summary>
    public static DateTime CalculateMartinLutherKingJrDay(int year)
    {
        ValidateFederalHolidayYear(year, FederalMartinLutherKingJrDayStartYear, "Martin Luther King Jr. Day");
        return CalculateNthWeekdayOfMonth(year, January, DayOfWeek.Monday, 3);
    }

    /// <summary>
    /// Calculates New Year's Day for the supplied year.
    /// </summary>
    public static DateTime CalculateNewYearsDay(int year)
    {
        ValidateFederalHolidayYear(year, FederalNewYearsDayStartYear, "New Year's Day");
        return CalculateFixedHoliday(year, January, 1);
    }

    /// <summary>
    /// Calculates the observed New Year's Day for the supplied year.
    /// </summary>
    public static DateTime CalculateObservedNewYearsDay(int year)
    {
        return CalculateObservedHoliday(CalculateNewYearsDay(year));
    }

    /// <summary>
    /// Calculates Presidents Day for the supplied year, including pre-1971 Washington's Birthday handling.
    /// </summary>
    public static DateTime CalculatePresidentsDay(int year)
    {
        ValidateFederalHolidayYear(year, FederalPresidentsDayStartYear, "Presidents Day");

        if (year < HistoricalPresidentsDayMondayObservanceStartYear)
        {
            return CalculateFixedHoliday(year, February, 22);
        }

        return CalculateNthWeekdayOfMonth(year, February, DayOfWeek.Monday, 3);
    }

    /// <summary>
    /// Calculates Juneteenth for the supplied year.
    /// </summary>
    public static DateTime CalculateJuneteenth(int year)
    {
        ValidateFederalHolidayYear(year, FederalJuneteenthStartYear, "Juneteenth");
        return CalculateFixedHoliday(year, June, 19);
    }

    /// <summary>
    /// Calculates the observed Juneteenth date for the supplied year.
    /// </summary>
    public static DateTime CalculateObservedJuneteenth(int year)
    {
        return CalculateObservedHoliday(CalculateJuneteenth(year));
    }

    /// <summary>
    /// Calculates Memorial Day for the supplied year, including the historical fixed-date observance before 1971.
    /// </summary>
    public static DateTime CalculateMemorialDay(int year)
    {
        ValidateFederalHolidayYear(year, HistoricalMemorialDayStartYear, "Memorial Day");

        if (year < HistoricalMemorialDayMondayObservanceStartYear)
        {
            return CalculateFixedHoliday(year, May, 30);
        }

        return CalculateLastWeekdayOfMonth(year, May, DayOfWeek.Monday);
    }

    /// <summary>
    /// Calculates Independence Day for the supplied year.
    /// </summary>
    public static DateTime CalculateIndependenceDay(int year)
    {
        ValidateFederalHolidayYear(year, FederalIndependenceDayStartYear, "Independence Day");
        return CalculateFixedHoliday(year, July, 4);
    }

    /// <summary>
    /// Calculates the observed Independence Day for the supplied year.
    /// </summary>
    public static DateTime CalculateObservedIndependenceDay(int year)
    {
        return CalculateObservedHoliday(CalculateIndependenceDay(year));
    }

    /// <summary>
    /// Calculates Labor Day for the supplied year.
    /// </summary>
    public static DateTime CalculateLaborDay(int year)
    {
        ValidateFederalHolidayYear(year, FederalLaborDayStartYear, "Labor Day");
        return CalculateNthWeekdayOfMonth(year, September, DayOfWeek.Monday, 1);
    }

    /// <summary>
    /// Calculates Columbus Day for the supplied year, including the historical fixed-date observance before 1971.
    /// </summary>
    public static DateTime CalculateColumbusDay(int year)
    {
        ValidateFederalHolidayYear(year, FederalColumbusDayStartYear, "Columbus Day");

        if (year < HistoricalColumbusDayMondayObservanceStartYear)
        {
            return CalculateFixedHoliday(year, October, 12);
        }

        return CalculateNthWeekdayOfMonth(year, October, DayOfWeek.Monday, 2);
    }

    /// <summary>
    /// Calculates Veterans Day for the supplied year, including the 1971-1977 Monday observance period.
    /// </summary>
    public static DateTime CalculateVeteransDay(int year)
    {
        ValidateFederalHolidayYear(year, FederalVeteransDayStartYear, "Veterans Day");

        if (year >= FederalVeteransDayMondayObservanceStartYear && year <= FederalVeteransDayMondayObservanceEndYear)
        {
            return CalculateNthWeekdayOfMonth(year, October, DayOfWeek.Monday, 4);
        }

        return CalculateFixedHoliday(year, November, 11);
    }

    /// <summary>
    /// Calculates the observed Veterans Day for the supplied year.
    /// </summary>
    public static DateTime CalculateObservedVeteransDay(int year)
    {
        return CalculateObservedHoliday(CalculateVeteransDay(year));
    }

    /// <summary>
    /// Calculates Thanksgiving for the supplied year.
    /// </summary>
    public static DateTime CalculateThanksgiving(int year)
    {
        ValidateFederalHolidayYear(year, FederalThanksgivingStartYear, "Thanksgiving");
        return CalculateNthWeekdayOfMonth(year, November, DayOfWeek.Thursday, 4);
    }

    /// <summary>
    /// Calculates Christmas Day for the supplied year.
    /// </summary>
    public static DateTime CalculateChristmasDay(int year)
    {
        ValidateFederalHolidayYear(year, FederalChristmasDayStartYear, "Christmas Day");
        return CalculateFixedHoliday(year, December, 25);
    }

    /// <summary>
    /// Calculates the observed Christmas Day for the supplied year.
    /// </summary>
    public static DateTime CalculateObservedChristmasDay(int year)
    {
        return CalculateObservedHoliday(CalculateChristmasDay(year));
    }

    /// <summary>
    /// Calculates Gregorian Easter Sunday for the supplied year.
    /// </summary>
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

    /// <summary>
    /// Calculates Good Friday for the supplied year.
    /// </summary>
    public static DateTime CalculateGoodFriday(int year)
    {
        return CalculateEasterSunday(year).AddDays(-2);
    }

    /// <summary>
    /// Calculates Holy Saturday for the supplied year.
    /// </summary>
    public static DateTime CalculateHolySaturday(int year)
    {
        return CalculateEasterSunday(year).AddDays(-1);
    }

    /// <summary>
    /// Calculates Easter Monday for the supplied year.
    /// </summary>
    public static DateTime CalculateEasterMonday(int year)
    {
        return CalculateEasterSunday(year).AddDays(1);
    }

    /// <summary>
    /// Calculates Ascension Day for the supplied year.
    /// </summary>
    public static DateTime CalculateAscensionDay(int year)
    {
        return CalculateEasterSunday(year).AddDays(39);
    }

    /// <summary>
    /// Calculates Pentecost Sunday for the supplied year.
    /// </summary>
    public static DateTime CalculatePentecostSunday(int year)
    {
        return CalculateEasterSunday(year).AddDays(49);
    }

    /// <summary>
    /// Calculates Pentecost Monday for the supplied year.
    /// </summary>
    public static DateTime CalculatePentecostMonday(int year)
    {
        return CalculateEasterSunday(year).AddDays(50);
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

    private static IReadOnlyList<Holiday> GetUpcomingHolidays(
        DateTime fromDate,
        int count,
        Func<int, IReadOnlyList<Holiday>> holidayProvider)
    {
        if (count < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be greater than zero.");
        }

        var upcomingHolidays = new List<Holiday>();
        var startDate = fromDate.Date;
        var year = startDate.Year;

        while (upcomingHolidays.Count < count)
        {
            upcomingHolidays.AddRange(
                holidayProvider(year)
                    .Where(holiday => holiday.ActualDate >= startDate));

            year++;
            startDate = new DateTime(year, 1, 1);
        }

        return upcomingHolidays
            .OrderBy(holiday => holiday.ActualDate)
            .Take(count)
            .ToArray();
    }

    private static void AddFederalHolidayIfSupported(
        ICollection<Holiday> holidays,
        int year,
        string name,
        int firstSupportedYear,
        Func<int, DateTime> actualDateCalculator,
        Func<int, DateTime> observedDateCalculator)
    {
        if (year < firstSupportedYear)
        {
            return;
        }

        holidays.Add(new Holiday(
            name,
            actualDateCalculator(year),
            observedDateCalculator(year),
            HolidayCategory.Federal));
    }
}
