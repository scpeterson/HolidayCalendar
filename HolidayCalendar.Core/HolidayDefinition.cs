namespace HolidayCalendar.Core;

internal readonly record struct HolidayDefinition(
    string Name,
    int FirstSupportedYear,
    Func<int, DateTime> ActualDate,
    Func<int, DateTime> ObservedDate,
    HolidayCategory Category)
{
    public bool IsSupportedInYear(int year) => year >= FirstSupportedYear;

    public Holiday Create(int year) => new(
        Name,
        ActualDate(year),
        ObservedDate(year),
        Category);
}
