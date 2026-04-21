# HolidayCalendar

`HolidayCalendar` is a .NET library for calculating US federal holidays and supported religious holidays.

## Features

- Fixed-date holiday calculations
- Nth weekday and last weekday date rules
- Federal holiday support with historical transitions
- Observed holiday date handling
- Single-holiday lookup by name
- Full federal holiday list for a year
- Religious holiday list and lookup support
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

### Get supported religious holidays for a year

```csharp
using HolidayCalendar.Core;

var holidays = HolidayCalculator.GetReligiousHolidays(2025);

foreach (var holiday in holidays)
{
    Console.WriteLine($"{holiday.Name}: {holiday.ActualDate:d}");
}
```

### Get a single religious holiday by name

```csharp
using HolidayCalendar.Core;

var easter = HolidayCalculator.GetReligiousHoliday("Easter Sunday", 2025);

Console.WriteLine(easter.ActualDate); // 4/20/2025
Console.WriteLine(easter.Category);   // Religious
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
