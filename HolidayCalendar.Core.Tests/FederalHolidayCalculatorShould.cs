using FluentAssertions;
using HolidayCalendar.Core;
using static HolidayCalendar.Core.HolidayCalculator;

namespace HolidayCalendar.Core.Tests;

public sealed class FederalHolidayCalculatorShould
{
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
    [InlineData(HolidayNames.NewYearsDay, 1869)]
    [InlineData(HolidayNames.MartinLutherKingJrDay, 1985)]
    [InlineData(HolidayNames.PresidentsDay, 1884)]
    [InlineData(HolidayNames.Juneteenth, 2020)]
    [InlineData(HolidayNames.MemorialDay, 1867)]
    [InlineData(HolidayNames.IndependenceDay, 1869)]
    [InlineData(HolidayNames.LaborDay, 1893)]
    [InlineData(HolidayNames.ColumbusDay, 1933)]
    [InlineData(HolidayNames.VeteransDay, 1937)]
    [InlineData(HolidayNames.Thanksgiving, 1941)]
    [InlineData(HolidayNames.ChristmasDay, 1869)]
    public void RejectYearsBeforeFederalHolidayAvailability(string holidayName, int year)
    {
        var action = () => GetFederalHoliday(holidayName, year);

        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .Which.ParamName.Should()
            .Be("year");
    }

    [Theory]
    [InlineData(HolidayNames.NewYearsDay, 1870, 1, 1)]
    [InlineData(HolidayNames.MartinLutherKingJrDay, 1986, 1, 20)]
    [InlineData(HolidayNames.PresidentsDay, 1885, 2, 22)]
    [InlineData(HolidayNames.Juneteenth, 2021, 6, 19)]
    [InlineData(HolidayNames.MemorialDay, 1868, 5, 30)]
    [InlineData(HolidayNames.IndependenceDay, 1870, 7, 4)]
    [InlineData(HolidayNames.LaborDay, 1894, 9, 3)]
    [InlineData(HolidayNames.ColumbusDay, 1934, 10, 12)]
    [InlineData(HolidayNames.VeteransDay, 1938, 11, 11)]
    [InlineData(HolidayNames.Thanksgiving, 1942, 11, 26)]
    [InlineData(HolidayNames.ChristmasDay, 1870, 12, 25)]
    public void CalculateFederalHolidaysAtTheirFirstSupportedYear(string holidayName, int year, int month, int day)
    {
        var result = GetFederalHoliday(holidayName, year);

        result.ActualDate.Should().Be(new DateTime(year, month, day));
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

    [Fact]
    public void RejectUnknownFederalHolidayNames()
    {
        var action = () => GetFederalHoliday("Boxing Day", 2025);

        action.Should()
            .Throw<ArgumentException>()
            .Which.ParamName.Should()
            .Be("name");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void RejectBlankFederalHolidayNames(string name)
    {
        var action = () => GetFederalHoliday(name, 2025);

        action.Should()
            .Throw<ArgumentException>()
            .Which.ParamName.Should()
            .Be("name");
    }

    [Fact]
    public void FindFederalHolidaysByNameCaseInsensitively()
    {
        var holiday = GetFederalHoliday("christmas day", 2025);

        holiday.Name.Should().Be(HolidayNames.ChristmasDay);
        holiday.ActualDate.Should().Be(new DateTime(2025, 12, 25));
    }

    [Theory]
    [InlineData("MLK Day", HolidayNames.MartinLutherKingJrDay, 2025, 1, 20)]
    [InlineData("Washington's Birthday", HolidayNames.PresidentsDay, 2025, 2, 17)]
    [InlineData("Fourth of July", HolidayNames.IndependenceDay, 2025, 7, 4)]
    [InlineData("Xmas", HolidayNames.ChristmasDay, 2025, 12, 25)]
    public void FindFederalHolidaysBySupportedAliases(string alias, string expectedName, int year, int month, int day)
    {
        var holiday = GetFederalHoliday(alias, year);

        holiday.Name.Should().Be(expectedName);
        holiday.ActualDate.Should().Be(new DateTime(year, month, day));
    }
}
