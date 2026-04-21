using HolidayCalendar.Core;

var actualDateExample = new DateTime(2025, 12, 23);
var observedDateExample = new DateTime(2021, 12, 25);

Console.WriteLine("Upcoming federal holidays by actual date:");
foreach (var holiday in HolidayCalculator.GetUpcomingFederalHolidays(actualDateExample, 3))
{
    Console.WriteLine($"{holiday.Name}: actual={holiday.ActualDate:d}, observed={holiday.ObservedDate:d}");
}

Console.WriteLine();
Console.WriteLine("Upcoming federal holidays by observed date:");
foreach (var holiday in HolidayCalculator.GetUpcomingFederalHolidays(observedDateExample, 3, HolidayDateMode.ObservedDate))
{
    Console.WriteLine($"{holiday.Name}: actual={holiday.ActualDate:d}, observed={holiday.ObservedDate:d}");
}

Console.WriteLine();
Console.WriteLine("Upcoming religious holidays:");
foreach (var holiday in HolidayCalculator.GetUpcomingReligiousHolidays(new DateTime(2025, 4, 18), 4))
{
    Console.WriteLine($"{holiday.Name}: {holiday.ActualDate:d}");
}
