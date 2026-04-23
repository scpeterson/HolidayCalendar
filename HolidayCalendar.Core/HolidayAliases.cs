using System.Collections.ObjectModel;

namespace HolidayCalendar.Core;

/// <summary>
/// Provides the supported friendly aliases accepted by holiday lookup APIs.
/// </summary>
public static class HolidayAliases
{
    /// <summary>
    /// Gets the friendly aliases accepted by federal holiday lookup APIs.
    /// </summary>
    public static IReadOnlyDictionary<string, string> Federal { get; } =
        new ReadOnlyDictionary<string, string>(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["New Years Day"] = HolidayNames.NewYearsDay,
                ["MLK Day"] = HolidayNames.MartinLutherKingJrDay,
                ["Washington's Birthday"] = HolidayNames.PresidentsDay,
                ["Fourth of July"] = HolidayNames.IndependenceDay,
                ["Xmas"] = HolidayNames.ChristmasDay,
                ["Christmas"] = HolidayNames.ChristmasDay
            });

    /// <summary>
    /// Gets the friendly aliases accepted by religious holiday lookup APIs.
    /// </summary>
    public static IReadOnlyDictionary<string, string> Religious { get; } =
        new ReadOnlyDictionary<string, string>(
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Easter"] = HolidayNames.EasterSunday,
                ["Holy Thursday"] = HolidayNames.MaundyThursday,
                ["Ascension Thursday"] = HolidayNames.AscensionDay,
                ["Pentecost"] = HolidayNames.PentecostSunday,
                ["All Hallows' Day"] = HolidayNames.AllSaintsDay,
                ["Christmas"] = HolidayNames.ChristmasDay,
                ["Xmas Eve"] = HolidayNames.ChristmasEve
            });
}
