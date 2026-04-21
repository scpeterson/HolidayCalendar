namespace HolidayCalendar.Core;

internal static class Constants
{
    public const int Sunday = 0;
    public const int Monday = 1;
    public const int Tuesday = 2;
    public const int Wednesday = 3;
    public const int Thursday = 4;
    public const int Friday = 5;
    public const int Saturday = 6;
    
    public const int January = 1;
    public const int February = 2;
    public const int March = 3;
    public const int April = 4;
    public const int May = 5;
    public const int June = 6;
    public const int July = 7;
    public const int August = 8;
    public const int September = 9;
    public const int October = 10;
    public const int November = 11;
    public const int December = 12;
    
    public const int MinGregorianYear = 1583;
    public const int FederalNewYearsDayStartYear = 1870;
    public const int FederalMartinLutherKingJrDayStartYear = 1986;
    public const int FederalPresidentsDayStartYear = 1885;
    public const int HistoricalPresidentsDayMondayObservanceStartYear = 1971;
    public const int HistoricalMemorialDayStartYear = 1868;
    public const int HistoricalMemorialDayMondayObservanceStartYear = 1971;
    public const int FederalJuneteenthStartYear = 2021;
    public const int FederalIndependenceDayStartYear = 1870;
    public const int FederalLaborDayStartYear = 1894;
    public const int FederalColumbusDayStartYear = 1934;
    public const int HistoricalColumbusDayMondayObservanceStartYear = 1971;
    public const int FederalVeteransDayStartYear = 1938;
    public const int FederalVeteransDayMondayObservanceStartYear = 1971;
    public const int FederalVeteransDayMondayObservanceEndYear = 1977;
    public const int FederalThanksgivingStartYear = 1942;
    public const int FederalChristmasDayStartYear = 1870;
    
    // Calendar Corrections
    
    //Used to calculate the Metonic cycle (variable a). A cycle of 19 solar years is almost exactly 235
    //lunar months, meaning moon phases repeat on the same dates every 19 years.
    public const int MetonicCycleYears = 19;
    
    // This is the most critical of the three. It calculates the "offset" needed to reach the next Sunday after
    // the full moon
    public const int LunarEquationNumerator = 8;
    public const int LunarEquationDenominator = 25;
    
    // This is a historical offset used in the calculation of the Epact (the moon's age). It anchors the calculation
    // to a specific period, ensuring the adjustment equals zero for the century starting in the year 1000.
    public const int HistoricalOffset = 15;
    
    // Weekday and Date Alignment
    
    // An auxiliary constant used to determine the day of the week (variable l). It ensures the result of the
    // modular arithmetic is positive before finding the remainder for Sunday.
    public const int AuxiliaryConstant = 32;

    // Used in the correction variable m to prevent Easter from falling on the same date twice in the same 19-year
    // cycle if the moon's age (epact) is 24 or 25.
    public const int PreventSameDateTwice = 22;
    
    // This is a refinement factor. It identifies rare years (about once every 451 years) where the standard
    // calculation would put Easter on the wrong Sunday, typically when the lunar and solar cycles collide at the
    // end of April.
    public const int RefinementFactor = 451;
    
    // FinalDateMapping
    
    // This constant converts the astronomical result into a specific calendar day
    // Since Easter cannot occur before March 22nd, 114 acts as a base offset.
    // When you divide by 31 (the number of days in March), the quotient determines if the month is March (3) or
    // April (4), and the remainder identifies the day.
    public const int FinalDateMapping = 114;
}
