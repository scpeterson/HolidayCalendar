using FluentAssertions;
using static HolidayCalendar.Core.HolidayCalculator;

namespace HolidayCalendar.Core.Tests;

public sealed class EasterHolidayCalculatorShould
{
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
}
