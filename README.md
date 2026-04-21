# HolidayCalendar

`HolidayCalendar` is a .NET library for calculating US federal holidays and supported Christian religious holidays.

## Features

- Fixed-date holiday calculations
- Nth weekday and last weekday date rules
- Federal holiday support with historical transitions
- Observed holiday date handling
- Single-holiday lookup by name
- Full federal holiday list for a year
- Religious holiday list and lookup support
- Easter-derived religious holiday calculations

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

Supported religious holidays currently include:

- Epiphany
- Ash Wednesday
- Annunciation
- Palm Sunday
- Maundy Thursday
- Good Friday
- Holy Saturday
- Easter Sunday
- Easter Monday
- Ascension Day
- Pentecost Sunday
- Pentecost Monday
- All Saints' Day
- All Souls' Day
- Christmas Eve
- Christmas Day

### Get a single religious holiday by name

```csharp
using HolidayCalendar.Core;

var easter = HolidayCalculator.GetReligiousHoliday("Easter Sunday", 2025);

Console.WriteLine(easter.ActualDate); // 4/20/2025
Console.WriteLine(easter.Category);   // Religious
```

Both lookup APIs also support a small set of friendly aliases, for example:

- `MLK Day`
- `Washington's Birthday`
- `Fourth of July`
- `Xmas`
- `Easter`
- `Holy Thursday`
- `Ascension Thursday`
- `Pentecost`

### Get upcoming federal holidays

```csharp
using HolidayCalendar.Core;

var upcoming = HolidayCalculator.GetUpcomingFederalHolidays(new DateTime(2025, 6, 20), 3);

foreach (var holiday in upcoming)
{
    Console.WriteLine($"{holiday.Name}: {holiday.ActualDate:d}");
}
```

### Get upcoming religious holidays

```csharp
using HolidayCalendar.Core;

var upcoming = HolidayCalculator.GetUpcomingReligiousHolidays(new DateTime(2025, 4, 19), 2);

foreach (var holiday in upcoming)
{
    Console.WriteLine($"{holiday.Name}: {holiday.ActualDate:d}");
}
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

## Releases

- CI validates restore, build, test, and package creation on `main`
- The release workflow builds a versioned package for tags like `v0.1.0`
- Release notes are tracked in [CHANGELOG.md](CHANGELOG.md)
