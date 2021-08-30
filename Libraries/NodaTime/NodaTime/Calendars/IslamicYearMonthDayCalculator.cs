// Decompiled with JetBrains decompiler
// Type: NodaTime.Calendars.IslamicYearMonthDayCalculator
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using System;

namespace NodaTime.Calendars
{
  internal sealed class IslamicYearMonthDayCalculator : RegularYearMonthDayCalculator
  {
    private const int MonthPairLength = 59;
    private const int LongMonthLength = 30;
    private const int ShortMonthLength = 29;
    private const long AverageTicksPerYear = 306172802880000;
    private const int DaysPerNonLeapYear = 354;
    private const int DaysPerLeapYear = 355;
    private const long TicksPerNonLeapYear = 305856000000000;
    private const long TicksAtCivilEpoch = -425215872000000000;
    private const long TicksAtAstronomicalEpoch = -425216736000000000;
    private const int LeapYearCycleLength = 30;
    private const int DaysPerLeapCycle = 10631;
    private readonly int leapYearPatternBits;
    private readonly int daysAtStartOfYear1;
    private static readonly long[] TotalTicksByMonth;

    static IslamicYearMonthDayCalculator()
    {
      long num1 = 0;
      IslamicYearMonthDayCalculator.TotalTicksByMonth = new long[12];
      int index = 0;
      while (index < 12)
      {
        IslamicYearMonthDayCalculator.TotalTicksByMonth[index] = num1;
        int num2 = (index & 1) == 0 ? 30 : 29;
        checked { num1 += (long) num2 * 864000000000L; }
        checked { ++index; }
      }
    }

    internal IslamicYearMonthDayCalculator(
      IslamicLeapYearPattern leapYearPattern,
      IslamicEpoch epoch)
      : base(1, 31513, 12, 306172802880000L, IslamicYearMonthDayCalculator.GetYear1Ticks(epoch), Era.AnnoHegirae)
    {
      this.daysAtStartOfYear1 = checked ((int) unchecked (this.TicksAtStartOfYear1 / 864000000000L));
      this.leapYearPatternBits = IslamicYearMonthDayCalculator.GetLeapYearPatternBits(leapYearPattern);
    }

    internal override LocalInstant SetYear(LocalInstant localInstant, int year)
    {
      int year1 = this.GetYear(localInstant);
      int dayOfYear = this.GetDayOfYear(localInstant, year1);
      if (dayOfYear == 355 && !this.IsLeapYear(year))
        checked { --dayOfYear; }
      long tickOfDay = TimeOfDayCalculator.GetTickOfDay(localInstant);
      return new LocalInstant(checked (this.GetStartOfYearInTicks(year) + (long) (dayOfYear - 1) * 864000000000L + tickOfDay));
    }

    protected override long GetTicksFromStartOfYearToStartOfMonth(int year, int month) => IslamicYearMonthDayCalculator.TotalTicksByMonth[checked (month - 1)];

    internal override int GetDayOfMonth(LocalInstant localInstant)
    {
      int dayOfYear = this.GetDayOfYear(localInstant);
      return dayOfYear == 355 ? 30 : checked (unchecked (checked (dayOfYear - 1) % 59 % 30) + 1);
    }

    internal override bool IsLeapYear(int year) => (this.leapYearPatternBits & 1 << (year >= 0 ? year % 30 : checked (unchecked (year % 30) + 30))) > 0;

    internal override int GetDaysInYear(int year) => !this.IsLeapYear(year) ? 354 : 355;

    protected override long GetTicksInYear(int year) => !this.IsLeapYear(year) ? 305856000000000L : 306720000000000L;

    internal override int GetDaysInMonth(int year, int month) => month == 12 && this.IsLeapYear(year) || (month & 1) != 0 ? 30 : 29;

    protected override int GetMonthOfYear(LocalInstant localInstant, int year)
    {
      int num = checked ((int) unchecked (checked (localInstant.Ticks - this.GetStartOfYearInTicks(year)) / 864000000000L));
      return num == 354 ? 12 : checked (unchecked (checked (num * 2) / 59) + 1);
    }

    protected override int CalculateStartOfYearDays(int year)
    {
      int num1 = year > 0 ? checked (year - 1) / 30 : checked (year - 30) / 30;
      int num2 = checked (num1 * 30 + 1);
      int num3 = checked (this.daysAtStartOfYear1 + num1 * 10631);
      int year1 = num2;
      while (year1 < year)
      {
        checked { num3 += this.GetDaysInYear(year1); }
        checked { ++year1; }
      }
      return num3;
    }

    private static int GetLeapYearPatternBits(IslamicLeapYearPattern leapYearPattern)
    {
      switch (leapYearPattern)
      {
        case IslamicLeapYearPattern.Base15:
          return 623158436;
        case IslamicLeapYearPattern.Base16:
          return 623191204;
        case IslamicLeapYearPattern.Indian:
          return 690562340;
        case IslamicLeapYearPattern.HabashAlHasib:
          return 153692453;
        default:
          throw new ArgumentOutOfRangeException(nameof (leapYearPattern));
      }
    }

    private static long GetYear1Ticks(IslamicEpoch epoch)
    {
      switch (epoch)
      {
        case IslamicEpoch.Astronomical:
          return -425216736000000000;
        case IslamicEpoch.Civil:
          return -425215872000000000;
        default:
          throw new ArgumentOutOfRangeException(nameof (epoch));
      }
    }
  }
}
