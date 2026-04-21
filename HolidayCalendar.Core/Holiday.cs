namespace HolidayCalendar.Core;

/// <summary>
/// Represents a holiday with its actual calendar date and any observed date used for business or federal closures.
/// </summary>
/// <param name="Name">The display name of the holiday.</param>
/// <param name="ActualDate">The calendar date on which the holiday falls.</param>
/// <param name="ObservedDate">The date on which the holiday is observed when different from the actual date.</param>
/// <param name="Category">The holiday grouping used by the library.</param>
public sealed record Holiday(string Name, DateTime ActualDate, DateTime ObservedDate, HolidayCategory Category)
{
    /// <summary>
    /// Gets a value indicating whether the observed date differs from the actual date.
    /// </summary>
    public bool IsObservedOnDifferentDate => ActualDate != ObservedDate;
}
