// Decompiled with JetBrains decompiler
// Type: NodaTime.Instant
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using JetBrains.Annotations;
using NodaTime.Text;
using NodaTime.Utility;
using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NodaTime
{
  public struct Instant : 
    IEquatable<Instant>,
    IComparable<Instant>,
    IFormattable,
    IComparable,
    IXmlSerializable
  {
    public static readonly Instant MinValue = new Instant(long.MinValue);
    public static readonly Instant MaxValue = new Instant(long.MaxValue);
    private readonly long ticks;

    public Instant(long ticks) => this.ticks = ticks;

    public long Ticks => this.ticks;

    public int CompareTo(Instant other) => this.Ticks.CompareTo(other.Ticks);

    int IComparable.CompareTo(object obj)
    {
      if (obj == null)
        return 1;
      Preconditions.CheckArgument(obj is Instant, nameof (obj), "Object must be of type NodaTime.Instant.");
      return this.CompareTo((Instant) obj);
    }

    public override bool Equals(object obj) => obj is Instant other && this.Equals(other);

    public override int GetHashCode() => this.Ticks.GetHashCode();

    [Pure]
    public Instant PlusTicks(long ticksToAdd) => new Instant(checked (this.ticks + ticksToAdd));

    public static Instant operator +(Instant left, Duration right) => new Instant(checked (left.Ticks + right.Ticks));

    [Pure]
    internal LocalInstant Plus(Offset offset) => new LocalInstant(checked (this.Ticks + offset.Ticks));

    public static Instant Add(Instant left, Duration right) => left + right;

    [Pure]
    public Instant Plus(Duration duration) => this + duration;

    public static Duration operator -(Instant left, Instant right) => new Duration(checked (left.Ticks - right.Ticks));

    public static Instant operator -(Instant left, Duration right) => new Instant(checked (left.Ticks - right.Ticks));

    public static Duration Subtract(Instant left, Instant right) => left - right;

    [Pure]
    public Duration Minus(Instant other) => this - other;

    [Pure]
    public static Instant Subtract(Instant left, Duration right) => left - right;

    [Pure]
    public Instant Minus(Duration duration) => this - duration;

    public static bool operator ==(Instant left, Instant right) => left.Equals(right);

    public static bool operator !=(Instant left, Instant right) => !(left == right);

    public static bool operator <(Instant left, Instant right) => left.Ticks < right.Ticks;

    public static bool operator <=(Instant left, Instant right) => left.Ticks <= right.Ticks;

    public static bool operator >(Instant left, Instant right) => left.Ticks > right.Ticks;

    public static bool operator >=(Instant left, Instant right) => left.Ticks >= right.Ticks;

    public static Instant FromUtc(
      int year,
      int monthOfYear,
      int dayOfMonth,
      int hourOfDay,
      int minuteOfHour)
    {
      return new Instant(CalendarSystem.Iso.GetLocalInstant(year, monthOfYear, dayOfMonth, hourOfDay, minuteOfHour).Ticks);
    }

    public static Instant FromUtc(
      int year,
      int monthOfYear,
      int dayOfMonth,
      int hourOfDay,
      int minuteOfHour,
      int secondOfMinute)
    {
      return new Instant(CalendarSystem.Iso.GetLocalInstant(year, monthOfYear, dayOfMonth, hourOfDay, minuteOfHour, secondOfMinute).Ticks);
    }

    public static Instant Max(Instant x, Instant y) => !(x > y) ? y : x;

    public static Instant Min(Instant x, Instant y) => !(x < y) ? y : x;

    public override string ToString() => InstantPattern.BclSupport.Format(this, (string) null, (IFormatProvider) CultureInfo.CurrentCulture);

    public string ToString(string patternText, IFormatProvider formatProvider) => InstantPattern.BclSupport.Format(this, patternText, formatProvider);

    public bool Equals(Instant other) => this.Ticks == other.Ticks;

    [Pure]
    public DateTime ToDateTimeUtc() => new DateTime((this - NodaConstants.BclEpoch).Ticks, DateTimeKind.Utc);

    [Pure]
    public DateTimeOffset ToDateTimeOffset() => new DateTimeOffset((this - NodaConstants.BclEpoch).Ticks, TimeSpan.Zero);

    public static Instant FromDateTimeOffset(DateTimeOffset dateTimeOffset) => NodaConstants.BclEpoch.PlusTicks(checked (dateTimeOffset.Ticks - dateTimeOffset.Offset.Ticks));

    public static Instant FromDateTimeUtc(DateTime dateTime)
    {
      Preconditions.CheckArgument(dateTime.Kind == DateTimeKind.Utc, nameof (dateTime), "Invalid DateTime.Kind for Instant.FromDateTimeUtc");
      return NodaConstants.BclEpoch.PlusTicks(dateTime.Ticks);
    }

    public static Instant FromSecondsSinceUnixEpoch(long seconds)
    {
      Preconditions.CheckArgumentRange(nameof (seconds), seconds, -922337203685L, 922337203685L);
      return new Instant(checked (seconds * 10000000L));
    }

    public static Instant FromMillisecondsSinceUnixEpoch(long milliseconds)
    {
      Preconditions.CheckArgumentRange(nameof (milliseconds), milliseconds, -922337203685477L, 922337203685477L);
      return new Instant(checked (milliseconds * 10000L));
    }

    public static Instant FromTicksSinceUnixEpoch(long ticks) => new Instant(ticks);

    [Pure]
    public ZonedDateTime InUtc() => new ZonedDateTime(this, DateTimeZone.Utc, CalendarSystem.Iso);

    [Pure]
    public ZonedDateTime InZone([NotNull] DateTimeZone zone)
    {
      Preconditions.CheckNotNull<DateTimeZone>(zone, nameof (zone));
      return new ZonedDateTime(this, zone, CalendarSystem.Iso);
    }

    [Pure]
    public ZonedDateTime InZone([NotNull] DateTimeZone zone, [NotNull] CalendarSystem calendar)
    {
      Preconditions.CheckNotNull<DateTimeZone>(zone, nameof (zone));
      Preconditions.CheckNotNull<CalendarSystem>(calendar, nameof (calendar));
      return new ZonedDateTime(this, zone, calendar);
    }

    [Pure]
    public OffsetDateTime WithOffset(Offset offset) => new OffsetDateTime(new LocalDateTime(this.Plus(offset)), offset);

    [Pure]
    public OffsetDateTime WithOffset(Offset offset, [NotNull] CalendarSystem calendar)
    {
      Preconditions.CheckNotNull<CalendarSystem>(calendar, nameof (calendar));
      return new OffsetDateTime(new LocalDateTime(this.Plus(offset), calendar), offset);
    }

    XmlSchema IXmlSerializable.GetSchema() => (XmlSchema) null;

    void IXmlSerializable.ReadXml(XmlReader reader)
    {
      Preconditions.CheckNotNull<XmlReader>(reader, nameof (reader));
      this = InstantPattern.ExtendedIsoPattern.Parse(reader.ReadElementContentAsString()).Value;
    }

    void IXmlSerializable.WriteXml(XmlWriter writer)
    {
      Preconditions.CheckNotNull<XmlWriter>(writer, nameof (writer));
      writer.WriteString(InstantPattern.ExtendedIsoPattern.Format(this));
    }
  }
}
