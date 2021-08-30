// Decompiled with JetBrains decompiler
// Type: NodaTime.Calendars.FixedMonthYearMonthDayCalculator
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Utility;

namespace NodaTime.Calendars
{
  internal abstract class FixedMonthYearMonthDayCalculator : RegularYearMonthDayCalculator
  {
    private const int DaysInMonth = 30;
    private const long TicksPerMonth = 25920000000000;
    private const long AverageTicksPerYear = 315576000000000;

    protected FixedMonthYearMonthDayCalculator(
      int minYear,
      int maxYear,
      long ticksAtStartOfYear1,
      params Era[] eras)
      : base(minYear, maxYear, 13, 315576000000000L, ticksAtStartOfYear1, eras)
    {
    }

    internal override LocalInstant SetYear(LocalInstant localInstant, int year)
    {
      int year1 = this.GetYear(localInstant);
      int dayOfYear = this.GetDayOfYear(localInstant, year1);
      long tickOfDay = TimeOfDayCalculator.GetTickOfDay(localInstant);
      if (dayOfYear > 365 && !this.IsLeapYear(year))
        checked { --dayOfYear; }
      return new LocalInstant(checked (this.GetStartOfYearInTicks(year) + (long) (dayOfYear - 1) * 864000000000L + tickOfDay));
    }

    internal override LocalInstant GetLocalInstant(
      int year,
      int monthOfYear,
      int dayOfMonth)
    {
      Preconditions.CheckArgumentRange(nameof (year), year, this.MinYear, this.MaxYear);
      Preconditions.CheckArgumentRange(nameof (monthOfYear), monthOfYear, 1, 13);
      Preconditions.CheckArgumentRange(nameof (dayOfMonth), dayOfMonth, 1, this.GetDaysInMonth(year, monthOfYear));
      return new LocalInstant(checked ((long) (this.GetStartOfYearInDays(year) + (monthOfYear - 1) * 30 + (dayOfMonth - 1)) * 864000000000L));
    }

    protected override long GetTicksFromStartOfYearToStartOfMonth(int year, int month) => checked ((long) (month - 1) * 25920000000000L);

    internal override int GetDayOfMonth(LocalInstant localInstant) => checked (unchecked (checked (this.GetDayOfYear(localInstant) - 1) % 30) + 1);

    internal override bool IsLeapYear(int year) => (year & 3) == 3;

    internal override int GetDaysInMonth(int year, int month)
    {
      if (month != 13)
        return 30;
      return !this.IsLeapYear(year) ? 5 : 6;
    }

    internal override int GetMonthOfYear(LocalInstant localInstant) => checked (unchecked (checked (this.GetDayOfYear(localInstant) - 1) / 30) + 1);

    protected override int GetMonthOfYear(LocalInstant localInstant, int year) => checked ((int) unchecked (checked (localInstant.Ticks - this.GetStartOfYearInTicks(year)) / 25920000000000L) + 1);
  }
}
