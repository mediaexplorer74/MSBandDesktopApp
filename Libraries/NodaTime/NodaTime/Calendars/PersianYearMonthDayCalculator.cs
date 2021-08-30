// Decompiled with JetBrains decompiler
// Type: NodaTime.Calendars.PersianYearMonthDayCalculator
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

namespace NodaTime.Calendars
{
  internal sealed class PersianYearMonthDayCalculator : RegularYearMonthDayCalculator
  {
    private const long LeapYearPatternBits = 1145184802;
    private const int LeapYearCycleLength = 33;
    private const int DaysPerNonLeapYear = 365;
    private const int DaysPerLeapYear = 366;
    private const int DaysPerLeapCycle = 12053;
    private const long AverageTicksPerYear = 315569454545454;
    private const long TicksAtStartOfYear1Constant = -425319552000000000;
    private const int DaysAtStartOfYear1 = -492268;
    private static readonly long[] TotalTicksByMonth;

    static PersianYearMonthDayCalculator()
    {
      long num1 = 0;
      PersianYearMonthDayCalculator.TotalTicksByMonth = new long[13];
      int index = 1;
      while (index <= 12)
      {
        PersianYearMonthDayCalculator.TotalTicksByMonth[index] = num1;
        int num2 = index <= 6 ? 31 : 30;
        checked { num1 += (long) num2 * 864000000000L; }
        checked { ++index; }
      }
    }

    internal PersianYearMonthDayCalculator()
      : base(1, 30574, 12, 315569454545454L, -425319552000000000L, Era.AnnoPersico)
    {
    }

    protected override long GetTicksFromStartOfYearToStartOfMonth(int year, int month) => PersianYearMonthDayCalculator.TotalTicksByMonth[month];

    protected override int CalculateStartOfYearDays(int year)
    {
      int num1 = year > 0 ? checked (year - 1) / 33 : checked (year - 33) / 33;
      int num2 = checked (num1 * 33 + 1);
      int num3 = checked (num1 * 12053 - 492268);
      int year1 = num2;
      while (year1 < year)
      {
        checked { num3 += this.GetDaysInYear(year1); }
        checked { ++year1; }
      }
      return num3;
    }

    protected override int GetMonthOfYear(LocalInstant localInstant, int year)
    {
      int num = checked ((int) unchecked (checked (localInstant.Ticks - this.GetStartOfYearInTicks(year)) / 864000000000L));
      if (num == 365)
        return 12;
      return num < 186 ? checked (unchecked (num / 31) + 1) : checked (unchecked (checked (num - 186) / 30) + 7);
    }

    internal override LocalInstant SetYear(LocalInstant localInstant, int year)
    {
      int year1 = this.GetYear(localInstant);
      int dayOfYear = this.GetDayOfYear(localInstant, year1);
      if (dayOfYear == 366 && !this.IsLeapYear(year))
        checked { --dayOfYear; }
      long tickOfDay = TimeOfDayCalculator.GetTickOfDay(localInstant);
      return new LocalInstant(checked (this.GetStartOfYearInTicks(year) + (long) (dayOfYear - 1) * 864000000000L + tickOfDay));
    }

    internal override int GetDaysInMonth(int year, int month)
    {
      if (month < 7)
        return 31;
      return month >= 12 && !this.IsLeapYear(year) ? 29 : 30;
    }

    internal override int GetDayOfMonth(LocalInstant localInstant)
    {
      int num = checked (this.GetDayOfYear(localInstant) - 1);
      return num < 186 ? checked (unchecked (num % 31) + 1) : checked (unchecked (checked (num - 186) % 30) + 1);
    }

    internal override bool IsLeapYear(int year) => (1145184802L & 1L << (year >= 0 ? year % 33 : checked (unchecked (year % 33) + 33))) > 0L;

    internal override int GetDaysInYear(int year) => !this.IsLeapYear(year) ? 365 : 366;

    protected override long GetTicksInYear(int year) => !this.IsLeapYear(year) ? 315360000000000L : 316224000000000L;
  }
}
