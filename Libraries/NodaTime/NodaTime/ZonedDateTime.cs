// Decompiled with JetBrains decompiler
// Type: NodaTime.ZonedDateTime
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using JetBrains.Annotations;
using NodaTime.Calendars;
using NodaTime.Text;
using NodaTime.TimeZones;
using NodaTime.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NodaTime
{
  public struct ZonedDateTime : 
    IEquatable<ZonedDateTime>,
    IComparable<ZonedDateTime>,
    IComparable,
    IFormattable,
    IXmlSerializable
  {
    private readonly LocalDateTime localDateTime;
    private readonly DateTimeZone zone;
    private readonly Offset offset;

    internal ZonedDateTime(LocalDateTime localDateTime, Offset offset, DateTimeZone zone)
    {
      this.localDateTime = localDateTime;
      this.offset = offset;
      this.zone = zone;
    }

    public ZonedDateTime(Instant instant, [NotNull] DateTimeZone zone, [NotNull] CalendarSystem calendar)
    {
      Preconditions.CheckNotNull<DateTimeZone>(zone, nameof (zone));
      Preconditions.CheckNotNull<CalendarSystem>(calendar, nameof (calendar));
      this.offset = zone.GetUtcOffset(instant);
      this.localDateTime = new LocalDateTime(instant.Plus(this.offset), calendar);
      this.zone = zone;
    }

    public ZonedDateTime(Instant instant, DateTimeZone zone)
      : this(instant, zone, CalendarSystem.Iso)
    {
    }

    public ZonedDateTime(LocalDateTime localDateTime, [NotNull] DateTimeZone zone, Offset offset)
    {
      Preconditions.CheckNotNull<DateTimeZone>(zone, nameof (zone));
      Instant instant = localDateTime.LocalInstant.Minus(offset);
      if (zone.GetUtcOffset(instant) != offset)
        throw new ArgumentException("Offset " + (object) offset + " is invalid for local date and time " + (object) localDateTime + " in time zone " + zone.Id, nameof (offset));
      this.localDateTime = localDateTime;
      this.offset = offset;
      this.zone = zone;
    }

    public Offset Offset => this.offset;

    public DateTimeZone Zone => this.zone ?? DateTimeZone.Utc;

    internal LocalInstant LocalInstant => this.localDateTime.LocalInstant;

    public LocalDateTime LocalDateTime => this.localDateTime;

    public CalendarSystem Calendar => this.localDateTime.Calendar;

    public LocalDate Date => this.localDateTime.Date;

    public LocalTime TimeOfDay => this.localDateTime.TimeOfDay;

    public Era Era => this.LocalDateTime.Era;

    public int CenturyOfEra => this.LocalDateTime.CenturyOfEra;

    public int Year => this.LocalDateTime.Year;

    public int YearOfCentury => this.LocalDateTime.YearOfCentury;

    public int YearOfEra => this.LocalDateTime.YearOfEra;

    public int WeekYear => this.LocalDateTime.WeekYear;

    public int Month => this.LocalDateTime.Month;

    public int WeekOfWeekYear => this.LocalDateTime.WeekOfWeekYear;

    public int DayOfYear => this.LocalDateTime.DayOfYear;

    public int Day => this.LocalDateTime.Day;

    public IsoDayOfWeek IsoDayOfWeek => this.LocalDateTime.IsoDayOfWeek;

    public int DayOfWeek => this.LocalDateTime.DayOfWeek;

    public int Hour => this.LocalDateTime.Hour;

    public int ClockHourOfHalfDay => this.LocalDateTime.ClockHourOfHalfDay;

    public int Minute => this.LocalDateTime.Minute;

    public int Second => this.LocalDateTime.Second;

    public int Millisecond => this.LocalDateTime.Millisecond;

    public int TickOfSecond => this.LocalDateTime.TickOfSecond;

    public long TickOfDay => this.LocalDateTime.TickOfDay;

    [Pure]
    public Instant ToInstant() => this.localDateTime.LocalInstant.Minus(this.offset);

    [Pure]
    public ZonedDateTime WithZone([NotNull] DateTimeZone targetZone)
    {
      Preconditions.CheckNotNull<DateTimeZone>(targetZone, nameof (targetZone));
      return new ZonedDateTime(this.ToInstant(), targetZone, this.localDateTime.Calendar);
    }

    public bool Equals(ZonedDateTime other) => this.LocalDateTime == other.LocalDateTime && this.Offset == other.Offset && this.Zone.Equals(other.Zone);

    public override bool Equals(object obj) => obj is ZonedDateTime other && this.Equals(other);

    public override int GetHashCode() => HashCodeHelper.Hash<DateTimeZone>(HashCodeHelper.Hash<Offset>(HashCodeHelper.Hash<LocalInstant>(HashCodeHelper.Initialize(), this.LocalInstant), this.Offset), this.Zone);

    public static bool operator ==(ZonedDateTime left, ZonedDateTime right) => left.Equals(right);

    public static bool operator !=(ZonedDateTime left, ZonedDateTime right) => !(left == right);

    public static bool operator <(ZonedDateTime lhs, ZonedDateTime rhs) => lhs.ToInstant() < rhs.ToInstant() && object.Equals((object) lhs.LocalDateTime.Calendar, (object) rhs.LocalDateTime.Calendar) && object.Equals((object) lhs.Zone, (object) rhs.Zone);

    public static bool operator <=(ZonedDateTime lhs, ZonedDateTime rhs) => lhs.ToInstant() <= rhs.ToInstant() && object.Equals((object) lhs.LocalDateTime.Calendar, (object) rhs.LocalDateTime.Calendar) && object.Equals((object) lhs.Zone, (object) rhs.Zone);

    public static bool operator >(ZonedDateTime lhs, ZonedDateTime rhs) => lhs.ToInstant() > rhs.ToInstant() && object.Equals((object) lhs.LocalDateTime.Calendar, (object) rhs.LocalDateTime.Calendar) && object.Equals((object) lhs.Zone, (object) rhs.Zone);

    public static bool operator >=(ZonedDateTime lhs, ZonedDateTime rhs) => lhs.ToInstant() >= rhs.ToInstant() && object.Equals((object) lhs.LocalDateTime.Calendar, (object) rhs.LocalDateTime.Calendar) && object.Equals((object) lhs.Zone, (object) rhs.Zone);

    public int CompareTo(ZonedDateTime other) => this.ToInstant().CompareTo(other.ToInstant());

    int IComparable.CompareTo(object obj)
    {
      if (obj == null)
        return 1;
      Preconditions.CheckArgument(obj is ZonedDateTime, nameof (obj), "Object must be of type NodaTime.ZonedDateTime.");
      return this.CompareTo((ZonedDateTime) obj);
    }

    public static ZonedDateTime operator +(
      ZonedDateTime zonedDateTime,
      Duration duration)
    {
      return new ZonedDateTime(zonedDateTime.ToInstant() + duration, zonedDateTime.Zone, zonedDateTime.LocalDateTime.Calendar);
    }

    public static ZonedDateTime Add(ZonedDateTime zonedDateTime, Duration duration) => zonedDateTime + duration;

    [Pure]
    public ZonedDateTime Plus(Duration duration) => this + duration;

    public static ZonedDateTime Subtract(
      ZonedDateTime zonedDateTime,
      Duration duration)
    {
      return zonedDateTime - duration;
    }

    [Pure]
    public ZonedDateTime Minus(Duration duration) => this - duration;

    public static ZonedDateTime operator -(
      ZonedDateTime zonedDateTime,
      Duration duration)
    {
      return new ZonedDateTime(zonedDateTime.ToInstant() - duration, zonedDateTime.Zone, zonedDateTime.LocalDateTime.Calendar);
    }

    [Pure]
    public ZoneInterval GetZoneInterval() => this.Zone.GetZoneInterval(this.ToInstant());

    [Pure]
    public bool IsDaylightSavingTime() => this.GetZoneInterval().Savings != Offset.Zero;

    public override string ToString() => ZonedDateTimePattern.Patterns.BclSupport.Format(this, (string) null, (IFormatProvider) CultureInfo.CurrentCulture);

    public string ToString(string patternText, IFormatProvider formatProvider) => ZonedDateTimePattern.Patterns.BclSupport.Format(this, patternText, formatProvider);

    [Pure]
    public DateTimeOffset ToDateTimeOffset() => new DateTimeOffset(checked (this.LocalInstant.Ticks - NodaConstants.BclEpoch.Ticks), this.Offset.ToTimeSpan());

    public static ZonedDateTime FromDateTimeOffset(DateTimeOffset dateTimeOffset) => new ZonedDateTime(Instant.FromDateTimeOffset(dateTimeOffset), (DateTimeZone) new FixedDateTimeZone(Offset.FromTimeSpan(dateTimeOffset.Offset)));

    [Pure]
    public DateTime ToDateTimeUtc() => this.ToInstant().ToDateTimeUtc();

    [Pure]
    public DateTime ToDateTimeUnspecified() => this.LocalInstant.ToDateTimeUnspecified();

    [Pure]
    public OffsetDateTime ToOffsetDateTime() => new OffsetDateTime(this.localDateTime, this.offset);

    XmlSchema IXmlSerializable.GetSchema() => (XmlSchema) null;

    void IXmlSerializable.ReadXml(XmlReader reader)
    {
      Preconditions.CheckNotNull<XmlReader>(reader, nameof (reader));
      OffsetDateTimePattern offsetDateTimePattern = OffsetDateTimePattern.ExtendedIsoPattern;
      if (!reader.MoveToAttribute("zone"))
        throw new ArgumentException("No zone specified in XML for ZonedDateTime");
      DateTimeZone zone = DateTimeZoneProviders.Serialization[reader.Value];
      if (reader.MoveToAttribute("calendar"))
      {
        CalendarSystem calendarSystem = CalendarSystem.ForId(reader.Value);
        OffsetDateTime newTemplateValue = offsetDateTimePattern.TemplateValue.WithCalendar(calendarSystem);
        offsetDateTimePattern = offsetDateTimePattern.WithTemplateValue(newTemplateValue);
      }
      reader.MoveToElement();
      string text = reader.ReadElementContentAsString();
      OffsetDateTime offsetDateTime = offsetDateTimePattern.Parse(text).Value;
      if (zone.GetUtcOffset(offsetDateTime.ToInstant()) != offsetDateTime.Offset)
        ParseResult<ZonedDateTime>.InvalidOffset(text).GetValueOrThrow();
      this = new ZonedDateTime(offsetDateTime.LocalDateTime, offsetDateTime.Offset, zone);
    }

    void IXmlSerializable.WriteXml(XmlWriter writer)
    {
      Preconditions.CheckNotNull<XmlWriter>(writer, nameof (writer));
      writer.WriteAttributeString("zone", this.Zone.Id);
      if (this.Calendar != CalendarSystem.Iso)
        writer.WriteAttributeString("calendar", this.Calendar.Id);
      writer.WriteString(OffsetDateTimePattern.ExtendedIsoPattern.Format(this.ToOffsetDateTime()));
    }

    public abstract class Comparer : IComparer<ZonedDateTime>
    {
      public static ZonedDateTime.Comparer Local => ZonedDateTime.LocalComparer.Instance;

      public static ZonedDateTime.Comparer Instant => ZonedDateTime.InstantComparer.Instance;

      internal Comparer()
      {
      }

      public abstract int Compare(ZonedDateTime x, ZonedDateTime y);
    }

    private sealed class LocalComparer : ZonedDateTime.Comparer
    {
      internal static readonly ZonedDateTime.Comparer Instance = (ZonedDateTime.Comparer) new ZonedDateTime.LocalComparer();

      private LocalComparer()
      {
      }

      public override int Compare(ZonedDateTime x, ZonedDateTime y) => x.localDateTime.LocalInstant.CompareTo(y.localDateTime.LocalInstant);
    }

    private sealed class InstantComparer : ZonedDateTime.Comparer
    {
      internal static readonly ZonedDateTime.Comparer Instance = (ZonedDateTime.Comparer) new ZonedDateTime.InstantComparer();

      private InstantComparer()
      {
      }

      public override int Compare(ZonedDateTime x, ZonedDateTime y) => x.ToInstant().CompareTo(y.ToInstant());
    }
  }
}
