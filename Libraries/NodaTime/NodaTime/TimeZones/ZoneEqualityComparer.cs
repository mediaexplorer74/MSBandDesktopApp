// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.ZoneEqualityComparer
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Annotations;
using NodaTime.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NodaTime.TimeZones
{
  [Immutable]
  public sealed class ZoneEqualityComparer : IEqualityComparer<DateTimeZone>
  {
    private readonly Interval interval;
    private readonly ZoneEqualityComparer.Options options;
    private readonly ZoneEqualityComparer.ZoneIntervalEqualityComparer zoneIntervalComparer;

    private static bool CheckOption(
      ZoneEqualityComparer.Options options,
      ZoneEqualityComparer.Options candidate)
    {
      return (options & candidate) != ZoneEqualityComparer.Options.OnlyMatchWallOffset;
    }

    [VisibleForTesting]
    internal Interval IntervalForTest => this.interval;

    [VisibleForTesting]
    internal ZoneEqualityComparer.Options OptionsForTest => this.options;

    private ZoneEqualityComparer(Interval interval, ZoneEqualityComparer.Options options)
    {
      this.interval = interval;
      this.options = options;
      this.zoneIntervalComparer = (options & ~ZoneEqualityComparer.Options.StrictestMatch) == ZoneEqualityComparer.Options.OnlyMatchWallOffset ? new ZoneEqualityComparer.ZoneIntervalEqualityComparer(options, interval) : throw new ArgumentOutOfRangeException("The value " + (object) options + " is not defined within ZoneEqualityComparer.Options");
    }

    public static ZoneEqualityComparer ForInterval(Interval interval) => new ZoneEqualityComparer(interval, ZoneEqualityComparer.Options.OnlyMatchWallOffset);

    public ZoneEqualityComparer WithOptions(ZoneEqualityComparer.Options options) => this.options != options ? new ZoneEqualityComparer(this.interval, options) : this;

    public bool Equals(DateTimeZone x, DateTimeZone y)
    {
      if (object.ReferenceEquals((object) x, (object) y))
        return true;
      return !object.ReferenceEquals((object) x, (object) null) && !object.ReferenceEquals((object) y, (object) null) && this.GetIntervals(x).SequenceEqual<ZoneInterval>(this.GetIntervals(y), (IEqualityComparer<ZoneInterval>) this.zoneIntervalComparer);
    }

    public int GetHashCode(DateTimeZone obj)
    {
      Preconditions.CheckNotNull<DateTimeZone>(obj, nameof (obj));
      int num = 19;
      foreach (ZoneInterval interval in this.GetIntervals(obj))
        num = num * 31 + this.zoneIntervalComparer.GetHashCode(interval);
      return num;
    }

    private IEnumerable<ZoneInterval> GetIntervals(DateTimeZone zone)
    {
      IEnumerable<ZoneInterval> zoneIntervals = zone.GetZoneIntervals(this.interval.Start, this.interval.End);
      return !ZoneEqualityComparer.CheckOption(this.options, ZoneEqualityComparer.Options.MatchAllTransitions) ? this.CoalesceIntervals(zoneIntervals) : zoneIntervals;
    }

    private IEnumerable<ZoneInterval> CoalesceIntervals(
      IEnumerable<ZoneInterval> zoneIntervals)
    {
      ZoneInterval current = (ZoneInterval) null;
      foreach (ZoneInterval zoneInterval in zoneIntervals)
      {
        if (current == null)
          current = zoneInterval;
        else if (this.zoneIntervalComparer.EqualExceptStartAndEnd(current, zoneInterval))
        {
          current = current.WithEnd(zoneInterval.End);
        }
        else
        {
          yield return current;
          current = zoneInterval;
        }
      }
      if (current != null)
        yield return current;
    }

    [Flags]
    public enum Options
    {
      OnlyMatchWallOffset = 0,
      MatchOffsetComponents = 1,
      MatchNames = 2,
      MatchAllTransitions = 4,
      MatchStartAndEndTransitions = 8,
      StrictestMatch = MatchStartAndEndTransitions | MatchAllTransitions | MatchNames | MatchOffsetComponents, // 0x0000000F
    }

    private sealed class ZoneIntervalEqualityComparer : IEqualityComparer<ZoneInterval>
    {
      private readonly ZoneEqualityComparer.Options options;
      private readonly Interval interval;

      internal ZoneIntervalEqualityComparer(ZoneEqualityComparer.Options options, Interval interval)
      {
        this.options = options;
        this.interval = interval;
      }

      public bool Equals(ZoneInterval x, ZoneInterval y) => this.EqualExceptStartAndEnd(x, y) && this.GetEffectiveStart(x) == this.GetEffectiveStart(y) && this.GetEffectiveEnd(x) == this.GetEffectiveEnd(y);

      public int GetHashCode(ZoneInterval obj)
      {
        int code1 = HashCodeHelper.Initialize();
        int code2 = !ZoneEqualityComparer.CheckOption(this.options, ZoneEqualityComparer.Options.MatchOffsetComponents) ? HashCodeHelper.Hash<Offset>(code1, obj.WallOffset) : HashCodeHelper.Hash<Offset>(HashCodeHelper.Hash<Offset>(code1, obj.StandardOffset), obj.Savings);
        if (ZoneEqualityComparer.CheckOption(this.options, ZoneEqualityComparer.Options.MatchNames))
          code2 = HashCodeHelper.Hash<string>(code2, obj.Name);
        return HashCodeHelper.Hash<Instant>(HashCodeHelper.Hash<Instant>(code2, this.GetEffectiveStart(obj)), this.GetEffectiveEnd(obj));
      }

      private Instant GetEffectiveStart(ZoneInterval zoneInterval) => !ZoneEqualityComparer.CheckOption(this.options, ZoneEqualityComparer.Options.MatchStartAndEndTransitions) ? Instant.Max(zoneInterval.Start, this.interval.Start) : zoneInterval.Start;

      private Instant GetEffectiveEnd(ZoneInterval zoneInterval) => !ZoneEqualityComparer.CheckOption(this.options, ZoneEqualityComparer.Options.MatchStartAndEndTransitions) ? Instant.Min(zoneInterval.End, this.interval.End) : zoneInterval.End;

      internal bool EqualExceptStartAndEnd(ZoneInterval x, ZoneInterval y) => !(x.WallOffset != y.WallOffset) && (!ZoneEqualityComparer.CheckOption(this.options, ZoneEqualityComparer.Options.MatchOffsetComponents) || !(x.Savings != y.Savings)) && (!ZoneEqualityComparer.CheckOption(this.options, ZoneEqualityComparer.Options.MatchNames) || !(x.Name != y.Name));
    }
  }
}
