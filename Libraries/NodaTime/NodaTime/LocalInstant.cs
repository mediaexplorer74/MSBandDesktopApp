// Decompiled with JetBrains decompiler
// Type: NodaTime.LocalInstant
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using JetBrains.Annotations;
using NodaTime.Text;
using NodaTime.Utility;
using System;

namespace NodaTime
{
  internal struct LocalInstant : IEquatable<LocalInstant>, IComparable<LocalInstant>, IComparable
  {
    public static readonly LocalInstant LocalUnixEpoch = new LocalInstant(0L);
    public static readonly LocalInstant MinValue = new LocalInstant(long.MinValue);
    public static readonly LocalInstant MaxValue = new LocalInstant(long.MaxValue);
    private readonly long ticks;

    internal LocalInstant(long ticks) => this.ticks = ticks;

    internal LocalInstant(int year, int month, int day, int hour, int minute)
      : this(Instant.FromUtc(year, month, day, hour, minute).Ticks)
    {
    }

    internal long Ticks => this.ticks;

    [Pure]
    public DateTime ToDateTimeUnspecified() => new DateTime(checked (this.ticks - NodaConstants.BclEpoch.Ticks), DateTimeKind.Unspecified);

    internal static LocalInstant FromDateTime(DateTime dateTime) => new LocalInstant(checked (NodaConstants.BclEpoch.Ticks + dateTime.Ticks));

    public static LocalInstant operator +(LocalInstant left, Duration right) => new LocalInstant(checked (left.Ticks + right.Ticks));

    public static LocalInstant Add(LocalInstant left, Duration right) => left + right;

    public static Duration operator -(LocalInstant left, LocalInstant right) => new Duration(checked (left.Ticks - right.Ticks));

    public Instant Minus(Offset offset) => new Instant(checked (this.Ticks - offset.Ticks));

    public static LocalInstant operator -(LocalInstant left, Duration right) => new LocalInstant(checked (left.Ticks - right.Ticks));

    public static Duration Subtract(LocalInstant left, LocalInstant right) => left - right;

    public static LocalInstant Subtract(LocalInstant left, Duration right) => left - right;

    public static bool operator ==(LocalInstant left, LocalInstant right) => left.Equals(right);

    public static bool operator !=(LocalInstant left, LocalInstant right) => !(left == right);

    public static bool operator <(LocalInstant left, LocalInstant right) => left.CompareTo(right) < 0;

    public static bool operator <=(LocalInstant left, LocalInstant right) => left.CompareTo(right) <= 0;

    public static bool operator >(LocalInstant left, LocalInstant right) => left.CompareTo(right) > 0;

    public static bool operator >=(LocalInstant left, LocalInstant right) => left.CompareTo(right) >= 0;

    [Pure]
    internal LocalInstant PlusTicks(long ticksToAdd) => new LocalInstant(checked (this.Ticks + ticksToAdd));

    public int CompareTo(LocalInstant other) => this.Ticks.CompareTo(other.Ticks);

    int IComparable.CompareTo(object obj)
    {
      if (obj == null)
        return 1;
      Preconditions.CheckArgument(obj is LocalInstant, nameof (obj), "Object must be of type NodaTime.LocalInstant.");
      return this.CompareTo((LocalInstant) obj);
    }

    public override bool Equals(object obj) => obj is LocalInstant other && this.Equals(other);

    public override int GetHashCode() => this.Ticks.GetHashCode();

    public override string ToString() => LocalDateTimePattern.CreateWithInvariantCulture("yyyy-MM-ddTHH:mm:ss LOC").Format(new LocalDateTime(new LocalInstant(this.Ticks)));

    public bool Equals(LocalInstant other) => this.Ticks == other.Ticks;
  }
}
