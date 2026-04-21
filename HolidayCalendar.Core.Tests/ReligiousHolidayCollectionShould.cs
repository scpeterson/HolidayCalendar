using FluentAssertions;
using HolidayCalendar.Core;
using static HolidayCalendar.Core.HolidayCalculator;

namespace HolidayCalendar.Core.Tests;

public sealed class ReligiousHolidayCollectionShould
{
    [Fact]
    public void ReturnOrderedReligiousHolidaysForAYear()
    {
        var holidays = GetReligiousHolidays(2025);

        holidays.Should().HaveCount(3);
        holidays.Select(holiday => holiday.Name).Should().Equal(
            HolidayNames.GoodFriday,
            HolidayNames.EasterSunday,
            HolidayNames.PentecostSunday);
        holidays.Select(holiday => holiday.ActualDate).Should().BeInAscendingOrder();
        holidays.Should().OnlyContain(holiday => holiday.Category == HolidayCategory.Religious);
        holidays.Should().OnlyContain(holiday => !holiday.IsObservedOnDifferentDate);
    }

    [Theory]
    [InlineData(HolidayNames.GoodFriday, 2025, 4, 18)]
    [InlineData(HolidayNames.EasterSunday, 2025, 4, 20)]
    [InlineData(HolidayNames.PentecostSunday, 2025, 6, 8)]
    public void FindReligiousHolidaysByName(string name, int year, int month, int day)
    {
        var holiday = GetReligiousHoliday(name, year);

        holiday.Name.Should().Be(name);
        holiday.ActualDate.Should().Be(new DateTime(year, month, day));
        holiday.ObservedDate.Should().Be(new DateTime(year, month, day));
        holiday.Category.Should().Be(HolidayCategory.Religious);
    }

    [Fact]
    public void FindReligiousHolidaysByNameCaseInsensitively()
    {
        var holiday = GetReligiousHoliday("easter sunday", 2025);

        holiday.Name.Should().Be(HolidayNames.EasterSunday);
        holiday.ActualDate.Should().Be(new DateTime(2025, 4, 20));
    }

    [Fact]
    public void RejectUnknownReligiousHolidayNames()
    {
        var action = () => GetReligiousHoliday("Ascension Day", 2025);

        action.Should()
            .Throw<ArgumentException>()
            .Which.ParamName.Should()
            .Be("name");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void RejectBlankReligiousHolidayNames(string name)
    {
        var action = () => GetReligiousHoliday(name, 2025);

        action.Should()
            .Throw<ArgumentException>()
            .Which.ParamName.Should()
            .Be("name");
    }

    [Theory]
    [InlineData(1582)]
    [InlineData(0)]
    [InlineData(-1)]
    public void RejectYearsBeforeSupportedGregorianRange(int year)
    {
        var action = () => GetReligiousHolidays(year);

        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .Which.ParamName.Should()
            .Be("year");
    }
}
