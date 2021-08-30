// Decompiled with JetBrains decompiler
// Type: NodaTime.DateTimeZone
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using JetBrains.Annotations;
using NodaTime.Annotations;
using NodaTime.TimeZones;
using NodaTime.Utility;
using System;
using System.Collections.Generic;

namespace NodaTime
{
  [Immutable]
  public abstract class DateTimeZone : IEquatable<DateTimeZone>, IZoneIntervalMap
  {
    internal const string UtcId = "UTC";
    private const int FixedZoneCacheGranularityMilliseconds = 1800000;
    private const int FixedZoneCacheMinimumMilliseconds = -43200000;
    private const int FixedZoneCacheSize = 55;
    private static readonly DateTimeZone UtcZone = (DateTimeZone) new FixedDateTimeZone(Offset.Zero);
    private static readonly DateTimeZone[] FixedZoneCache = DateTimeZone.BuildFixedZoneCache();
    private readonly string id;
    private readonly bool isFixed;
    private readonly long minOffsetTicks;
    private readonly long maxOffsetTicks;

    public static DateTimeZone Utc => DateTimeZone.UtcZone;

    public static DateTimeZone ForOffset(Offset offset)
    {
      int milliseconds = offset.Milliseconds;
      if (milliseconds % 1800000 != 0)
        return (DateTimeZone) new FixedDateTimeZone(offset);
      int index = checked (milliseconds - -43200000) / 1800000;
      return index < 0 || index >= 55 ? (DateTimeZone) new FixedDateTimeZone(offset) : DateTimeZone.FixedZoneCache[index];
    }

    protected DateTimeZone(string id, bool isFixed, Offset minOffset, Offset maxOffset)
    {
      this.id = id;
      this.isFixed = isFixed;
      this.minOffsetTicks = minOffset.Ticks;
      this.maxOffsetTicks = maxOffset.Ticks;
    }

    public string Id => this.id;

    internal bool IsFixed => this.isFixed;

    public Offset MinOffset => Offset.FromTicks(this.minOffsetTicks);

    public Offset MaxOffset => Offset.FromTicks(this.maxOffsetTicks);

    public virtual Offset GetUtcOffset(Instant instant) => this.GetZoneInterval(instant).WallOffset;

    public abstract ZoneInterval GetZoneInterval(Instant instant);

    internal virtual ZoneIntervalPair GetZoneIntervalPair(LocalInstant localInstant)
    {
      ZoneInterval zoneInterval = this.GetZoneInterval(new Instant(localInstant.Ticks));
      if (zoneInterval.Contains(localInstant))
      {
        ZoneInterval matchingInterval1 = this.GetEarlierMatchingInterval(zoneInterval, localInstant);
        if (matchingInterval1 != null)
          return ZoneIntervalPair.Ambiguous(matchingInterval1, zoneInterval);
        ZoneInterval matchingInterval2 = this.GetLaterMatchingInterval(zoneInterval, localInstant);
        return matchingInterval2 != null ? ZoneIntervalPair.Ambiguous(zoneInterval, matchingInterval2) : ZoneIntervalPair.Unambiguous(zoneInterval);
      }
      ZoneInterval matchingInterval3 = this.GetEarlierMatchingInterval(zoneInterval, localInstant);
      if (matchingInterval3 != null)
        return ZoneIntervalPair.Unambiguous(matchingInterval3);
      ZoneInterval matchingInterval4 = this.GetLaterMatchingInterval(zoneInterval, localInstant);
      return matchingInterval4 != null ? ZoneIntervalPair.Unambiguous(matchingInterval4) : ZoneIntervalPair.NoMatch;
    }

    public ZonedDateTime AtStartOfDay(LocalDate date)
    {
      LocalInstant localInstant = date.AtMidnight().LocalInstant;
      ZoneIntervalPair zoneIntervalPair = this.GetZoneIntervalPair(localInstant);
      switch (zoneIntervalPair.MatchingIntervals)
      {
        case 0:
          ZoneInterval intervalAfterGap = this.GetIntervalAfterGap(localInstant);
          LocalDateTime localDateTime = new LocalDateTime(intervalAfterGap.LocalStart, date.Calendar);
          if (localDateTime.Date != date)
            throw new SkippedTimeException(date + LocalTime.Midnight, this);
          return new ZonedDateTime(localDateTime, intervalAfterGap.WallOffset, this);
        case 1:
        case 2:
          return new ZonedDateTime(date.AtMidnight(), zoneIntervalPair.EarlyInterval.WallOffset, this);
        default:
          throw new InvalidOperationException("This won't happen.");
      }
    }

    public ZoneLocalMapping MapLocal(LocalDateTime localDateTime)
    {
      LocalInstant localInstant = localDateTime.LocalInstant;
      ZoneInterval zoneInterval = this.GetZoneInterval(new Instant(localInstant.Ticks));
      if (zoneInterval.Contains(localInstant))
      {
        ZoneInterval matchingInterval1 = this.GetEarlierMatchingInterval(zoneInterval, localInstant);
        if (matchingInterval1 != null)
          return new ZoneLocalMapping(this, localDateTime, matchingInterval1, zoneInterval, 2);
        ZoneInterval matchingInterval2 = this.GetLaterMatchingInterval(zoneInterval, localInstant);
        return matchingInterval2 != null ? new ZoneLocalMapping(this, localDateTime, zoneInterval, matchingInterval2, 2) : new ZoneLocalMapping(this, localDateTime, zoneInterval, zoneInterval, 1);
      }
      ZoneInterval matchingInterval3 = this.GetEarlierMatchingInterval(zoneInterval, localInstant);
      if (matchingInterval3 != null)
        return new ZoneLocalMapping(this, localDateTime, matchingInterval3, matchingInterval3, 1);
      ZoneInterval matchingInterval4 = this.GetLaterMatchingInterval(zoneInterval, localInstant);
      return matchingInterval4 != null ? new ZoneLocalMapping(this, localDateTime, matchingInterval4, matchingInterval4, 1) : new ZoneLocalMapping(this, localDateTime, this.GetIntervalBeforeGap(localInstant), this.GetIntervalAfterGap(localInstant), 0);
    }

    public ZonedDateTime ResolveLocal(
      LocalDateTime localDateTime,
      [NotNull] ZoneLocalMappingResolver resolver)
    {
      Preconditions.CheckNotNull<ZoneLocalMappingResolver>(resolver, nameof (resolver));
      return resolver(this.MapLocal(localDateTime));
    }

    public ZonedDateTime AtStrictly(LocalDateTime localDateTime) => this.MapLocal(localDateTime).Single();

    public ZonedDateTime AtLeniently(LocalDateTime localDateTime) => this.ResolveLocal(localDateTime, Resolvers.LenientResolver);

    private ZoneInterval GetEarlierMatchingInterval(
      ZoneInterval interval,
      LocalInstant localInstant)
    {
      Instant start = interval.Start;
      if (start == Instant.MinValue)
        return (ZoneInterval) null;
      Instant instant = start;
      if (checked (instant.Ticks + this.maxOffsetTicks) > localInstant.Ticks)
      {
        ZoneInterval zoneInterval = this.GetZoneInterval(instant - Duration.Epsilon);
        if (zoneInterval.Contains(localInstant))
          return zoneInterval;
      }
      return (ZoneInterval) null;
    }

    private ZoneInterval GetLaterMatchingInterval(
      ZoneInterval interval,
      LocalInstant localInstant)
    {
      Instant end = interval.End;
      if (end == Instant.MaxValue)
        return (ZoneInterval) null;
      if (checked (end.Ticks + this.minOffsetTicks) <= localInstant.Ticks)
      {
        ZoneInterval zoneInterval = this.GetZoneInterval(end);
        if (zoneInterval.Contains(localInstant))
          return zoneInterval;
      }
      return (ZoneInterval) null;
    }

    private ZoneInterval GetIntervalBeforeGap(LocalInstant localInstant)
    {
      ZoneInterval zoneInterval = this.GetZoneInterval(new Instant(localInstant.Ticks));
      return localInstant.Minus(zoneInterval.WallOffset) < zoneInterval.Start ? this.GetZoneInterval(zoneInterval.Start - Duration.Epsilon) : zoneInterval;
    }

    private ZoneInterval GetIntervalAfterGap(LocalInstant localInstant)
    {
      ZoneInterval zoneInterval = this.GetZoneInterval(new Instant(localInstant.Ticks));
      return localInstant.Minus(zoneInterval.WallOffset) < zoneInterval.Start ? zoneInterval : this.GetZoneInterval(zoneInterval.End);
    }

    public override string ToString() => this.Id;

    [NotNull]
    private static DateTimeZone[] BuildFixedZoneCache()
    {
      DateTimeZone[] dateTimeZoneArray = new DateTimeZone[55];
      int index = 0;
      while (index < 55)
      {
        int milliseconds = checked (index * 1800000 - 43200000);
        dateTimeZoneArray[index] = (DateTimeZone) new FixedDateTimeZone(Offset.FromMilliseconds(milliseconds));
        checked { ++index; }
      }
      dateTimeZoneArray[24] = DateTimeZone.Utc;
      return dateTimeZoneArray;
    }

    public override sealed bool Equals(object obj) => this.Equals(obj as DateTimeZone);

    public bool Equals(DateTimeZone obj)
    {
      if (object.ReferenceEquals((object) this, (object) obj))
        return true;
      return !object.ReferenceEquals((object) obj, (object) null) && (object) obj.GetType() == (object) this.GetType() && this.EqualsImpl(obj);
    }

    protected abstract bool EqualsImpl(DateTimeZone zone);

    public abstract override int GetHashCode();

    public IEnumerable<ZoneInterval> GetZoneIntervals(
      Instant start,
      Instant end)
    {
      return this.GetZoneIntervals(new Interval(start, end));
    }

    public IEnumerable<ZoneInterval> GetZoneIntervals(Interval interval)
    {
      ZoneInterval zoneInterval;
      for (Instant current = interval.Start; current < interval.End; current = zoneInterval.End)
      {
        zoneInterval = this.GetZoneInterval(current);
        yield return zoneInterval;
      }
    }
  }
}
