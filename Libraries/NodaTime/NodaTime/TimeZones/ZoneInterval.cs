// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.ZoneInterval
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using JetBrains.Annotations;
using NodaTime.Annotations;
using NodaTime.Utility;
using System;
using System.Diagnostics;

namespace NodaTime.TimeZones
{
  [Immutable]
  public sealed class ZoneInterval : IEquatable<ZoneInterval>
  {
    private readonly Instant end;
    private readonly LocalInstant localEnd;
    private readonly LocalInstant localStart;
    private readonly string name;
    private readonly Offset wallOffset;
    private readonly Offset savings;
    private readonly Instant start;

    public ZoneInterval(
      [NotNull] string name,
      Instant start,
      Instant end,
      Offset wallOffset,
      Offset savings)
    {
      Preconditions.CheckNotNull<string>(name, nameof (name));
      Preconditions.CheckArgument(start < end, nameof (start), "The start Instant must be less than the end Instant");
      this.name = name;
      this.start = start;
      this.end = end;
      this.wallOffset = wallOffset;
      this.savings = savings;
      this.localStart = start == Instant.MinValue ? LocalInstant.MinValue : this.start.Plus(this.wallOffset);
      this.localEnd = end == Instant.MaxValue ? LocalInstant.MaxValue : this.end.Plus(this.wallOffset);
    }

    internal ZoneInterval WithStart(Instant newStart) => new ZoneInterval(this.name, newStart, this.end, this.wallOffset, this.savings);

    internal ZoneInterval WithEnd(Instant newEnd) => new ZoneInterval(this.name, this.start, newEnd, this.wallOffset, this.savings);

    public Offset StandardOffset
    {
      [DebuggerStepThrough] get => this.WallOffset - this.Savings;
    }

    public Duration Duration
    {
      [DebuggerStepThrough] get => this.End - this.Start;
    }

    public Instant End
    {
      [DebuggerStepThrough] get => this.end;
    }

    internal LocalInstant LocalEnd
    {
      [DebuggerStepThrough] get => this.localEnd;
    }

    internal LocalInstant LocalStart
    {
      [DebuggerStepThrough] get => this.localStart;
    }

    public LocalDateTime IsoLocalStart
    {
      [DebuggerStepThrough] get => new LocalDateTime(this.localStart);
    }

    public LocalDateTime IsoLocalEnd
    {
      [DebuggerStepThrough] get => new LocalDateTime(this.localEnd);
    }

    public string Name
    {
      [DebuggerStepThrough] get => this.name;
    }

    public Offset WallOffset
    {
      [DebuggerStepThrough] get => this.wallOffset;
    }

    public Offset Savings
    {
      [DebuggerStepThrough] get => this.savings;
    }

    public Instant Start
    {
      [DebuggerStepThrough] get => this.start;
    }

    [DebuggerStepThrough]
    public bool Contains(Instant instant)
    {
      if (!(this.Start <= instant))
        return false;
      return instant < this.End || this.End == Instant.MaxValue;
    }

    [DebuggerStepThrough]
    internal bool Contains(LocalInstant localInstant)
    {
      if (!(this.LocalStart <= localInstant))
        return false;
      return localInstant < this.LocalEnd || this.End == Instant.MaxValue;
    }

    internal bool EqualIgnoreBounds([NotNull] ZoneInterval other) => other.WallOffset == this.WallOffset && other.Savings == this.Savings && other.Name == this.Name;

    [DebuggerStepThrough]
    public bool Equals(ZoneInterval other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return this.Name == other.Name && this.Start == other.Start && this.End == other.End && this.WallOffset == other.WallOffset && this.Savings == other.Savings;
    }

    [DebuggerStepThrough]
    public override bool Equals(object obj) => this.Equals(obj as ZoneInterval);

    public override int GetHashCode() => HashCodeHelper.Hash<Offset>(HashCodeHelper.Hash<Offset>(HashCodeHelper.Hash<Instant>(HashCodeHelper.Hash<Instant>(HashCodeHelper.Hash<string>(HashCodeHelper.Initialize(), this.Name), this.Start), this.End), this.WallOffset), this.Savings);

    public override string ToString() => string.Format("{0}: [{1}, {2}) {3} ({4})", (object) this.Name, (object) this.Start, (object) this.End, (object) this.WallOffset, (object) this.Savings);
  }
}
