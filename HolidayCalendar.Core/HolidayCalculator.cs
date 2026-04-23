using static HolidayCalendar.Core.Constants;

namespace HolidayCalendar.Core;

/// <summary>
/// Provides reusable date rules and holiday calculations for United States' federal holidays and supported Christian religious holidays.
/// </summary>
public static class HolidayCalculator
{
    private static readonly HolidayDefinition[] FederalHolidayDefinitions =
    [
        new(HolidayNames.NewYearsDay, FederalNewYearsDayStartYear, CalculateNewYearsDay, CalculateObservedNewYearsDay, HolidayCategory.Federal),
        new(HolidayNames.MartinLutherKingJrDay, FederalMartinLutherKingJrDayStartYear, CalculateMartinLutherKingJrDay, CalculateMartinLutherKingJrDay, HolidayCategory.Federal),
        new(HolidayNames.PresidentsDay, FederalPresidentsDayStartYear, CalculatePresidentsDay, CalculatePresidentsDay, HolidayCategory.Federal),
        new(HolidayNames.MemorialDay, HistoricalMemorialDayStartYear, CalculateMemorialDay, CalculateMemorialDay, HolidayCategory.Federal),
        new(HolidayNames.Juneteenth, FederalJuneteenthStartYear, CalculateJuneteenth, CalculateObservedJuneteenth, HolidayCategory.Federal),
        new(HolidayNames.IndependenceDay, FederalIndependenceDayStartYear, CalculateIndependenceDay, CalculateObservedIndependenceDay, HolidayCategory.Federal),
        new(HolidayNames.LaborDay, FederalLaborDayStartYear, CalculateLaborDay, CalculateLaborDay, HolidayCategory.Federal),
        new(HolidayNames.ColumbusDay, FederalColumbusDayStartYear, CalculateColumbusDay, CalculateColumbusDay, HolidayCategory.Federal),
        new(HolidayNames.VeteransDay, FederalVeteransDayStartYear, CalculateVeteransDay, CalculateObservedVeteransDay, HolidayCategory.Federal),
        new(HolidayNames.Thanksgiving, FederalThanksgivingStartYear, CalculateThanksgiving, CalculateThanksgiving, HolidayCategory.Federal),
        new(HolidayNames.ChristmasDay, FederalChristmasDayStartYear, CalculateChristmasDay, CalculateObservedChristmasDay, HolidayCategory.Federal)
    ];

    private static readonly HolidayDefinition[] ReligiousHolidayDefinitions =
    [
        new(HolidayNames.Epiphany, MinGregorianYear, CalculateEpiphany, CalculateEpiphany, HolidayCategory.Religious),
        new(HolidayNames.AshWednesday, MinGregorianYear, CalculateAshWednesday, CalculateAshWednesday, HolidayCategory.Religious),
        new(HolidayNames.Annunciation, MinGregorianYear, CalculateAnnunciation, CalculateAnnunciation, HolidayCategory.Religious),
        new(HolidayNames.PalmSunday, MinGregorianYear, CalculatePalmSunday, CalculatePalmSunday, HolidayCategory.Religious),
        new(HolidayNames.MaundyThursday, MinGregorianYear, CalculateMaundyThursday, CalculateMaundyThursday, HolidayCategory.Religious),
        new(HolidayNames.GoodFriday, MinGregorianYear, CalculateGoodFriday, CalculateGoodFriday, HolidayCategory.Religious),
        new(HolidayNames.HolySaturday, MinGregorianYear, CalculateHolySaturday, CalculateHolySaturday, HolidayCategory.Religious),
        new(HolidayNames.EasterSunday, MinGregorianYear, CalculateEasterSunday, CalculateEasterSunday, HolidayCategory.Religious),
        new(HolidayNames.EasterMonday, MinGregorianYear, CalculateEasterMonday, CalculateEasterMonday, HolidayCategory.Religious),
        new(HolidayNames.AscensionDay, MinGregorianYear, CalculateAscensionDay, CalculateAscensionDay, HolidayCategory.Religious),
        new(HolidayNames.PentecostSunday, MinGregorianYear, CalculatePentecostSunday, CalculatePentecostSunday, HolidayCategory.Religious),
        new(HolidayNames.PentecostMonday, MinGregorianYear, CalculatePentecostMonday, CalculatePentecostMonday, HolidayCategory.Religious),
        new(HolidayNames.AllSaintsDay, MinGregorianYear, CalculateAllSaintsDay, CalculateAllSaintsDay, HolidayCategory.Religious),
        new(HolidayNames.AllSoulsDay, MinGregorianYear, CalculateAllSoulsDay, CalculateAllSoulsDay, HolidayCategory.Religious),
        new(HolidayNames.ChristmasEve, MinGregorianYear, CalculateChristmasEve, CalculateChristmasEve, HolidayCategory.Religious),
        new(HolidayNames.ChristmasDay, MinGregorianYear, CalculateChristmasDay, CalculateChristmasDay, HolidayCategory.Religious)
    ];

    private static readonly string[] SupportedFederalHolidayNames = FederalHolidayDefinitions
        .Select(definition => definition.Name)
        .ToArray();

    private static readonly string[] SupportedReligiousHolidayNames = ReligiousHolidayDefinitions
        .Select(definition => definition.Name)
        .ToArray();

    private static readonly IReadOnlyDictionary<string, HolidayDefinition> FederalHolidayDefinitionsByName =
        FederalHolidayDefinitions.ToDictionary(definition => definition.Name, StringComparer.OrdinalIgnoreCase);

    private static readonly IReadOnlyDictionary<string, HolidayDefinition> ReligiousHolidayDefinitionsByName =
        ReligiousHolidayDefinitions.ToDictionary(definition => definition.Name, StringComparer.OrdinalIgnoreCase);

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
        var normalizedName = NormalizeHolidayName(name, HolidayAliases.Federal);

        if (FederalHolidayDefinitionsByName.TryGetValue(normalizedName, out var definition))
        {
            return definition.Create(year);
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
        var normalizedName = NormalizeHolidayName(name, HolidayAliases.Religious);

        if (ReligiousHolidayDefinitionsByName.TryGetValue(normalizedName, out var definition))
        {
            return definition.Create(year);
        }

        throw new ArgumentException(
            $"Religious holiday '{name}' is not supported.",
            nameof(name));
    }

    /// <summary>
    /// Attempts to get a single federal holiday for the supplied year without throwing for unsupported names or years.
    /// </summary>
    /// <param name="name">The holiday name to resolve. Matching is case-insensitive.</param>
    /// <param name="year">The year for which the holiday should be calculated.</param>
    /// <param name="holiday">When this method returns, contains the resolved holiday if successful; otherwise <see langword="null"/>.</param>
    /// <returns><see langword="true"/> when a supported holiday can be resolved for the supplied year; otherwise <see langword="false"/>.</returns>
    public static bool TryGetFederalHoliday(string name, int year, out Holiday? holiday)
    {
        return TryGetHoliday(name, year, HolidayAliases.Federal, FederalHolidayDefinitionsByName, out holiday);
    }

    /// <summary>
    /// Attempts to get a single supported religious holiday for the supplied year without throwing for unsupported names or years.
    /// </summary>
    /// <param name="name">The holiday name to resolve. Matching is case-insensitive.</param>
    /// <param name="year">The year for which the holiday should be calculated.</param>
    /// <param name="holiday">When this method returns, contains the resolved holiday if successful; otherwise <see langword="null"/>.</param>
    /// <returns><see langword="true"/> when a supported holiday can be resolved for the supplied year; otherwise <see langword="false"/>.</returns>
    public static bool TryGetReligiousHoliday(string name, int year, out Holiday? holiday)
    {
        return TryGetHoliday(name, year, HolidayAliases.Religious, ReligiousHolidayDefinitionsByName, out holiday);
    }

    /// <summary>
    /// Gets the canonical names accepted by the federal holiday APIs.
    /// </summary>
    /// <returns>An ordered list of supported federal holiday names.</returns>
    public static IReadOnlyList<string> GetSupportedFederalHolidayNames()
    {
        return SupportedFederalHolidayNames;
    }

    /// <summary>
    /// Gets the canonical names accepted by the religious holiday APIs.
    /// </summary>
    /// <returns>An ordered list of supported religious holiday names.</returns>
    public static IReadOnlyList<string> GetSupportedReligiousHolidayNames()
    {
        return SupportedReligiousHolidayNames;
    }

    /// <summary>
    /// Gets the friendly aliases accepted by the federal holiday lookup APIs.
    /// </summary>
    /// <returns>A read-only alias map keyed by accepted alias name.</returns>
    public static IReadOnlyDictionary<string, string> GetFederalHolidayAliases()
    {
        return HolidayAliases.Federal;
    }

    /// <summary>
    /// Gets the friendly aliases accepted by the religious holiday lookup APIs.
    /// </summary>
    /// <returns>A read-only alias map keyed by accepted alias name.</returns>
    public static IReadOnlyDictionary<string, string> GetReligiousHolidayAliases()
    {
        return HolidayAliases.Religious;
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

        return FederalHolidayDefinitions
            .Where(definition => definition.IsSupportedInYear(year))
            .Select(definition => definition.Create(year))
            .ToArray();
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
        return GetUpcomingFederalHolidays(fromDate, count, HolidayDateMode.ActualDate);
    }

    /// <summary>
    /// Gets the next federal holidays on or after the supplied date, ordered by the requested date mode.
    /// </summary>
    /// <param name="fromDate">The date from which upcoming holidays should be returned.</param>
    /// <param name="count">The number of upcoming holidays to return.</param>
    /// <param name="dateMode">Whether to evaluate upcoming holidays by actual or observed dates.</param>
    /// <returns>An ordered list of upcoming federal holidays.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="count"/> is less than 1.</exception>
    public static IReadOnlyList<Holiday> GetUpcomingFederalHolidays(DateTime fromDate, int count, HolidayDateMode dateMode)
    {
        return GetUpcomingHolidays(fromDate, count, GetFederalHolidays, dateMode);
    }

    /// <summary>
    /// Gets the supported religious holidays for the supplied year.
    /// </summary>
    /// <param name="year">The year for which holidays should be returned.</param>
    /// <returns>An ordered list of supported religious holidays for the supplied year.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="year"/> is outside the supported Gregorian range.</exception>
    public static IReadOnlyList<Holiday> GetReligiousHolidays(int year)
    {
        return ReligiousHolidayDefinitions
            .Select(definition => definition.Create(year))
            .OrderBy(holiday => holiday.ActualDate)
            .ToArray();
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
        return GetUpcomingReligiousHolidays(fromDate, count, HolidayDateMode.ActualDate);
    }

    /// <summary>
    /// Gets the next supported religious holidays on or after the supplied date, ordered by the requested date mode.
    /// </summary>
    /// <param name="fromDate">The date from which upcoming holidays should be returned.</param>
    /// <param name="count">The number of upcoming holidays to return.</param>
    /// <param name="dateMode">Whether to evaluate upcoming holidays by actual or observed dates.</param>
    /// <returns>An ordered list of upcoming religious holidays.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="count"/> is less than 1.</exception>
    public static IReadOnlyList<Holiday> GetUpcomingReligiousHolidays(DateTime fromDate, int count, HolidayDateMode dateMode)
    {
        return GetUpcomingHolidays(fromDate, count, GetReligiousHolidays, dateMode);
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
    /// Calculates Epiphany for the supplied year.
    /// </summary>
    public static DateTime CalculateEpiphany(int year)
    {
        return CalculateFixedHoliday(year, January, 6);
    }

    /// <summary>
    /// Calculates Ash Wednesday for the supplied year.
    /// </summary>
    public static DateTime CalculateAshWednesday(int year)
    {
        return CalculateEasterSunday(year).AddDays(-46);
    }

    /// <summary>
    /// Calculates Annunciation for the supplied year.
    /// </summary>
    public static DateTime CalculateAnnunciation(int year)
    {
        return CalculateFixedHoliday(year, March, 25);
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
        ValidateFederalHolidayYear(year, FederalMartinLutherKingJrDayStartYear, HolidayNames.MartinLutherKingJrDay);
        return CalculateNthWeekdayOfMonth(year, January, DayOfWeek.Monday, 3);
    }

    /// <summary>
    /// Calculates New Year's Day for the supplied year.
    /// </summary>
    public static DateTime CalculateNewYearsDay(int year)
    {
        ValidateFederalHolidayYear(year, FederalNewYearsDayStartYear, HolidayNames.NewYearsDay);
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
        ValidateFederalHolidayYear(year, FederalPresidentsDayStartYear, HolidayNames.PresidentsDay);

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
        ValidateFederalHolidayYear(year, FederalJuneteenthStartYear, HolidayNames.Juneteenth);
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
        ValidateFederalHolidayYear(year, HistoricalMemorialDayStartYear, HolidayNames.MemorialDay);

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
        ValidateFederalHolidayYear(year, FederalIndependenceDayStartYear, HolidayNames.IndependenceDay);
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
        ValidateFederalHolidayYear(year, FederalLaborDayStartYear, HolidayNames.LaborDay);
        return CalculateNthWeekdayOfMonth(year, September, DayOfWeek.Monday, 1);
    }

    /// <summary>
    /// Calculates Columbus Day for the supplied year, including the historical fixed-date observance before 1971.
    /// </summary>
    public static DateTime CalculateColumbusDay(int year)
    {
        ValidateFederalHolidayYear(year, FederalColumbusDayStartYear, HolidayNames.ColumbusDay);

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
        ValidateFederalHolidayYear(year, FederalVeteransDayStartYear, HolidayNames.VeteransDay);

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
        ValidateFederalHolidayYear(year, FederalThanksgivingStartYear, HolidayNames.Thanksgiving);
        return CalculateNthWeekdayOfMonth(year, November, DayOfWeek.Thursday, 4);
    }

    /// <summary>
    /// Calculates Christmas Day for the supplied year.
    /// </summary>
    public static DateTime CalculateChristmasDay(int year)
    {
        ValidateFederalHolidayYear(year, FederalChristmasDayStartYear, HolidayNames.ChristmasDay);
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

    /// <summary>
    /// Calculates All Saints' Day for the supplied year.
    /// </summary>
    public static DateTime CalculateAllSaintsDay(int year)
    {
        return CalculateFixedHoliday(year, November, 1);
    }

    /// <summary>
    /// Calculates All Souls' Day for the supplied year.
    /// </summary>
    public static DateTime CalculateAllSoulsDay(int year)
    {
        return CalculateFixedHoliday(year, November, 2);
    }

    /// <summary>
    /// Calculates Christmas Eve for the supplied year.
    /// </summary>
    public static DateTime CalculateChristmasEve(int year)
    {
        return CalculateFixedHoliday(year, December, 24);
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

    private static string NormalizeHolidayName(string name, IReadOnlyDictionary<string, string> aliases)
    {
        var normalizedName = name.Trim();
        return aliases.GetValueOrDefault(normalizedName, normalizedName);
    }

    private static bool TryGetHoliday(
        string name,
        int year,
        IReadOnlyDictionary<string, string> aliases,
        IReadOnlyDictionary<string, HolidayDefinition> holidayDefinitions,
        out Holiday? holiday)
    {
        holiday = null;

        if (string.IsNullOrWhiteSpace(name) ||
            year < DateTime.MinValue.Year ||
            year > DateTime.MaxValue.Year)
        {
            return false;
        }

        var normalizedName = NormalizeHolidayName(name, aliases);

        return holidayDefinitions.TryGetValue(normalizedName, out var definition) &&
               TryCreateHoliday(definition, year, out holiday);
    }

    private static IReadOnlyList<Holiday> GetUpcomingHolidays(
        DateTime fromDate,
        int count,
        Func<int, IReadOnlyList<Holiday>> holidayProvider,
        HolidayDateMode dateMode)
    {
        if (count < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be greater than zero.");
        }

        ValidateHolidayDateMode(dateMode);

        var startDate = fromDate.Date;
        var holidayDateSelector = GetHolidayDateSelector(dateMode);
        var yearsToInspect = Math.Min(
            DateTime.MaxValue.Year - startDate.Year + 1,
            count + 1);

        return Enumerable.Range(startDate.Year, yearsToInspect)
            .SelectMany(year => GetUpcomingHolidayCandidates(
                holidayProvider,
                holidayDateSelector,
                startDate,
                year,
                dateMode))
            .OrderBy(holiday => holidayDateSelector(holiday))
            .ThenBy(holiday => holiday.ActualDate)
            .ThenBy(holiday => holiday.Name, StringComparer.Ordinal)
            .Take(count)
            .ToArray();
    }

    private static Func<Holiday, DateTime> GetHolidayDateSelector(HolidayDateMode dateMode)
    {
        return dateMode switch
        {
            HolidayDateMode.ActualDate => static holiday => holiday.ActualDate,
            HolidayDateMode.ObservedDate => static holiday => holiday.ObservedDate,
            _ => throw new ArgumentOutOfRangeException(nameof(dateMode), dateMode, "Unsupported holiday date mode.")
        };
    }

    private static void ValidateHolidayDateMode(HolidayDateMode dateMode)
    {
        if (!Enum.IsDefined(dateMode))
        {
            throw new ArgumentOutOfRangeException(nameof(dateMode), dateMode, "Unsupported holiday date mode.");
        }
    }

    private static bool TryCreateHoliday(HolidayDefinition definition, int year, out Holiday? holiday)
    {
        try
        {
            holiday = definition.Create(year);
            return true;
        }
        catch (ArgumentOutOfRangeException)
        {
            holiday = null;
            return false;
        }
    }

    private static IEnumerable<Holiday> GetUpcomingHolidayCandidates(
        Func<int, IReadOnlyList<Holiday>> holidayProvider,
        Func<Holiday, DateTime> holidayDateSelector,
        DateTime startDate,
        int year,
        HolidayDateMode dateMode)
    {
        var thresholdDate = year == startDate.Year
            ? startDate
            : new DateTime(year, 1, 1);

        var holidaysInYear = holidayProvider(year)
            .Where(holiday => holidayDateSelector(holiday) >= thresholdDate);

        return dateMode == HolidayDateMode.ObservedDate &&
               year == startDate.Year &&
               year < DateTime.MaxValue.Year
            ? holidaysInYear.Concat(GetObservedCarryoverHolidays(holidayProvider, startDate, year + 1))
            : holidaysInYear;
    }

    private static IEnumerable<Holiday> GetObservedCarryoverHolidays(
        Func<int, IReadOnlyList<Holiday>> holidayProvider,
        DateTime startDate,
        int year)
    {
        return holidayProvider(year)
            .Where(holiday =>
                holiday.ObservedDate < holiday.ActualDate &&
                holiday.ObservedDate >= startDate);
    }
}
