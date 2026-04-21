using HolidayCalendar.Core;

var now = DateTime.Now;
var today = now.Date;

Console.WriteLine($"Current local date/time: {now:F}");
Console.WriteLine();
Console.WriteLine($"Upcoming federal holidays by actual date from {today:d}:");
foreach (var holiday in HolidayCalculator.GetUpcomingFederalHolidays(today, 3))
{
    Console.WriteLine($"{holiday.Name}: actual={holiday.ActualDate:d}, observed={holiday.ObservedDate:d}");
}

Console.WriteLine();
Console.WriteLine($"Upcoming federal holidays by observed date from {today:d}:");
foreach (var holiday in HolidayCalculator.GetUpcomingFederalHolidays(today, 3, HolidayDateMode.ObservedDate))
{
    Console.WriteLine($"{holiday.Name}: actual={holiday.ActualDate:d}, observed={holiday.ObservedDate:d}");
}

Console.WriteLine();
Console.WriteLine($"Upcoming religious holidays from {today:d}:");
foreach (var holiday in HolidayCalculator.GetUpcomingReligiousHolidays(today, 4))
{
    Console.WriteLine($"{holiday.Name}: {holiday.ActualDate:d}");
}
