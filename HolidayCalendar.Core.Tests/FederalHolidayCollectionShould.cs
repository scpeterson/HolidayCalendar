using FluentAssertions;
using HolidayCalendar.Core;
using static HolidayCalendar.Core.HolidayCalculator;

namespace HolidayCalendar.Core.Tests;

public sealed class FederalHolidayCollectionShould
{
    [Fact]
    public void ReturnOrderedFederalHolidaysForAModernYear()
    {
        var holidays = GetFederalHolidays(2025);

        holidays.Should().HaveCount(11);
        holidays.Select(holiday => holiday.Name).Should().Equal(
            HolidayNames.NewYearsDay,
            HolidayNames.MartinLutherKingJrDay,
            HolidayNames.PresidentsDay,
            HolidayNames.MemorialDay,
            HolidayNames.Juneteenth,
            HolidayNames.IndependenceDay,
            HolidayNames.LaborDay,
            HolidayNames.ColumbusDay,
            HolidayNames.VeteransDay,
            HolidayNames.Thanksgiving,
            HolidayNames.ChristmasDay);
        holidays.Select(holiday => holiday.ActualDate).Should().BeInAscendingOrder();
        holidays.Should().OnlyContain(holiday => holiday.Category == HolidayCategory.Federal);
    }

    [Fact]
    public void ReturnHistoricalFederalHolidaySetForAYearBeforeJuneteenthAndMlk()
    {
        var holidays = GetFederalHolidays(1969);

        holidays.Select(holiday => holiday.Name).Should().Equal(
            HolidayNames.NewYearsDay,
            HolidayNames.PresidentsDay,
            HolidayNames.MemorialDay,
            HolidayNames.IndependenceDay,
            HolidayNames.LaborDay,
            HolidayNames.ColumbusDay,
            HolidayNames.VeteransDay,
            HolidayNames.Thanksgiving,
            HolidayNames.ChristmasDay);
    }

    [Fact]
    public void ReturnOnlyMemorialDayAtTheEarliestSupportedFederalYear()
    {
        var holidays = GetFederalHolidays(1868);

        holidays.Should().ContainSingle();
        holidays[0].Name.Should().Be(HolidayNames.MemorialDay);
        holidays[0].ActualDate.Should().Be(new DateTime(1868, 5, 30));
        holidays[0].ObservedDate.Should().Be(new DateTime(1868, 5, 30));
    }

    [Fact]
    public void IncludeObservedDatesWhenTheyDifferFromActualDates()
    {
        var holidays = GetFederalHolidays(2022);

        holidays.Should().ContainSingle(holiday =>
            holiday.Name == HolidayNames.NewYearsDay &&
            holiday.ActualDate == new DateTime(2022, 1, 1) &&
            holiday.ObservedDate == new DateTime(2021, 12, 31) &&
            holiday.IsObservedOnDifferentDate);
    }

    [Fact]
    public void ReturnUpcomingFederalHolidaysWithinTheSameYear()
    {
        var holidays = GetUpcomingFederalHolidays(new DateTime(2025, 6, 20), 3);

        holidays.Select(holiday => holiday.Name).Should().Equal(
            HolidayNames.IndependenceDay,
            HolidayNames.LaborDay,
            HolidayNames.ColumbusDay);
    }

    [Fact]
    public void ReturnUpcomingFederalHolidaysAcrossYearBoundaries()
    {
        var holidays = GetUpcomingFederalHolidays(new DateTime(2025, 12, 26), 2);

        holidays.Select(holiday => holiday.Name).Should().Equal(
            HolidayNames.NewYearsDay,
            HolidayNames.MartinLutherKingJrDay);
        holidays[0].ActualDate.Should().Be(new DateTime(2026, 1, 1));
        holidays[1].ActualDate.Should().Be(new DateTime(2026, 1, 19));
    }

    [Fact]
    public void ReturnUpcomingFederalHolidaysByObservedDateAcrossYearBoundaries()
    {
        var holidays = GetUpcomingFederalHolidays(new DateTime(2021, 12, 25), 2, HolidayDateMode.ObservedDate);

        holidays.Select(holiday => holiday.Name).Should().Equal(
            HolidayNames.NewYearsDay,
            HolidayNames.MartinLutherKingJrDay);
        holidays[0].ActualDate.Should().Be(new DateTime(2022, 1, 1));
        holidays[0].ObservedDate.Should().Be(new DateTime(2021, 12, 31));
        holidays[1].ObservedDate.Should().Be(new DateTime(2022, 1, 17));
    }

    [Fact]
    public void IncludeFederalHolidaysObservedOnTheStartingDate()
    {
        var holidays = GetUpcomingFederalHolidays(new DateTime(2021, 12, 31, 18, 0, 0), 1, HolidayDateMode.ObservedDate);

        holidays.Should().ContainSingle();
        holidays[0].Name.Should().Be(HolidayNames.NewYearsDay);
        holidays[0].ActualDate.Should().Be(new DateTime(2022, 1, 1));
        holidays[0].ObservedDate.Should().Be(new DateTime(2021, 12, 31));
    }

    [Fact]
    public void IncludeFederalHolidaysThatOccurOnTheStartingDate()
    {
        var holidays = GetUpcomingFederalHolidays(new DateTime(2025, 7, 4, 18, 0, 0), 1);

        holidays.Should().ContainSingle();
        holidays[0].Name.Should().Be(HolidayNames.IndependenceDay);
        holidays[0].ActualDate.Should().Be(new DateTime(2025, 7, 4));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void RejectInvalidUpcomingFederalHolidayCount(int count)
    {
        var action = () => GetUpcomingFederalHolidays(new DateTime(2025, 1, 1), count);

        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .Which.ParamName.Should()
            .Be("count");
    }

    [Fact]
    public void RejectInvalidUpcomingFederalHolidayDateMode()
    {
        var action = () => GetUpcomingFederalHolidays(new DateTime(2025, 1, 1), 1, (HolidayDateMode)999);

        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .Which.ParamName.Should()
            .Be("dateMode");
    }
}
