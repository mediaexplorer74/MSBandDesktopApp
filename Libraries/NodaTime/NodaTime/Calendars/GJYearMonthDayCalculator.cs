// Decompiled with JetBrains decompiler
// Type: NodaTime.Calendars.GJYearMonthDayCalculator
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Utility;

namespace NodaTime.Calendars
{
  internal abstract class GJYearMonthDayCalculator : RegularYearMonthDayCalculator
  {
    private static readonly int[] MinDaysPerMonth = new int[12]
    {
      31,
      28,
      31,
      30,
      31,
      30,
      31,
      31,
      30,
      31,
      30,
      31
    };
    private static readonly int[] MaxDaysPerMonth = new int[12]
    {
      31,
      29,
      31,
      30,
      31,
      30,
      31,
      31,
      30,
      31,
      30,
      31
    };
    private static readonly long[] MinTotalTicksByMonth = new long[12];
    private static readonly long[] MaxTotalTicksByMonth = new long[12];

    static GJYearMonthDayCalculator()
    {
      long num1 = 0;
      long num2 = 0;
      int index = 0;
      while (index < 11)
      {
        checked { num1 += (long) GJYearMonthDayCalculator.MinDaysPerMonth[index] * 864000000000L; }
        checked { num2 += (long) GJYearMonthDayCalculator.MaxDaysPerMonth[index] * 864000000000L; }
        GJYearMonthDayCalculator.MinTotalTicksByMonth[checked (index + 1)] = num1;
        GJYearMonthDayCalculator.MaxTotalTicksByMonth[checked (index + 1)] = num2;
        checked { ++index; }
      }
    }

    protected GJYearMonthDayCalculator(
      int minYear,
      int maxYear,
      long averageTicksPerYear,
      long ticksAtStartOfYear1)
      : base(minYear, maxYear, 12, averageTicksPerYear, ticksAtStartOfYear1, Era.BeforeCommon, Era.Common)
    {
    }

    protected override int GetMonthOfYear(LocalInstant localInstant, int year)
    {
      int num = checked ((int) unchecked (checked (localInstant.Ticks - this.GetStartOfYearInTicks(year)) >> 10 / 10000L));
      if (!this.IsLeapYear(year))
      {
        if (num >= 15271875)
        {
          if (num >= 23034375)
          {
            if (num < 25650000)
              return 10;
            return num >= 28181250 ? 12 : 11;
          }
          if (num < 17887500)
            return 7;
          return num >= 20503125 ? 9 : 8;
        }
        if (num >= 7593750)
        {
          if (num < 10125000)
            return 4;
          return num >= 12740625 ? 6 : 5;
        }
        if (num < 2615625)
          return 1;
        return num >= 4978125 ? 3 : 2;
      }
      if (num >= 15356250)
      {
        if (num >= 23118750)
        {
          if (num < 25734375)
            return 10;
          return num >= 28265625 ? 12 : 11;
        }
        if (num < 17971875)
          return 7;
        return num >= 20587500 ? 9 : 8;
      }
      if (num >= 7678125)
      {
        if (num < 10209375)
          return 4;
        return num >= 12825000 ? 6 : 5;
      }
      if (num < 2615625)
        return 1;
      return num >= 5062500 ? 3 : 2;
    }

    internal override int GetDaysInMonth(int year, int month) => !this.IsLeapYear(year) ? GJYearMonthDayCalculator.MinDaysPerMonth[checked (month - 1)] : GJYearMonthDayCalculator.MaxDaysPerMonth[checked (month - 1)];

    protected override long GetTicksFromStartOfYearToStartOfMonth(int year, int month) => !this.IsLeapYear(year) ? GJYearMonthDayCalculator.MinTotalTicksByMonth[checked (month - 1)] : GJYearMonthDayCalculator.MaxTotalTicksByMonth[checked (month - 1)];

    internal override LocalInstant GetLocalInstant(
      Era era,
      int yearOfEra,
      int monthOfYear,
      int dayOfMonth)
    {
      int eraIndex = this.GetEraIndex(era);
      Preconditions.CheckArgumentRange(nameof (yearOfEra), yearOfEra, 1, this.GetMaxYearOfEra(eraIndex));
      return this.GetLocalInstant(this.GetAbsoluteYear(yearOfEra, eraIndex), monthOfYear, dayOfMonth);
    }

    internal override LocalInstant SetYear(LocalInstant localInstant, int year)
    {
      int year1 = this.GetYear(localInstant);
      int dayOfYear = this.GetDayOfYear(localInstant, year1);
      long tickOfDay = TimeOfDayCalculator.GetTickOfDay(localInstant);
      if (dayOfYear > 59)
      {
        if (this.IsLeapYear(year1))
        {
          if (!this.IsLeapYear(year))
            checked { --dayOfYear; }
        }
        else if (this.IsLeapYear(year))
          checked { ++dayOfYear; }
      }
      return new LocalInstant(checked (this.GetYearMonthDayTicks(year, 1, dayOfYear) + tickOfDay));
    }

    internal override int GetAbsoluteYear(int yearOfEra, int eraIndex) => eraIndex != 0 ? yearOfEra : checked (1 - yearOfEra);

    internal override int GetMaxYearOfEra(int eraIndex) => eraIndex != 0 ? this.MaxYear : checked (1 - this.MinYear);

    internal override int GetYearOfEra(LocalInstant localInstant)
    {
      int year = this.GetYear(localInstant);
      return year > 0 ? year : checked (-year + 1);
    }

    internal override int GetEra(LocalInstant localInstant) => localInstant.Ticks >= this.TicksAtStartOfYear1 ? 1 : 0;
  }
}
