using FluentAssertions;
using HolidayCalendar.Core;
using static HolidayCalendar.Core.HolidayCalculator;

namespace HolidayCalendar.Core.Tests;

public sealed class HolidayCalendarShould
{
    // Small, high-value edge-case set
    [Theory]
    [InlineData(1818, 3, 22)] // earliest possible Gregorian Easter
    [InlineData(1943, 4, 25)] // latest possible Gregorian Easter
    [InlineData(1900, 4, 15)] // century year: divisible by 100, not by 400 (NOT leap)
    [InlineData(2000, 4, 23)] // century year: divisible by 400 (IS leap)
    [InlineData(1954, 4, 18)] // algorithm exception case
    [InlineData(1981, 4, 19)] // algorithm exception case
    [InlineData(1804, 4, 1)]  // historical check
    public void CalculateExpectedEasterSunday(int year, int month, int day)
    {
        var result = CalculateEasterSunday(year);

        result.Should().Be(new DateTime(year, month, day));
    }

    [Fact]
    public void AcceptTheFirstSupportedGregorianYear()
    {
        var result = CalculateEasterSunday(1583);

        result.Should().Be(new DateTime(1583, 4, 10));
    }

    [Theory]
    [InlineData(1582)]
    [InlineData(0)]
    [InlineData(-1)]
    public void RejectYearsBeforeGregorianCalendarStart(int year)
    {
        var action = () => CalculateEasterSunday(year);

        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .Which.ParamName.Should()
            .Be("year");
    }

    [Theory]
    [InlineData(1582)]
    [InlineData(0)]
    [InlineData(-1)]
    public void RejectYearsBeforeGregorianCalendarStartForGoodFriday(int year)
    {
        var action = () => CalculateGoodFriday(year);

        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .Which.ParamName.Should()
            .Be("year");
    }

    [Theory]
    [InlineData(1582)]
    [InlineData(0)]
    [InlineData(-1)]
    public void RejectYearsBeforeGregorianCalendarStartForPentecostSunday(int year)
    {
        var action = () => CalculatePentecostSunday(year);

        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .Which.ParamName.Should()
            .Be("year");
    }

    [Fact]
    public void ExposeDateConstantsThatMatchDateTimeConventions()
    {
        Constants.Wednesday.Should().Be((int)DayOfWeek.Wednesday);
        Constants.Thursday.Should().Be((int)DayOfWeek.Thursday);
        Constants.January.Should().Be(1);
        Constants.December.Should().Be(12);
    }

    [Theory]
    [InlineData(2025, 1, 1)]
    [InlineData(2026, 12, 25)]
    [InlineData(2024, 2, 29)]
    public void CalculateExpectedFixedHoliday(int year, int month, int day)
    {
        var result = CalculateFixedHoliday(year, month, day);

        result.Should().Be(new DateTime(year, month, day));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10000)]
    public void RejectYearsOutsideDateTimeRangeForFixedHolidayCalculations(int year)
    {
        var action = () => CalculateFixedHoliday(year, Constants.January, 1);

        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .Which.ParamName.Should()
            .Be("year");
    }

    [Theory]
    [InlineData(2025, 1, (int)DayOfWeek.Monday, 3, 20)]
    [InlineData(2026, 9, (int)DayOfWeek.Monday, 1, 7)]
    [InlineData(2025, 11, (int)DayOfWeek.Thursday, 4, 27)]
    public void CalculateExpectedNthWeekdayOfMonth(int year, int month, int dayOfWeek, int occurrence, int day)
    {
        var result = CalculateNthWeekdayOfMonth(year, month, (DayOfWeek)dayOfWeek, occurrence);

        result.Should().Be(new DateTime(year, month, day));
    }

    [Theory]
    [InlineData(2025, 5, (int)DayOfWeek.Monday, 26)]
    [InlineData(2025, 2, (int)DayOfWeek.Friday, 28)]
    [InlineData(2026, 10, (int)DayOfWeek.Monday, 26)]
    public void CalculateExpectedLastWeekdayOfMonth(int year, int month, int dayOfWeek, int day)
    {
        var result = CalculateLastWeekdayOfMonth(year, month, (DayOfWeek)dayOfWeek);

        result.Should().Be(new DateTime(year, month, day));
    }

    [Theory]
    [InlineData(2026, 7, 4, 2026, 7, 3)]
    [InlineData(2027, 7, 4, 2027, 7, 5)]
    [InlineData(2025, 6, 19, 2025, 6, 19)]
    public void CalculateExpectedObservedHoliday(int year, int month, int day, int expectedYear, int expectedMonth, int expectedDay)
    {
        var result = CalculateObservedHoliday(new DateTime(year, month, day));

        result.Should().Be(new DateTime(expectedYear, expectedMonth, expectedDay));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public void RejectInvalidNthWeekdayOccurrence(int occurrence)
    {
        var action = () => CalculateNthWeekdayOfMonth(2025, Constants.January, DayOfWeek.Monday, occurrence);

        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .Which.ParamName.Should()
            .Be("occurrence");
    }

    [Fact]
    public void RejectANonexistentNthWeekdayOccurrence()
    {
        var action = () => CalculateNthWeekdayOfMonth(2025, Constants.February, DayOfWeek.Monday, 5);

        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .Which.ParamName.Should()
            .Be("occurrence");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10000)]
    public void RejectYearsOutsideDateTimeRangeForNthWeekdayCalculations(int year)
    {
        var action = () => CalculateNthWeekdayOfMonth(year, Constants.January, DayOfWeek.Monday, 1);

        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .Which.ParamName.Should()
            .Be("year");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10000)]
    public void RejectYearsOutsideDateTimeRangeForLastWeekdayCalculations(int year)
    {
        var action = () => CalculateLastWeekdayOfMonth(year, Constants.January, DayOfWeek.Monday);

        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .Which.ParamName.Should()
            .Be("year");
    }

    [Theory]
    [InlineData(2025, 1, 20)]
    [InlineData(2026, 1, 19)]
    public void CalculateExpectedMartinLutherKingJrDay(int year, int month, int day)
    {
        var result = CalculateMartinLutherKingJrDay(year);

        result.Should().Be(new DateTime(year, month, day));
    }

    [Theory]
    [InlineData(2025, 1, 1)]
    [InlineData(2026, 1, 1)]
    public void CalculateExpectedNewYearsDay(int year, int month, int day)
    {
        var result = CalculateNewYearsDay(year);

        result.Should().Be(new DateTime(year, month, day));
    }

    [Theory]
    [InlineData(2022, 2021, 12, 31)]
    [InlineData(2023, 2023, 1, 2)]
    [InlineData(2025, 2025, 1, 1)]
    public void CalculateExpectedObservedNewYearsDay(int year, int expectedYear, int expectedMonth, int expectedDay)
    {
        var result = CalculateObservedNewYearsDay(year);

        result.Should().Be(new DateTime(expectedYear, expectedMonth, expectedDay));
    }

    [Theory]
    [InlineData(2025, 2, 17)]
    [InlineData(2026, 2, 16)]
    public void CalculateExpectedPresidentsDay(int year, int month, int day)
    {
        var result = CalculatePresidentsDay(year);

        result.Should().Be(new DateTime(year, month, day));
    }

    [Theory]
    [InlineData(2025, 6, 19)]
    [InlineData(2026, 6, 19)]
    public void CalculateExpectedJuneteenth(int year, int month, int day)
    {
        var result = CalculateJuneteenth(year);

        result.Should().Be(new DateTime(year, month, day));
    }

    [Theory]
    [InlineData(2026, 2026, 6, 19)]
    [InlineData(2027, 2027, 6, 18)]
    [InlineData(2021, 2021, 6, 18)]
    public void CalculateExpectedObservedJuneteenth(int year, int expectedYear, int expectedMonth, int expectedDay)
    {
        var result = CalculateObservedJuneteenth(year);

        result.Should().Be(new DateTime(expectedYear, expectedMonth, expectedDay));
    }

    [Theory]
    [InlineData(2025, 5, 26)]
    [InlineData(2026, 5, 25)]
    public void CalculateExpectedMemorialDay(int year, int month, int day)
    {
        var result = CalculateMemorialDay(year);

        result.Should().Be(new DateTime(year, month, day));
    }

    [Theory]
    [InlineData(2025, 7, 4)]
    [InlineData(2026, 7, 4)]
    public void CalculateExpectedIndependenceDay(int year, int month, int day)
    {
        var result = CalculateIndependenceDay(year);

        result.Should().Be(new DateTime(year, month, day));
    }

    [Theory]
    [InlineData(2026, 2026, 7, 3)]
    [InlineData(2027, 2027, 7, 5)]
    [InlineData(2025, 2025, 7, 4)]
    public void CalculateExpectedObservedIndependenceDay(int year, int expectedYear, int expectedMonth, int expectedDay)
    {
        var result = CalculateObservedIndependenceDay(year);

        result.Should().Be(new DateTime(expectedYear, expectedMonth, expectedDay));
    }

    [Theory]
    [InlineData(2025, 9, 1)]
    [InlineData(2026, 9, 7)]
    public void CalculateExpectedLaborDay(int year, int month, int day)
    {
        var result = CalculateLaborDay(year);

        result.Should().Be(new DateTime(year, month, day));
    }

    [Theory]
    [InlineData(2025, 10, 13)]
    [InlineData(2026, 10, 12)]
    public void CalculateExpectedColumbusDay(int year, int month, int day)
    {
        var result = CalculateColumbusDay(year);

        result.Should().Be(new DateTime(year, month, day));
    }

    [Theory]
    [InlineData(2025, 11, 11)]
    [InlineData(2026, 11, 11)]
    public void CalculateExpectedVeteransDay(int year, int month, int day)
    {
        var result = CalculateVeteransDay(year);

        result.Should().Be(new DateTime(year, month, day));
    }

    [Theory]
    [InlineData(2027, 2027, 11, 11)]
    [InlineData(2026, 2026, 11, 11)]
    [InlineData(2021, 2021, 11, 11)]
    public void CalculateExpectedObservedVeteransDay(int year, int expectedYear, int expectedMonth, int expectedDay)
    {
        var result = CalculateObservedVeteransDay(year);

        result.Should().Be(new DateTime(expectedYear, expectedMonth, expectedDay));
    }

    [Theory]
    [InlineData(2025, 11, 27)]
    [InlineData(2026, 11, 26)]
    public void CalculateExpectedThanksgiving(int year, int month, int day)
    {
        var result = CalculateThanksgiving(year);

        result.Should().Be(new DateTime(year, month, day));
    }

    [Theory]
    [InlineData(2025, 12, 25)]
    [InlineData(2026, 12, 25)]
    public void CalculateExpectedChristmasDay(int year, int month, int day)
    {
        var result = CalculateChristmasDay(year);

        result.Should().Be(new DateTime(year, month, day));
    }

    [Theory]
    [InlineData(2026, 2026, 12, 25)]
    [InlineData(2027, 2027, 12, 24)]
    [InlineData(2021, 2021, 12, 24)]
    public void CalculateExpectedObservedChristmasDay(int year, int expectedYear, int expectedMonth, int expectedDay)
    {
        var result = CalculateObservedChristmasDay(year);

        result.Should().Be(new DateTime(expectedYear, expectedMonth, expectedDay));
    }

    [Theory]
    [InlineData("NewYearsDay", 1869)]
    [InlineData("MartinLutherKingJrDay", 1985)]
    [InlineData("PresidentsDay", 1884)]
    [InlineData("Juneteenth", 2020)]
    [InlineData("MemorialDay", 1867)]
    [InlineData("IndependenceDay", 1869)]
    [InlineData("LaborDay", 1893)]
    [InlineData("ColumbusDay", 1933)]
    [InlineData("VeteransDay", 1937)]
    [InlineData("Thanksgiving", 1941)]
    [InlineData("ChristmasDay", 1869)]
    public void RejectYearsBeforeFederalHolidayAvailability(string holidayName, int year)
    {
        var action = () => CalculateFederalHoliday(holidayName, year);

        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .Which.ParamName.Should()
            .Be("year");
    }

    [Theory]
    [InlineData("ObservedNewYearsDay", 1869)]
    [InlineData("ObservedJuneteenth", 2020)]
    [InlineData("ObservedIndependenceDay", 1869)]
    [InlineData("ObservedVeteransDay", 1937)]
    [InlineData("ObservedChristmasDay", 1869)]
    public void RejectYearsBeforeFederalHolidayAvailabilityForObservedHolidays(string holidayName, int year)
    {
        var action = () => CalculateFederalHoliday(holidayName, year);

        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .Which.ParamName.Should()
            .Be("year");
    }

    [Theory]
    [InlineData("NewYearsDay", 1870, 1, 1)]
    [InlineData("MartinLutherKingJrDay", 1986, 1, 20)]
    [InlineData("PresidentsDay", 1885, 2, 22)]
    [InlineData("Juneteenth", 2021, 6, 19)]
    [InlineData("MemorialDay", 1868, 5, 30)]
    [InlineData("IndependenceDay", 1870, 7, 4)]
    [InlineData("LaborDay", 1894, 9, 3)]
    [InlineData("ColumbusDay", 1934, 10, 12)]
    [InlineData("VeteransDay", 1938, 11, 11)]
    [InlineData("Thanksgiving", 1942, 11, 26)]
    [InlineData("ChristmasDay", 1870, 12, 25)]
    public void CalculateFederalHolidaysAtTheirFirstSupportedYear(string holidayName, int year, int month, int day)
    {
        var result = CalculateFederalHoliday(holidayName, year);

        result.Should().Be(new DateTime(year, month, day));
    }

    [Theory]
    [InlineData(1971, 10, 25)]
    [InlineData(1977, 10, 24)]
    public void CalculateExpectedVeteransDayDuringMondayObservancePeriod(int year, int month, int day)
    {
        var result = CalculateVeteransDay(year);

        result.Should().Be(new DateTime(year, month, day));
    }

    [Theory]
    [InlineData(1971, 10, 25)]
    [InlineData(1977, 10, 24)]
    public void CalculateExpectedObservedVeteransDayDuringMondayObservancePeriod(int year, int month, int day)
    {
        var result = CalculateObservedVeteransDay(year);

        result.Should().Be(new DateTime(year, month, day));
    }

    [Theory]
    [InlineData(1970, 2, 22)]
    [InlineData(1971, 2, 15)]
    public void CalculateExpectedPresidentsDayAcrossHistoricalTransition(int year, int month, int day)
    {
        var result = CalculatePresidentsDay(year);

        result.Should().Be(new DateTime(year, month, day));
    }

    [Theory]
    [InlineData(1970, 5, 30)]
    [InlineData(1971, 5, 31)]
    public void CalculateExpectedMemorialDayAcrossHistoricalTransition(int year, int month, int day)
    {
        var result = CalculateMemorialDay(year);

        result.Should().Be(new DateTime(year, month, day));
    }

    [Theory]
    [InlineData(1970, 10, 12)]
    [InlineData(1971, 10, 11)]
    public void CalculateExpectedColumbusDayAcrossHistoricalTransition(int year, int month, int day)
    {
        var result = CalculateColumbusDay(year);

        result.Should().Be(new DateTime(year, month, day));
    }

    [Theory]
    [InlineData(1818, 3, 20)] // earliest possible Gregorian Good Friday
    [InlineData(1943, 4, 23)] // latest-range Easter minus two days
    [InlineData(2000, 4, 21)] // leap-century regression check
    public void CalculateExpectedGoodFriday(int year, int month, int day)
    {
        var result = CalculateGoodFriday(year);

        result.Should().Be(new DateTime(year, month, day));
    }

    [Theory]
    [InlineData(1818, 5, 10)] // earliest possible Gregorian Easter plus 49 days
    [InlineData(1943, 6, 13)] // latest-range Easter plus 49 days
    [InlineData(2000, 6, 11)] // leap-century regression check
    public void CalculateExpectedPentecostSunday(int year, int month, int day)
    {
        var result = CalculatePentecostSunday(year);

        result.Should().Be(new DateTime(year, month, day));
    }

    // Property-style sanity checks (fast and catches lots of bugs)
    [Theory]
    [InlineData(1583, 4099)] // Gregorian Easter algorithm is commonly used for this range
    public void CalculateEasterOnASundayWithinADateRange(int startYear, int endYear)
    {
        for (var year = startYear; year <= endYear; year++)
        {
            var easter = CalculateEasterSunday(year);

            easter.DayOfWeek.Should().Be(DayOfWeek.Sunday, $"Easter must be Sunday for {year}");

            var earliest = new DateTime(year, 3, 22);
            var latest = new DateTime(year, 4, 25);

            easter.Should()
                .BeOnOrAfter(earliest).And
                .BeOnOrBefore(latest, $"Easter must be within the valid range for {year}");
        }
    }

    [Theory]
    [InlineData(1583, 4099)] // Gregorian Easter algorithm is commonly used for this range
    public void CalculateGoodFridayWithinADateRange(int startYear, int endYear)
    {
        for (var year = startYear; year <= endYear; year++)
        {
            var easter = CalculateEasterSunday(year);
            var goodFriday = CalculateGoodFriday(year);

            goodFriday.DayOfWeek.Should().Be(DayOfWeek.Friday, $"Good Friday must be Friday for {year}");
            goodFriday.Should().Be(easter.AddDays(-2));
        }
    }

    [Theory]
    [InlineData(1583, 4099)] // Gregorian Easter algorithm is commonly used for this range
    public void CalculatePentecostSundayWithinADateRange(int startYear, int endYear)
    {
        for (var year = startYear; year <= endYear; year++)
        {
            var easter = CalculateEasterSunday(year);
            var pentecostSunday = CalculatePentecostSunday(year);

            pentecostSunday.DayOfWeek.Should().Be(DayOfWeek.Sunday, $"Pentecost Sunday must be Sunday for {year}");
            pentecostSunday.Should().Be(easter.AddDays(49));
        }
    }

    private static DateTime CalculateFederalHoliday(string holidayName, int year)
    {
        return holidayName switch
        {
            "NewYearsDay" => CalculateNewYearsDay(year),
            "ObservedNewYearsDay" => CalculateObservedNewYearsDay(year),
            "MartinLutherKingJrDay" => CalculateMartinLutherKingJrDay(year),
            "PresidentsDay" => CalculatePresidentsDay(year),
            "Juneteenth" => CalculateJuneteenth(year),
            "ObservedJuneteenth" => CalculateObservedJuneteenth(year),
            "MemorialDay" => CalculateMemorialDay(year),
            "IndependenceDay" => CalculateIndependenceDay(year),
            "ObservedIndependenceDay" => CalculateObservedIndependenceDay(year),
            "LaborDay" => CalculateLaborDay(year),
            "ColumbusDay" => CalculateColumbusDay(year),
            "VeteransDay" => CalculateVeteransDay(year),
            "ObservedVeteransDay" => CalculateObservedVeteransDay(year),
            "Thanksgiving" => CalculateThanksgiving(year),
            "ChristmasDay" => CalculateChristmasDay(year),
            "ObservedChristmasDay" => CalculateObservedChristmasDay(year),
            _ => throw new ArgumentOutOfRangeException(nameof(holidayName), holidayName, "Unsupported holiday test case.")
        };
    }
}
