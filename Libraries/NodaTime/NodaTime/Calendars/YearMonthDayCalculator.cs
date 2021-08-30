// Decompiled with JetBrains decompiler
// Type: NodaTime.Calendars.YearMonthDayCalculator
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Utility;
using System;

namespace NodaTime.Calendars
{
  internal abstract class YearMonthDayCalculator
  {
    private readonly YearStartCacheEntry[] yearCache = YearStartCacheEntry.CreateCache();
    private readonly Era[] eras;
    private readonly int minYear;
    private readonly int maxYear;
    private readonly long averageTicksPerYear;
    private readonly long ticksAtStartOfYear1;

    internal Era[] Eras => this.eras;

    internal int MinYear => this.minYear;

    internal int MaxYear => this.maxYear;

    internal long TicksAtStartOfYear1 => this.ticksAtStartOfYear1;

    protected YearMonthDayCalculator(
      int minYear,
      int maxYear,
      long averageTicksPerYear,
      long ticksAtStartOfYear1,
      Era[] eras)
    {
      Preconditions.CheckArgument(maxYear < 64512, nameof (maxYear), "Calendar year range would invalidate caching.");
      this.minYear = minYear;
      this.maxYear = maxYear;
      this.eras = Preconditions.CheckNotNull<Era[]>(eras, nameof (eras));
      this.averageTicksPerYear = averageTicksPerYear;
      this.ticksAtStartOfYear1 = ticksAtStartOfYear1;
    }

    protected abstract long GetTicksFromStartOfYearToStartOfMonth(int year, int month);

    protected abstract int CalculateStartOfYearDays(int year);

    protected abstract int GetMonthOfYear(LocalInstant localInstant, int year);

    internal abstract int GetMaxMonth(int year);

    internal abstract LocalInstant SetYear(LocalInstant localInstant, int year);

    internal abstract int GetDaysInMonth(int year, int month);

    internal abstract bool IsLeapYear(int year);

    internal abstract LocalInstant AddMonths(LocalInstant localInstant, int months);

    internal abstract int MonthsBetween(LocalInstant minuendInstant, LocalInstant subtrahendInstant);

    internal virtual long GetStartOfYearInTicks(int year) => (long) this.GetStartOfYearInDays(year) * 864000000000L;

    internal virtual int GetDayOfMonth(LocalInstant localInstant)
    {
      int year = this.GetYear(localInstant);
      int monthOfYear = this.GetMonthOfYear(localInstant, year);
      return this.GetDayOfMonth(localInstant, year, monthOfYear);
    }

    protected int GetDayOfMonth(LocalInstant localInstant, int year, int month)
    {
      long yearMonthTicks = this.GetYearMonthTicks(year, month);
      return TickArithmetic.FastTicksToDays(localInstant.Ticks - yearMonthTicks) + 1;
    }

    internal int GetDayOfYear(LocalInstant localInstant) => this.GetDayOfYear(localInstant, this.GetYear(localInstant));

    internal int GetDayOfYear(LocalInstant localInstant, int year)
    {
      long startOfYearInTicks = this.GetStartOfYearInTicks(year);
      return TickArithmetic.FastTicksToDays(localInstant.Ticks - startOfYearInTicks) + 1;
    }

    internal virtual int GetMonthOfYear(LocalInstant localInstant) => this.GetMonthOfYear(localInstant, this.GetYear(localInstant));

    internal virtual LocalInstant GetLocalInstant(
      int year,
      int monthOfYear,
      int dayOfMonth)
    {
      Preconditions.CheckArgumentRange(nameof (year), year, this.MinYear, this.MaxYear);
      Preconditions.CheckArgumentRange(nameof (monthOfYear), monthOfYear, 1, this.GetMaxMonth(year));
      Preconditions.CheckArgumentRange(nameof (dayOfMonth), dayOfMonth, 1, this.GetDaysInMonth(year, monthOfYear));
      return new LocalInstant(this.GetYearMonthDayTicks(year, monthOfYear, dayOfMonth));
    }

    internal long GetYearMonthDayTicks(int year, int month, int dayOfMonth) => checked (this.GetYearMonthTicks(year, month) + (long) (dayOfMonth - 1) * 864000000000L);

    internal long GetYearMonthTicks(int year, int month) => checked (this.GetStartOfYearInTicks(year) + this.GetTicksFromStartOfYearToStartOfMonth(year, month));

    internal virtual LocalInstant GetLocalInstant(
      Era era,
      int yearOfEra,
      int monthOfYear,
      int dayOfMonth)
    {
      this.GetEraIndex(era);
      return this.GetLocalInstant(yearOfEra, monthOfYear, dayOfMonth);
    }

    protected int GetEraIndex(Era era)
    {
      Preconditions.CheckNotNull<Era>(era, nameof (era));
      int num = Array.IndexOf<Era>(this.Eras, era);
      Preconditions.CheckArgument(num != -1, nameof (era), "Era is not used in this calendar");
      return num;
    }

    internal int GetYear(LocalInstant localInstant)
    {
      long ticks = localInstant.Ticks;
      long num1 = this.averageTicksPerYear >> 1;
      long num2 = checked (ticks >> 1 - this.ticksAtStartOfYear1 >> 1);
      if (num2 < 0L)
        checked { num2 += 1L - num1; }
      int year = checked ((int) unchecked (num2 / num1) + 1);
      long startOfYearInTicks = this.GetStartOfYearInTicks(year);
      long num3 = checked (ticks - startOfYearInTicks);
      if (num3 < 0L)
      {
        do
        {
          checked { --year; }
          checked { num3 += this.GetTicksInYear(year); }
        }
        while (num3 < 0L);
        return year;
      }
      for (long ticksInYear = this.GetTicksInYear(year); num3 >= ticksInYear; ticksInYear = this.GetTicksInYear(year))
      {
        checked { ++year; }
        checked { num3 -= ticksInYear; }
      }
      return year;
    }

    internal virtual int GetYearOfEra(LocalInstant localInstant) => this.GetYear(localInstant);

    internal virtual int GetCenturyOfEra(LocalInstant localInstant)
    {
      int yearOfEra = this.GetYearOfEra(localInstant);
      int num1 = yearOfEra % 100;
      int num2 = yearOfEra / 100;
      return num1 != 0 ? checked (num2 + 1) : num2;
    }

    internal virtual int GetYearOfCentury(LocalInstant localInstant)
    {
      int num = this.GetYearOfEra(localInstant) % 100;
      return num != 0 ? num : 100;
    }

    internal virtual int GetEra(LocalInstant localInstant) => 0;

    internal virtual int GetDaysInYear(int year) => !this.IsLeapYear(year) ? 365 : 366;

    protected virtual long GetTicksInYear(int year) => !this.IsLeapYear(year) ? 315360000000000L : 316224000000000L;

    internal virtual int GetAbsoluteYear(int yearOfEra, int eraIndex) => yearOfEra >= 1 && yearOfEra <= this.MaxYear ? yearOfEra : throw new ArgumentOutOfRangeException(nameof (yearOfEra));

    internal virtual int GetMinYearOfEra(int eraIndex) => 1;

    internal virtual int GetMaxYearOfEra(int eraIndex) => this.MaxYear;

    protected int GetStartOfYearInDays(int year)
    {
      if (year < this.MinYear || year > this.MaxYear)
        return this.CalculateStartOfYearDays(year);
      int cacheIndex = YearStartCacheEntry.GetCacheIndex(year);
      YearStartCacheEntry yearStartCacheEntry = this.yearCache[cacheIndex];
      if (!yearStartCacheEntry.IsValidForYear(year))
      {
        int startOfYearDays = this.CalculateStartOfYearDays(year);
        yearStartCacheEntry = new YearStartCacheEntry(year, startOfYearDays);
        this.yearCache[cacheIndex] = yearStartCacheEntry;
      }
      return yearStartCacheEntry.StartOfYearDays;
    }
  }
}
