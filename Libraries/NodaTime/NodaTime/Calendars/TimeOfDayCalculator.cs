// Decompiled with JetBrains decompiler
// Type: NodaTime.Calendars.TimeOfDayCalculator
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Fields;
using NodaTime.Utility;

namespace NodaTime.Calendars
{
  internal static class TimeOfDayCalculator
  {
    internal static readonly PeriodFieldSet TimeFields = new PeriodFieldSet.Builder()
    {
      Ticks = FixedDurationPeriodField.Ticks,
      Milliseconds = FixedDurationPeriodField.Milliseconds,
      Seconds = FixedDurationPeriodField.Seconds,
      Minutes = FixedDurationPeriodField.Minutes,
      Hours = FixedDurationPeriodField.Hours
    }.Build();

    internal static long GetTicks(int hourOfDay, int minuteOfHour)
    {
      Preconditions.CheckArgumentRange(nameof (hourOfDay), hourOfDay, 0, 23);
      Preconditions.CheckArgumentRange(nameof (minuteOfHour), minuteOfHour, 0, 59);
      return (long) hourOfDay * 36000000000L + (long) minuteOfHour * 600000000L;
    }

    internal static long GetTicks(int hourOfDay, int minuteOfHour, int secondOfMinute)
    {
      Preconditions.CheckArgumentRange(nameof (hourOfDay), hourOfDay, 0, 23);
      Preconditions.CheckArgumentRange(nameof (minuteOfHour), minuteOfHour, 0, 59);
      Preconditions.CheckArgumentRange(nameof (secondOfMinute), secondOfMinute, 0, 59);
      return (long) hourOfDay * 36000000000L + (long) minuteOfHour * 600000000L + (long) secondOfMinute * 10000000L;
    }

    internal static long GetTicks(
      int hourOfDay,
      int minuteOfHour,
      int secondOfMinute,
      int millisecondOfSecond,
      int tickOfMillisecond)
    {
      Preconditions.CheckArgumentRange(nameof (hourOfDay), hourOfDay, 0, 23);
      Preconditions.CheckArgumentRange(nameof (minuteOfHour), minuteOfHour, 0, 59);
      Preconditions.CheckArgumentRange(nameof (secondOfMinute), secondOfMinute, 0, 59);
      Preconditions.CheckArgumentRange(nameof (millisecondOfSecond), millisecondOfSecond, 0, 999);
      Preconditions.CheckArgumentRange(nameof (tickOfMillisecond), (long) tickOfMillisecond, 0L, 9999L);
      return (long) hourOfDay * 36000000000L + (long) minuteOfHour * 600000000L + (long) secondOfMinute * 10000000L + (long) millisecondOfSecond * 10000L + (long) tickOfMillisecond;
    }

    internal static long GetTickOfDay(LocalInstant localInstant)
    {
      long ticks = localInstant.Ticks;
      if (ticks < 0L)
        return 863999999999L + (ticks + 1L) % 864000000000L;
      int days = TickArithmetic.FastTicksToDays(ticks);
      return ticks - ((long) days * 52734375L << 14);
    }

    internal static int GetTickOfSecond(LocalInstant localInstant) => TimeOfDayCalculator.GetTickOfSecondFromTickOfDay(TimeOfDayCalculator.GetTickOfDay(localInstant));

    internal static int GetTickOfMillisecond(LocalInstant localInstant) => checked ((int) unchecked (TimeOfDayCalculator.GetTickOfDay(localInstant) % 10000L));

    internal static int GetMillisecondOfSecond(LocalInstant localInstant) => TimeOfDayCalculator.GetMillisecondOfSecondFromTickOfDay(TimeOfDayCalculator.GetTickOfDay(localInstant));

    internal static int GetMillisecondOfDay(LocalInstant localInstant) => checked ((int) unchecked (TimeOfDayCalculator.GetTickOfDay(localInstant) / 10000L));

    internal static int GetSecondOfMinute(LocalInstant localInstant) => TimeOfDayCalculator.GetSecondOfMinuteFromTickOfDay(TimeOfDayCalculator.GetTickOfDay(localInstant));

    internal static int GetSecondOfDay(LocalInstant localInstant) => checked ((int) unchecked (TimeOfDayCalculator.GetTickOfDay(localInstant) / 10000000L));

    internal static int GetMinuteOfHour(LocalInstant localInstant) => TimeOfDayCalculator.GetMinuteOfHourFromTickOfDay(TimeOfDayCalculator.GetTickOfDay(localInstant));

    internal static int GetMinuteOfDay(LocalInstant localInstant) => checked ((int) unchecked (TimeOfDayCalculator.GetTickOfDay(localInstant) / 600000000L));

    internal static int GetHourOfDay(LocalInstant localInstant) => TimeOfDayCalculator.GetHourOfDayFromTickOfDay(TimeOfDayCalculator.GetTickOfDay(localInstant));

    internal static int GetHourOfHalfDay(LocalInstant localInstant) => TimeOfDayCalculator.GetHourOfDay(localInstant) % 12;

    internal static int GetClockHourOfHalfDay(LocalInstant localInstant)
    {
      int hourOfHalfDay = TimeOfDayCalculator.GetHourOfHalfDay(localInstant);
      return hourOfHalfDay != 0 ? hourOfHalfDay : 12;
    }

    internal static int GetHourOfDayFromTickOfDay(long tickOfDay) => checked ((int) (tickOfDay >> 11)) / 17578125;

    internal static int GetMinuteOfHourFromTickOfDay(long tickOfDay) => checked ((int) unchecked (tickOfDay / 600000000L)) % 60;

    internal static int GetSecondOfMinuteFromTickOfDay(long tickOfDay) => checked ((int) unchecked (tickOfDay / 10000000L)) % 60;

    internal static int GetMillisecondOfSecondFromTickOfDay(long tickOfDay) => checked ((int) unchecked (tickOfDay / 10000L % 1000L));

    internal static int GetTickOfSecondFromTickOfDay(long tickOfDay) => checked ((int) unchecked (tickOfDay % 10000000L));
  }
}
