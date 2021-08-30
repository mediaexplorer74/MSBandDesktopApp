// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.DateTimeOffsetExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class DateTimeOffsetExtensions
  {
    public static DateTimeOffset RoundDown(
      this DateTimeOffset dateTime,
      TimeSpan interval)
    {
      long ticks1 = dateTime.Ticks;
      long ticks2 = interval.Ticks;
      long num = ticks2;
      return new DateTimeOffset(ticks1 / num * ticks2, dateTime.Offset);
    }

    public static DateTimeOffset RoundUp(
      this DateTimeOffset dateTime,
      TimeSpan interval)
    {
      long ticks1 = dateTime.Ticks;
      long ticks2 = interval.Ticks;
      long num = ticks2;
      return new DateTimeOffset((ticks1 + num - 1L) / ticks2 * ticks2, dateTime.Offset);
    }

    public static IEnumerable<DateTimeOffset> Range(
      this DateTimeOffset dateTime,
      DateTimeOffset endTime,
      TimeSpan duration)
    {
      DateTimeOffset currentTime = dateTime;
      while (currentTime + duration <= endTime)
      {
        yield return currentTime;
        currentTime += duration;
      }
    }

    public static DateTimeOffset WithZeroOffset(this DateTimeOffset dateTime) => dateTime.WithOffset(TimeSpan.Zero);

    public static DateTimeOffset WithCurrentOffset(this DateTimeOffset dateTime) => dateTime.WithOffset(TimeZoneInfo.Local.BaseUtcOffset);

    public static DateTimeOffset WithOffset(
      this DateTimeOffset dateTime,
      TimeSpan offset)
    {
      return new DateTimeOffset(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, offset);
    }
  }
}
