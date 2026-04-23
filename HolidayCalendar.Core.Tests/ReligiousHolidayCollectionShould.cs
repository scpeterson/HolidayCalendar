using FluentAssertions;
using HolidayCalendar.Core;
using static HolidayCalendar.Core.HolidayCalculator;

namespace HolidayCalendar.Core.Tests;

public sealed class ReligiousHolidayCollectionShould
{
    [Fact]
    public void ReturnSupportedReligiousHolidayNames()
    {
        var names = GetSupportedReligiousHolidayNames();

        names.Should().Equal(
            HolidayNames.Epiphany,
            HolidayNames.AshWednesday,
            HolidayNames.Annunciation,
            HolidayNames.PalmSunday,
            HolidayNames.MaundyThursday,
            HolidayNames.GoodFriday,
            HolidayNames.HolySaturday,
            HolidayNames.EasterSunday,
            HolidayNames.EasterMonday,
            HolidayNames.AscensionDay,
            HolidayNames.PentecostSunday,
            HolidayNames.PentecostMonday,
            HolidayNames.AllSaintsDay,
            HolidayNames.AllSoulsDay,
            HolidayNames.ChristmasEve,
            HolidayNames.ChristmasDay);
    }

    [Fact]
    public void ReturnSupportedReligiousHolidayAliases()
    {
        var aliases = GetReligiousHolidayAliases();

        aliases.Should().Contain([
            new KeyValuePair<string, string>("Easter", HolidayNames.EasterSunday),
            new KeyValuePair<string, string>("Holy Thursday", HolidayNames.MaundyThursday),
            new KeyValuePair<string, string>("Ascension Thursday", HolidayNames.AscensionDay),
            new KeyValuePair<string, string>("Pentecost", HolidayNames.PentecostSunday),
            new KeyValuePair<string, string>("All Hallows' Day", HolidayNames.AllSaintsDay),
            new KeyValuePair<string, string>("Christmas", HolidayNames.ChristmasDay),
            new KeyValuePair<string, string>("Xmas Eve", HolidayNames.ChristmasEve)
        ]);
    }

    [Fact]
    public void ReturnOrderedReligiousHolidaysForAYear()
    {
        var holidays = GetReligiousHolidays(2025);

        holidays.Should().HaveCount(16);
        holidays.Select(holiday => holiday.Name).Should().Equal(
            HolidayNames.Epiphany,
            HolidayNames.AshWednesday,
            HolidayNames.Annunciation,
            HolidayNames.PalmSunday,
            HolidayNames.MaundyThursday,
            HolidayNames.GoodFriday,
            HolidayNames.HolySaturday,
            HolidayNames.EasterSunday,
            HolidayNames.EasterMonday,
            HolidayNames.AscensionDay,
            HolidayNames.PentecostSunday,
            HolidayNames.PentecostMonday,
            HolidayNames.AllSaintsDay,
            HolidayNames.AllSoulsDay,
            HolidayNames.ChristmasEve,
            HolidayNames.ChristmasDay);
        holidays.Select(holiday => holiday.ActualDate).Should().BeInAscendingOrder();
        holidays.Should().OnlyContain(holiday => holiday.Category == HolidayCategory.Religious);
        holidays.Should().OnlyContain(holiday => !holiday.IsObservedOnDifferentDate);
    }

    [Theory]
    [InlineData(HolidayNames.Epiphany, 2025, 1, 6)]
    [InlineData(HolidayNames.AshWednesday, 2025, 3, 5)]
    [InlineData(HolidayNames.Annunciation, 2025, 3, 25)]
    [InlineData(HolidayNames.PalmSunday, 2025, 4, 13)]
    [InlineData(HolidayNames.MaundyThursday, 2025, 4, 17)]
    [InlineData(HolidayNames.GoodFriday, 2025, 4, 18)]
    [InlineData(HolidayNames.HolySaturday, 2025, 4, 19)]
    [InlineData(HolidayNames.EasterSunday, 2025, 4, 20)]
    [InlineData(HolidayNames.EasterMonday, 2025, 4, 21)]
    [InlineData(HolidayNames.AscensionDay, 2025, 5, 29)]
    [InlineData(HolidayNames.PentecostSunday, 2025, 6, 8)]
    [InlineData(HolidayNames.PentecostMonday, 2025, 6, 9)]
    [InlineData(HolidayNames.AllSaintsDay, 2025, 11, 1)]
    [InlineData(HolidayNames.AllSoulsDay, 2025, 11, 2)]
    [InlineData(HolidayNames.ChristmasEve, 2025, 12, 24)]
    [InlineData(HolidayNames.ChristmasDay, 2025, 12, 25)]
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
    public void TryFindReligiousHolidayByName()
    {
        var result = TryGetReligiousHoliday(HolidayNames.EasterSunday, 2025, out var holiday);

        result.Should().BeTrue();
        holiday.Should().NotBeNull();
        holiday!.Name.Should().Be(HolidayNames.EasterSunday);
        holiday.ActualDate.Should().Be(new DateTime(2025, 4, 20));
    }

    [Fact]
    public void TryFindReligiousHolidayByAlias()
    {
        var result = TryGetReligiousHoliday("Easter", 2025, out var holiday);

        result.Should().BeTrue();
        holiday.Should().NotBeNull();
        holiday!.Name.Should().Be(HolidayNames.EasterSunday);
        holiday.ActualDate.Should().Be(new DateTime(2025, 4, 20));
    }

    [Theory]
    [InlineData("Easter", HolidayNames.EasterSunday, 2025, 4, 20)]
    [InlineData("Holy Thursday", HolidayNames.MaundyThursday, 2025, 4, 17)]
    [InlineData("Ascension Thursday", HolidayNames.AscensionDay, 2025, 5, 29)]
    [InlineData("Pentecost", HolidayNames.PentecostSunday, 2025, 6, 8)]
    [InlineData("All Hallows' Day", HolidayNames.AllSaintsDay, 2025, 11, 1)]
    [InlineData("Christmas", HolidayNames.ChristmasDay, 2025, 12, 25)]
    public void FindReligiousHolidaysBySupportedAliases(string alias, string expectedName, int year, int month, int day)
    {
        var holiday = GetReligiousHoliday(alias, year);

        holiday.Name.Should().Be(expectedName);
        holiday.ActualDate.Should().Be(new DateTime(year, month, day));
    }

    [Fact]
    public void RejectUnknownReligiousHolidayNames()
    {
        var action = () => GetReligiousHoliday("Corpus Christi", 2025);

        action.Should()
            .Throw<ArgumentException>()
            .Which.ParamName.Should()
            .Be("name");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("Corpus Christi")]
    public void ReturnFalseForUnknownOrBlankReligiousHolidayNames(string name)
    {
        var result = TryGetReligiousHoliday(name, 2025, out var holiday);

        result.Should().BeFalse();
        holiday.Should().BeNull();
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

    [Theory]
    [InlineData(HolidayNames.EasterSunday, 1582)]
    [InlineData(HolidayNames.EasterSunday, 0)]
    [InlineData(HolidayNames.EasterSunday, -1)]
    public void ReturnFalseForUnsupportedReligiousHolidayYears(string name, int year)
    {
        var result = TryGetReligiousHoliday(name, year, out var holiday);

        result.Should().BeFalse();
        holiday.Should().BeNull();
    }

    [Fact]
    public void ReturnUpcomingReligiousHolidaysWithinTheSameYear()
    {
        var holidays = GetUpcomingReligiousHolidays(new DateTime(2025, 4, 19), 2);

        holidays.Select(holiday => holiday.Name).Should().Equal(
            HolidayNames.HolySaturday,
            HolidayNames.EasterSunday);
    }

    [Fact]
    public void ReturnUpcomingReligiousHolidaysAcrossYearBoundaries()
    {
        var holidays = GetUpcomingReligiousHolidays(new DateTime(2025, 12, 26), 2);

        holidays.Select(holiday => holiday.Name).Should().Equal(
            HolidayNames.Epiphany,
            HolidayNames.AshWednesday);
        holidays[0].ActualDate.Should().Be(new DateTime(2026, 1, 6));
        holidays[1].ActualDate.Should().Be(new DateTime(2026, 2, 18));
    }

    [Fact]
    public void ReturnUpcomingReligiousHolidaysByObservedDate()
    {
        var holidays = GetUpcomingReligiousHolidays(new DateTime(2025, 12, 24), 2, HolidayDateMode.ObservedDate);

        holidays.Select(holiday => holiday.Name).Should().Equal(
            HolidayNames.ChristmasEve,
            HolidayNames.ChristmasDay);
        holidays[0].ObservedDate.Should().Be(new DateTime(2025, 12, 24));
        holidays[1].ObservedDate.Should().Be(new DateTime(2025, 12, 25));
    }

    [Fact]
    public void IncludeReligiousHolidaysThatOccurOnTheStartingDate()
    {
        var holidays = GetUpcomingReligiousHolidays(new DateTime(2025, 4, 20, 12, 0, 0), 1);

        holidays.Should().ContainSingle();
        holidays[0].Name.Should().Be(HolidayNames.EasterSunday);
        holidays[0].ActualDate.Should().Be(new DateTime(2025, 4, 20));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void RejectInvalidUpcomingReligiousHolidayCount(int count)
    {
        var action = () => GetUpcomingReligiousHolidays(new DateTime(2025, 1, 1), count);

        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .Which.ParamName.Should()
            .Be("count");
    }

    [Fact]
    public void RejectInvalidUpcomingReligiousHolidayDateMode()
    {
        var action = () => GetUpcomingReligiousHolidays(new DateTime(2025, 1, 1), 1, (HolidayDateMode)(-1));

        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .Which.ParamName.Should()
            .Be("dateMode");
    }
}
