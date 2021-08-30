// Decompiled with JetBrains decompiler
// Type: NodaTime.Calendars.WeekYearCalculator
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Utility;

namespace NodaTime.Calendars
{
  internal sealed class WeekYearCalculator
  {
    private readonly YearMonthDayCalculator yearMonthDayCalculator;
    private readonly int minDaysInFirstWeek;

    internal WeekYearCalculator(
      YearMonthDayCalculator yearMonthDayCalculator,
      int minDaysInFirstWeek)
    {
      this.yearMonthDayCalculator = yearMonthDayCalculator;
      this.minDaysInFirstWeek = minDaysInFirstWeek;
    }

    internal LocalInstant GetLocalInstant(
      int weekYear,
      int weekOfWeekYear,
      IsoDayOfWeek dayOfWeek)
    {
      Preconditions.CheckArgumentRange(nameof (weekYear), weekYear, this.yearMonthDayCalculator.MinYear, this.yearMonthDayCalculator.MaxYear);
      Preconditions.CheckArgumentRange(nameof (weekOfWeekYear), weekOfWeekYear, 1, this.GetWeeksInWeekYear(weekYear));
      Preconditions.CheckArgumentRange(nameof (dayOfWeek), (int) dayOfWeek, 1, 7);
      return new LocalInstant(checked (this.GetWeekYearTicks(weekYear) + (long) (weekOfWeekYear - 1) * 6048000000000L + (long) unchecked (dayOfWeek - 1) * 864000000000L));
    }

    internal static int GetDayOfWeek(LocalInstant localInstant)
    {
      long ticks = localInstant.Ticks;
      long num;
      if (ticks >= 0L)
      {
        num = (long) TickArithmetic.FastTicksToDays(ticks);
      }
      else
      {
        num = ((ticks >> 14) - 52734374L) / 52734375L;
        if (num < -3L)
          return 7 + (int) ((num + 4L) % 7L);
      }
      return 1 + (int) ((num + 3L) % 7L);
    }

    internal int GetWeekOfWeekYear(LocalInstant localInstant)
    {
      long weekYearTicks = this.GetWeekYearTicks(this.GetWeekYear(localInstant));
      return checked ((int) unchecked (checked (localInstant.Ticks - weekYearTicks) / 6048000000000L) + 1);
    }

    private int GetWeeksInWeekYear(int weekYear)
    {
      long weekYearTicks = this.GetWeekYearTicks(weekYear);
      int num = checked ((int) unchecked (checked (this.yearMonthDayCalculator.GetStartOfYearInTicks(weekYear) - weekYearTicks) / 864000000000L));
      return checked (this.yearMonthDayCalculator.GetDaysInYear(weekYear) + num + this.minDaysInFirstWeek - 1) / 7;
    }

    private long GetWeekYearTicks(int weekYear)
    {
      long startOfYearInTicks = this.yearMonthDayCalculator.GetStartOfYearInTicks(weekYear);
      int dayOfWeek = WeekYearCalculator.GetDayOfWeek(new LocalInstant(startOfYearInTicks));
      return dayOfWeek > checked (8 - this.minDaysInFirstWeek) ? checked (startOfYearInTicks + (long) (8 - dayOfWeek) * 864000000000L) : checked (startOfYearInTicks - (long) (dayOfWeek - 1) * 864000000000L);
    }

    internal int GetWeekYear(LocalInstant localInstant)
    {
      int year = this.yearMonthDayCalculator.GetYear(localInstant);
      long weekYearTicks = this.GetWeekYearTicks(year);
      if (localInstant.Ticks < weekYearTicks)
        return checked (year - 1);
      int weeksInWeekYear = this.GetWeeksInWeekYear(year);
      long num = checked (weekYearTicks + (long) weeksInWeekYear * 6048000000000L);
      return localInstant.Ticks >= num ? checked (year + 1) : year;
    }
  }
}
