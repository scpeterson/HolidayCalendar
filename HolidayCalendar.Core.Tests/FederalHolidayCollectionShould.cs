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
}
