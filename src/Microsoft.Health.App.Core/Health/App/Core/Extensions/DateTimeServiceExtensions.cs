// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.DateTimeServiceExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Services;
using Microsoft.Health.App.Core.Utilities;
using NodaTime;
using NodaTime.TimeZones;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class DateTimeServiceExtensions
  {
    private const int DaysInWeek = 7;

    public static IReadOnlyList<TzdbZoneLocation> GetTimeZoneLocations(
      this IDateTimeService dateTimeService)
    {
      return (IReadOnlyList<TzdbZoneLocation>) new ReadOnlyCollection<TzdbZoneLocation>(TzdbDateTimeZoneSource.Default.ZoneLocations);
    }

    public static IReadOnlyCollection<string> GetTimeZoneIds(
      this IDateTimeService dateTimeService)
    {
      return (IReadOnlyCollection<string>) DateTimeZoneProviders.Tzdb.Ids;
    }

    public static DateTimeZone TimeZoneWithId(
      this IDateTimeService dateTimeService,
      string id)
    {
      return DateTimeZoneProviders.Tzdb.GetZoneOrNull(id);
    }

    public static DateTimeOffset ToLocalTime(
      this IDateTimeService dateTimeService,
      DateTimeOffset dateTimeOffset)
    {
      return OffsetDateTime.FromDateTimeOffset(dateTimeOffset).ToInstant().InZone(dateTimeService.TimeZone).ToDateTimeOffset();
    }

    public static DateTimeOffset GetToday(this IDateTimeService dateTimeService) => (DateTimeOffset) dateTimeService.Now.Date;

    public static DateTimeOffset GetTomorrow(this IDateTimeService dateTimeService) => dateTimeService.AddDays(dateTimeService.GetToday(), 1);

    public static DateTimeOffset GetTomorrowLastWeek(
      this IDateTimeService dateTimeService)
    {
      return dateTimeService.AddWeeks(dateTimeService.GetTomorrow(), -1);
    }

    public static Range<DateTimeOffset> GetWeekThroughToday(
      this IDateTimeService dateTimeService)
    {
      return Range.GetExclusiveHigh<DateTimeOffset>(dateTimeService.GetTomorrowLastWeek(), dateTimeService.GetTomorrow());
    }

    public static Range<DateTimeOffset> GetTodayToTomorrow(
      this IDateTimeService dateTimeService)
    {
      return Range.GetExclusiveHigh<DateTimeOffset>(dateTimeService.GetToday(), dateTimeService.GetTomorrow());
    }

    public static Range<DateTimeOffset> GetDateThroughDay(
      this IDateTimeService dateTimeService,
      DateTimeOffset dateTimeOffset)
    {
      return Range.GetExclusiveHigh<DateTimeOffset>(dateTimeService.ToLocalTime(dateTimeOffset), dateTimeService.AddDays(dateTimeOffset, 1));
    }

    public static Range<DateTimeOffset> GetDateThroughWeek(
      this IDateTimeService dateTimeService,
      DateTimeOffset dateTimeOffset)
    {
      return Range.GetExclusiveHigh<DateTimeOffset>(dateTimeService.ToLocalTime(dateTimeOffset), dateTimeService.AddWeeks(dateTimeOffset, 1));
    }

    public static DateTimeOffset AddWeeks(
      this IDateTimeService dateTimeService,
      DateTimeOffset dateTimeOffset,
      int weeks)
    {
      return dateTimeService.AddTimeZoneAwareTime(dateTimeOffset, TimeSpan.FromDays((double) (weeks * 7)));
    }

    public static DateTimeOffset AddDays(
      this IDateTimeService dateTimeService,
      DateTimeOffset dateTimeOffset,
      int days)
    {
      return dateTimeService.AddTimeZoneAwareTime(dateTimeOffset, TimeSpan.FromDays((double) days));
    }

    public static DateTimeOffset AddHours(
      this IDateTimeService dateTimeService,
      DateTimeOffset dateTimeOffset,
      int hours)
    {
      return dateTimeService.AddTimeZoneAwareTime(dateTimeOffset, TimeSpan.FromHours((double) hours));
    }

    public static DateTimeOffset AddTimeZoneAwareTime(
      this IDateTimeService dateTimeService,
      DateTimeOffset dateTimeOffset,
      TimeSpan timeSpan)
    {
      DateTimeOffset localTime1 = dateTimeService.ToLocalTime(dateTimeOffset);
      DateTimeOffset localTime2 = dateTimeService.ToLocalTime(dateTimeOffset.Add(timeSpan));
      return localTime2.Add(localTime1.Offset - localTime2.Offset);
    }

    public static void ForEachTimeZoneDay(
      this IDateTimeService dateTimeService,
      Range<DateTimeOffset> range,
      Action<int, Range<DateTimeOffset>> action)
    {
      DateTimeOffset localTime = dateTimeService.ToLocalTime(range.Low);
      DateTimeOffset date1 = (DateTimeOffset) localTime.Date;
      localTime = dateTimeService.ToLocalTime(range.High);
      DateTimeOffset date2 = (DateTimeOffset) localTime.Date;
      int num = 0;
      for (DateTimeOffset dateTimeOffset = date1; dateTimeOffset < date2; dateTimeOffset = dateTimeService.AddDays(dateTimeOffset, 1))
        action(num++, Range.GetExclusiveHigh<DateTimeOffset>((DateTimeOffset) dateTimeOffset.Date, (DateTimeOffset) dateTimeService.AddDays(dateTimeOffset, 1).Date));
    }

    public static void ForEachTimeZoneWeek(
      this IDateTimeService dateTimeService,
      Range<DateTimeOffset> range,
      Action<Range<int>, Range<DateTimeOffset>> action)
    {
      DateTimeOffset localTime = dateTimeService.ToLocalTime(range.Low);
      DateTimeOffset date1 = (DateTimeOffset) localTime.Date;
      localTime = dateTimeService.ToLocalTime(range.High);
      DateTimeOffset date2 = (DateTimeOffset) localTime.Date;
      int low = 0;
      for (DateTimeOffset dateTimeOffset = date1; dateTimeOffset < date2; dateTimeOffset = dateTimeService.AddWeeks(dateTimeOffset, 1))
      {
        action(Range.GetExclusiveHigh<int>(low, low + 7), Range.GetExclusiveHigh<DateTimeOffset>((DateTimeOffset) dateTimeOffset.Date, (DateTimeOffset) dateTimeService.AddWeeks(dateTimeOffset, 1).Date));
        low += 7;
      }
    }

    public static IEnumerable<Range<DateTimeOffset>> SplitByMidnightUtcOffset(
      this IDateTimeService dateTimeService,
      Range<DateTimeOffset> range)
    {
      DateTimeOffset dateTimeOffset = (DateTimeOffset) dateTimeService.ToLocalTime(range.Low).Date;
      DateTimeOffset end = (DateTimeOffset) dateTimeService.ToLocalTime(range.High).Date;
      TimeSpan offset = dateTimeOffset.Offset;
      DateTimeOffset i;
      for (i = dateTimeOffset; i < end; i = dateTimeService.AddDays(i, 1))
      {
        if (i.Offset != offset)
        {
          yield return Range.GetExclusiveHigh<DateTimeOffset>((DateTimeOffset) dateTimeOffset.Date, (DateTimeOffset) i.Date);
          dateTimeOffset = i;
          offset = dateTimeOffset.Offset;
        }
      }
      i = new DateTimeOffset();
      yield return Range.GetExclusiveHigh<DateTimeOffset>((DateTimeOffset) dateTimeOffset.Date, (DateTimeOffset) end.Date);
    }

    public static Assembly GetNodaTimeAssembly() => typeof (LocalDate).GetTypeInfo().Assembly;
  }
}
