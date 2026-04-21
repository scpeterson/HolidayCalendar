using FluentAssertions;
using HolidayCalendar.Core;
using static HolidayCalendar.Core.HolidayCalculator;

namespace HolidayCalendar.Core.Tests;

public sealed class HolidayRuleCalculatorShould
{
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
        var action = () => CalculateFixedHoliday(year, 1, 1);

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
        var action = () => CalculateNthWeekdayOfMonth(2025, 1, DayOfWeek.Monday, occurrence);

        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .Which.ParamName.Should()
            .Be("occurrence");
    }

    [Fact]
    public void RejectANonexistentNthWeekdayOccurrence()
    {
        var action = () => CalculateNthWeekdayOfMonth(2025, 2, DayOfWeek.Monday, 5);

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
        var action = () => CalculateNthWeekdayOfMonth(year, 1, DayOfWeek.Monday, 1);

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
        var action = () => CalculateLastWeekdayOfMonth(year, 1, DayOfWeek.Monday);

        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .Which.ParamName.Should()
            .Be("year");
    }
}
