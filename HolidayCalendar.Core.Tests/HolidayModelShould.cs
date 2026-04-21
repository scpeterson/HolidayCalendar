using FluentAssertions;
using HolidayCalendar.Core;

namespace HolidayCalendar.Core.Tests;

public sealed class HolidayModelShould
{
    [Fact]
    public void ExposeHolidayModelWithObservedDateMetadata()
    {
        var holiday = new Holiday(
            "New Year's Day",
            new DateTime(2022, 1, 1),
            new DateTime(2021, 12, 31),
            HolidayCategory.Federal);

        holiday.Name.Should().Be("New Year's Day");
        holiday.ActualDate.Should().Be(new DateTime(2022, 1, 1));
        holiday.ObservedDate.Should().Be(new DateTime(2021, 12, 31));
        holiday.Category.Should().Be(HolidayCategory.Federal);
        holiday.IsObservedOnDifferentDate.Should().BeTrue();
    }
}
