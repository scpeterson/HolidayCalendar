using FluentAssertions;
using HolidayCalendar.Core;
using static HolidayCalendar.Core.HolidayCalculator;

namespace HolidayCalendar.Core.Tests;

public sealed class FederalHolidayCollectionShould
{
    [Fact]
    public void ReturnSupportedFederalHolidayNames()
    {
        var names = GetSupportedFederalHolidayNames();

        names.Should().Equal(
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
    }

    [Fact]
    public void ReturnSupportedFederalHolidayAliases()
    {
        var aliases = GetFederalHolidayAliases();

        aliases.Should().Contain([
            new KeyValuePair<string, string>("New Years Day", HolidayNames.NewYearsDay),
            new KeyValuePair<string, string>("MLK Day", HolidayNames.MartinLutherKingJrDay),
            new KeyValuePair<string, string>("Washington's Birthday", HolidayNames.PresidentsDay),
            new KeyValuePair<string, string>("Fourth of July", HolidayNames.IndependenceDay),
            new KeyValuePair<string, string>("Xmas", HolidayNames.ChristmasDay),
            new KeyValuePair<string, string>("Christmas", HolidayNames.ChristmasDay)
        ]);
    }

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
    public void TryFindFederalHolidayByName()
    {
        var result = TryGetFederalHoliday(HolidayNames.Thanksgiving, 2025, out var holiday);

        result.Should().BeTrue();
        holiday.Should().NotBeNull();
        holiday!.Name.Should().Be(HolidayNames.Thanksgiving);
        holiday.ActualDate.Should().Be(new DateTime(2025, 11, 27));
    }

    [Fact]
    public void TryFindFederalHolidayByAlias()
    {
        var result = TryGetFederalHoliday("MLK Day", 2025, out var holiday);

        result.Should().BeTrue();
        holiday.Should().NotBeNull();
        holiday!.Name.Should().Be(HolidayNames.MartinLutherKingJrDay);
        holiday.ActualDate.Should().Be(new DateTime(2025, 1, 20));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("Boxing Day")]
    public void ReturnFalseForUnknownOrBlankFederalHolidayNames(string name)
    {
        var result = TryGetFederalHoliday(name, 2025, out var holiday);

        result.Should().BeFalse();
        holiday.Should().BeNull();
    }

    [Theory]
    [InlineData(HolidayNames.Juneteenth, 2020)]
    [InlineData(HolidayNames.NewYearsDay, 0)]
    [InlineData(HolidayNames.NewYearsDay, 10000)]
    public void ReturnFalseForUnsupportedFederalHolidayYears(string name, int year)
    {
        var result = TryGetFederalHoliday(name, year, out var holiday);

        result.Should().BeFalse();
        holiday.Should().BeNull();
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
