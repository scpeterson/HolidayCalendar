# HolidayCalendar

`HolidayCalendar` is a .NET library for calculating US federal holidays and Easter-related dates.

## Features

- Fixed-date holiday calculations
- Nth weekday and last weekday date rules
- Federal holiday support with historical transitions
- Observed holiday date handling
- Single-holiday lookup by name
- Full federal holiday list for a year
- Easter, Good Friday, and Pentecost calculations

## Target Framework

- `.NET 10`

## Usage

### Get all federal holidays for a year

```csharp
using HolidayCalendar.Core;

var holidays = HolidayCalculator.GetFederalHolidays(2025);

foreach (var holiday in holidays)
{
    Console.WriteLine($"{holiday.Name}: actual={holiday.ActualDate:d}, observed={holiday.ObservedDate:d}");
}
```

### Get a single federal holiday by name

```csharp
using HolidayCalendar.Core;

var christmas = HolidayCalculator.GetFederalHoliday("Christmas Day", 2025);

Console.WriteLine(christmas.ActualDate);   // 12/25/2025
Console.WriteLine(christmas.ObservedDate); // 12/25/2025
```

### Work with observed dates

```csharp
using HolidayCalendar.Core;

var newYears = HolidayCalculator.GetFederalHoliday("New Year's Day", 2022);

Console.WriteLine(newYears.ActualDate);              // 1/1/2022
Console.WriteLine(newYears.ObservedDate);            // 12/31/2021
Console.WriteLine(newYears.IsObservedOnDifferentDate); // true
```

## Holiday Model

Each returned holiday contains:

- `Name`
- `ActualDate`
- `ObservedDate`
- `Category`
- `IsObservedOnDifferentDate`

## Development

Run the test suite locally with:

```bash
dotnet test HolidayCalendar.sln
```

GitHub Actions also runs restore, build, and tests on pushes and pull requests to `main`.
