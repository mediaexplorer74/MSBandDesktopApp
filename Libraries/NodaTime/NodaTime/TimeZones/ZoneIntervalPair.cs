// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.ZoneIntervalPair
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NodaTime.TimeZones
{
  internal struct ZoneIntervalPair : IEquatable<ZoneIntervalPair>
  {
    internal static readonly ZoneIntervalPair NoMatch = new ZoneIntervalPair((ZoneInterval) null, (ZoneInterval) null, 0);
    private readonly ZoneInterval earlyInterval;
    private readonly ZoneInterval lateInterval;
    private readonly int matchingIntervals;

    internal ZoneInterval EarlyInterval => this.earlyInterval;

    internal ZoneInterval LateInterval => this.lateInterval;

    private ZoneIntervalPair(ZoneInterval early, ZoneInterval late, int matchingIntervals)
    {
      this.earlyInterval = early;
      this.lateInterval = late;
      this.matchingIntervals = matchingIntervals;
    }

    internal static ZoneIntervalPair Unambiguous(ZoneInterval interval) => new ZoneIntervalPair(interval, (ZoneInterval) null, 1);

    internal static ZoneIntervalPair Ambiguous(
      ZoneInterval early,
      ZoneInterval late)
    {
      return new ZoneIntervalPair(early, late, 2);
    }

    internal int MatchingIntervals => this.matchingIntervals;

    [DebuggerStepThrough]
    public override bool Equals(object obj) => obj is ZoneIntervalPair other && this.Equals(other);

    [DebuggerStepThrough]
    public bool Equals(ZoneIntervalPair other) => EqualityComparer<ZoneInterval>.Default.Equals(this.earlyInterval, other.earlyInterval) && EqualityComparer<ZoneInterval>.Default.Equals(this.lateInterval, other.lateInterval);

    public override int GetHashCode() => HashCodeHelper.Hash<ZoneInterval>(HashCodeHelper.Hash<ZoneInterval>(HashCodeHelper.Initialize(), this.earlyInterval), this.lateInterval);

    public override string ToString()
    {
      switch (this.MatchingIntervals)
      {
        case 0:
          return "No match (gap)";
        case 1:
          return "Unambiguous: " + (object) this.earlyInterval;
        case 2:
          return "Ambiguous between " + (object) this.earlyInterval + " and " + (object) this.lateInterval;
        default:
          throw new InvalidOperationException("Won't happen");
      }
    }
  }
}
