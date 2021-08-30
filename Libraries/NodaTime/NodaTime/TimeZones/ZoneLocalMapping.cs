// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.ZoneLocalMapping
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Annotations;
using NodaTime.Utility;
using System;

namespace NodaTime.TimeZones
{
  [Immutable]
  public sealed class ZoneLocalMapping
  {
    private readonly DateTimeZone zone;
    private readonly LocalDateTime localDateTime;
    private readonly ZoneInterval earlyInterval;
    private readonly ZoneInterval lateInterval;
    private readonly int count;

    internal ZoneLocalMapping(
      DateTimeZone zone,
      LocalDateTime localDateTime,
      ZoneInterval earlyInterval,
      ZoneInterval lateInterval,
      int count)
    {
      this.zone = Preconditions.CheckNotNull<DateTimeZone>(zone, nameof (zone));
      this.localDateTime = localDateTime;
      this.earlyInterval = Preconditions.CheckNotNull<ZoneInterval>(earlyInterval, nameof (earlyInterval));
      this.lateInterval = Preconditions.CheckNotNull<ZoneInterval>(lateInterval, nameof (lateInterval));
      Preconditions.CheckArgumentRange(nameof (count), count, 0, 2);
      this.count = count;
    }

    public int Count => this.count;

    public DateTimeZone Zone => this.zone;

    public LocalDateTime LocalDateTime => this.localDateTime;

    public ZoneInterval EarlyInterval => this.earlyInterval;

    public ZoneInterval LateInterval => this.lateInterval;

    public ZonedDateTime Single()
    {
      switch (this.count)
      {
        case 0:
          throw new SkippedTimeException(this.localDateTime, this.zone);
        case 1:
          return this.BuildZonedDateTime(this.earlyInterval);
        case 2:
          throw new AmbiguousTimeException(this.BuildZonedDateTime(this.earlyInterval), this.BuildZonedDateTime(this.lateInterval));
        default:
          throw new InvalidOperationException("Can't happen");
      }
    }

    public ZonedDateTime First()
    {
      switch (this.count)
      {
        case 0:
          throw new SkippedTimeException(this.localDateTime, this.zone);
        case 1:
        case 2:
          return this.BuildZonedDateTime(this.earlyInterval);
        default:
          throw new InvalidOperationException("Can't happen");
      }
    }

    public ZonedDateTime Last()
    {
      switch (this.count)
      {
        case 0:
          throw new SkippedTimeException(this.localDateTime, this.zone);
        case 1:
          return this.BuildZonedDateTime(this.earlyInterval);
        case 2:
          return this.BuildZonedDateTime(this.lateInterval);
        default:
          throw new InvalidOperationException("Can't happen");
      }
    }

    private ZonedDateTime BuildZonedDateTime(ZoneInterval interval) => new ZonedDateTime(this.localDateTime, interval.WallOffset, this.zone);
  }
}
