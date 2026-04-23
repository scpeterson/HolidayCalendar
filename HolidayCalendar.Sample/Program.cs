using HolidayCalendar.Core;

var now = DateTime.Now;
var today = now.Date;

Console.WriteLine($"Current local date/time: {now:F}");
Console.WriteLine();

Console.WriteLine("Supported federal holiday names:");
Console.WriteLine(string.Join(", ", HolidayCalculator.GetSupportedFederalHolidayNames()));
Console.WriteLine();

Console.WriteLine("Federal holiday aliases:");
foreach (var (alias, canonicalName) in HolidayAliases.Federal)
{
    Console.WriteLine($"{alias} => {canonicalName}");
}

Console.WriteLine();
if (HolidayCalculator.TryGetFederalHoliday("MLK Day", today.Year, out var mlkHoliday) &&
    mlkHoliday is not null)
{
    Console.WriteLine($"Alias lookup for MLK Day in {today.Year}: {mlkHoliday.Name} on {mlkHoliday.ActualDate:d}");
}
else
{
    Console.WriteLine($"MLK Day is not available in {today.Year}.");
}

if (!HolidayCalculator.TryGetReligiousHoliday("Corpus Christi", today.Year, out _))
{
    Console.WriteLine($"Corpus Christi is not a supported religious holiday in {today.Year}.");
}

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
