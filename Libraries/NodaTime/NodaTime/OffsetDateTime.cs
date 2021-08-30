// Decompiled with JetBrains decompiler
// Type: NodaTime.OffsetDateTime
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using JetBrains.Annotations;
using NodaTime.Calendars;
using NodaTime.Text;
using NodaTime.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NodaTime
{
  public struct OffsetDateTime : IEquatable<OffsetDateTime>, IFormattable, IXmlSerializable
  {
    private readonly LocalDateTime localDateTime;
    private readonly Offset offset;

    public OffsetDateTime(LocalDateTime localDateTime, Offset offset)
    {
      this.localDateTime = localDateTime;
      this.offset = offset;
    }

    public CalendarSystem Calendar => this.localDateTime.Calendar;

    public int Year => this.localDateTime.Year;

    public int Month => this.localDateTime.Month;

    public int Day => this.localDateTime.Day;

    public IsoDayOfWeek IsoDayOfWeek => this.localDateTime.IsoDayOfWeek;

    public int DayOfWeek => this.localDateTime.DayOfWeek;

    public int WeekYear => this.localDateTime.WeekYear;

    public int WeekOfWeekYear => this.localDateTime.WeekOfWeekYear;

    public int YearOfCentury => this.localDateTime.YearOfCentury;

    public int YearOfEra => this.localDateTime.YearOfEra;

    public Era Era => this.localDateTime.Era;

    public int DayOfYear => this.localDateTime.DayOfYear;

    public int Hour => this.localDateTime.Hour;

    public int ClockHourOfHalfDay => this.localDateTime.ClockHourOfHalfDay;

    public int Minute => this.localDateTime.Minute;

    public int Second => this.localDateTime.Second;

    public int Millisecond => this.localDateTime.Millisecond;

    public int TickOfSecond => this.localDateTime.TickOfSecond;

    public long TickOfDay => this.localDateTime.TickOfDay;

    public LocalDateTime LocalDateTime => this.localDateTime;

    public LocalDate Date => this.localDateTime.Date;

    public LocalTime TimeOfDay => this.localDateTime.TimeOfDay;

    public Offset Offset => this.offset;

    [Pure]
    public Instant ToInstant() => this.localDateTime.LocalInstant.Minus(this.offset);

    [Pure]
    public ZonedDateTime InFixedZone() => new ZonedDateTime(this.localDateTime, this.offset, DateTimeZone.ForOffset(this.offset));

    [Pure]
    public DateTimeOffset ToDateTimeOffset() => new DateTimeOffset(this.localDateTime.ToDateTimeUnspecified(), this.offset.ToTimeSpan());

    [Pure]
    public static OffsetDateTime FromDateTimeOffset(DateTimeOffset dateTimeOffset) => new OffsetDateTime(LocalDateTime.FromDateTime(dateTimeOffset.DateTime), Offset.FromTimeSpan(dateTimeOffset.Offset));

    [Pure]
    public OffsetDateTime WithCalendar([NotNull] CalendarSystem calendarSystem) => new OffsetDateTime(this.localDateTime.WithCalendar(calendarSystem), this.offset);

    [Pure]
    public OffsetDateTime WithOffset(Offset offset) => new OffsetDateTime(new LocalDateTime(this.LocalDateTime.LocalInstant.Minus(this.Offset).Plus(offset), this.Calendar), offset);

    public override int GetHashCode() => HashCodeHelper.Hash<Offset>(HashCodeHelper.Hash<LocalDateTime>(HashCodeHelper.Initialize(), this.localDateTime), this.offset);

    public override bool Equals(object obj) => obj is OffsetDateTime offsetDateTime && this == offsetDateTime;

    public bool Equals(OffsetDateTime other) => this.localDateTime == other.localDateTime && this.offset == other.offset;

    public override string ToString() => OffsetDateTimePattern.Patterns.BclSupport.Format(this, (string) null, (IFormatProvider) CultureInfo.CurrentCulture);

    public string ToString(string patternText, IFormatProvider formatProvider) => OffsetDateTimePattern.Patterns.BclSupport.Format(this, patternText, formatProvider);

    public static bool operator ==(OffsetDateTime left, OffsetDateTime right) => left.Equals(right);

    public static bool operator !=(OffsetDateTime left, OffsetDateTime right) => !(left == right);

    XmlSchema IXmlSerializable.GetSchema() => (XmlSchema) null;

    void IXmlSerializable.ReadXml(XmlReader reader)
    {
      Preconditions.CheckNotNull<XmlReader>(reader, nameof (reader));
      OffsetDateTimePattern offsetDateTimePattern = OffsetDateTimePattern.Rfc3339Pattern;
      if (reader.MoveToAttribute("calendar"))
      {
        CalendarSystem calendarSystem = CalendarSystem.ForId(reader.Value);
        OffsetDateTime newTemplateValue = offsetDateTimePattern.TemplateValue.WithCalendar(calendarSystem);
        offsetDateTimePattern = offsetDateTimePattern.WithTemplateValue(newTemplateValue);
        reader.MoveToElement();
      }
      string text = reader.ReadElementContentAsString();
      this = offsetDateTimePattern.Parse(text).Value;
    }

    void IXmlSerializable.WriteXml(XmlWriter writer)
    {
      Preconditions.CheckNotNull<XmlWriter>(writer, nameof (writer));
      if (this.Calendar != CalendarSystem.Iso)
        writer.WriteAttributeString("calendar", this.Calendar.Id);
      writer.WriteString(OffsetDateTimePattern.Rfc3339Pattern.Format(this));
    }

    public abstract class Comparer : IComparer<OffsetDateTime>
    {
      public static OffsetDateTime.Comparer Local => OffsetDateTime.LocalComparer.Instance;

      public static OffsetDateTime.Comparer Instant => OffsetDateTime.InstantComparer.Instance;

      internal Comparer()
      {
      }

      public abstract int Compare(OffsetDateTime x, OffsetDateTime y);
    }

    private sealed class LocalComparer : OffsetDateTime.Comparer
    {
      internal static readonly OffsetDateTime.Comparer Instance = (OffsetDateTime.Comparer) new OffsetDateTime.LocalComparer();

      private LocalComparer()
      {
      }

      public override int Compare(OffsetDateTime x, OffsetDateTime y) => x.localDateTime.LocalInstant.CompareTo(y.localDateTime.LocalInstant);
    }

    private sealed class InstantComparer : OffsetDateTime.Comparer
    {
      internal static readonly OffsetDateTime.Comparer Instance = (OffsetDateTime.Comparer) new OffsetDateTime.InstantComparer();

      private InstantComparer()
      {
      }

      public override int Compare(OffsetDateTime x, OffsetDateTime y) => x.ToInstant().CompareTo(y.ToInstant());
    }
  }
}
