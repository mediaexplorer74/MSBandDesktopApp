// Decompiled with JetBrains decompiler
// Type: NodaTime.Calendars.GregorianYearMonthDayCalculator
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

namespace NodaTime.Calendars
{
  internal class GregorianYearMonthDayCalculator : GJYearMonthDayCalculator
  {
    private const int FirstOptimizedYear = 1900;
    private const int LastOptimizedYear = 2100;
    private const int DaysFrom0000To1970 = 719527;
    private const long AverageTicksPerGregorianYear = 315569520000000;
    private static readonly long[] MonthStartTicks = new long[2413];
    private static readonly int[] MonthLengths = new int[2413];
    private static readonly long[] YearStartTicks = new long[201];
    private static readonly int[] YearStartDays = new int[201];

    static GregorianYearMonthDayCalculator()
    {
      GregorianYearMonthDayCalculator monthDayCalculator = new GregorianYearMonthDayCalculator();
      int year = 1900;
      while (year <= 2100)
      {
        GregorianYearMonthDayCalculator.YearStartDays[checked (year - 1900)] = monthDayCalculator.CalculateStartOfYearDays(year);
        GregorianYearMonthDayCalculator.YearStartTicks[checked (year - 1900)] = checked ((long) GregorianYearMonthDayCalculator.YearStartDays[year - 1900] * 864000000000L);
        int month = 1;
        while (month <= 12)
        {
          int index = checked ((year - 1900) * 12 + month);
          GregorianYearMonthDayCalculator.MonthStartTicks[index] = monthDayCalculator.GetYearMonthTicks(year, month);
          GregorianYearMonthDayCalculator.MonthLengths[index] = monthDayCalculator.GetDaysInMonth(year, month);
          checked { ++month; }
        }
        checked { ++year; }
      }
    }

    internal GregorianYearMonthDayCalculator()
      : base(-27255, 31195, 315569520000000L, -621355968000000000L)
    {
    }

    internal override long GetStartOfYearInTicks(int year) => year < 1900 || year > 2100 ? base.GetStartOfYearInTicks(year) : GregorianYearMonthDayCalculator.YearStartTicks[checked (year - 1900)];

    internal override LocalInstant GetLocalInstant(
      int year,
      int monthOfYear,
      int dayOfMonth)
    {
      int index = checked ((year - 1900) * 12 + monthOfYear);
      return year < 1900 || year > 2099 || monthOfYear < 1 || monthOfYear > 12 || dayOfMonth < 1 || dayOfMonth > GregorianYearMonthDayCalculator.MonthLengths[index] ? base.GetLocalInstant(year, monthOfYear, dayOfMonth) : new LocalInstant(GregorianYearMonthDayCalculator.MonthStartTicks[index] + (long) (dayOfMonth - 1) * 864000000000L);
    }

    protected override int CalculateStartOfYearDays(int year)
    {
      int num1 = year / 100;
      int num2;
      if (year < 0)
      {
        num2 = checked ((year + 3 >> 2) - num1 + (num1 + 3 >> 2) - 1);
      }
      else
      {
        num2 = checked ((year >> 2) - num1 + num1 >> 2);
        if (this.IsLeapYear(year))
          checked { --num2; }
      }
      return checked (year * 365 + num2 - 719527);
    }

    internal override bool IsLeapYear(int year)
    {
      if ((year & 3) != 0)
        return false;
      return year % 100 != 0 || year % 400 == 0;
    }
  }
}
