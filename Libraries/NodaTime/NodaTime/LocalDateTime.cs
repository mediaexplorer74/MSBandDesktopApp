// Decompiled with JetBrains decompiler
// Type: NodaTime.LocalDateTime
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using JetBrains.Annotations;
using NodaTime.Calendars;
using NodaTime.Text;
using NodaTime.TimeZones;
using NodaTime.Utility;
using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NodaTime
{
  public struct LocalDateTime : 
    IEquatable<LocalDateTime>,
    IComparable<LocalDateTime>,
    IComparable,
    IFormattable,
    IXmlSerializable
  {
    private readonly CalendarSystem calendar;
    private readonly LocalInstant localInstant;

    internal LocalDateTime(LocalInstant localInstant)
      : this(localInstant, CalendarSystem.Iso)
    {
    }

    internal LocalDateTime(LocalInstant localInstant, [NotNull] CalendarSystem calendar)
    {
      Preconditions.CheckNotNull<CalendarSystem>(calendar, nameof (calendar));
      this.localInstant = localInstant;
      this.calendar = calendar;
    }

    public LocalDateTime(int year, int month, int day, int hour, int minute)
      : this(year, month, day, hour, minute, CalendarSystem.Iso)
    {
    }

    public LocalDateTime(
      int year,
      int month,
      int day,
      int hour,
      int minute,
      [NotNull] CalendarSystem calendar)
    {
      Preconditions.CheckNotNull<CalendarSystem>(calendar, nameof (calendar));
      this.localInstant = calendar.GetLocalInstant(year, month, day, hour, minute);
      this.calendar = calendar;
    }

    public LocalDateTime(int year, int month, int day, int hour, int minute, int second)
      : this(year, month, day, hour, minute, second, CalendarSystem.Iso)
    {
    }

    public LocalDateTime(
      int year,
      int month,
      int day,
      int hour,
      int minute,
      int second,
      [NotNull] CalendarSystem calendar)
    {
      Preconditions.CheckNotNull<CalendarSystem>(calendar, nameof (calendar));
      this.localInstant = calendar.GetLocalInstant(year, month, day, hour, minute, second);
      this.calendar = calendar;
    }

    public LocalDateTime(
      int year,
      int month,
      int day,
      int hour,
      int minute,
      int second,
      int millisecond)
      : this(year, month, day, hour, minute, second, millisecond, 0, CalendarSystem.Iso)
    {
    }

    public LocalDateTime(
      int year,
      int month,
      int day,
      int hour,
      int minute,
      int second,
      int millisecond,
      [NotNull] CalendarSystem calendar)
      : this(year, month, day, hour, minute, second, millisecond, 0, calendar)
    {
    }

    public LocalDateTime(
      int year,
      int month,
      int day,
      int hour,
      int minute,
      int second,
      int millisecond,
      int tickWithinMillisecond)
      : this(year, month, day, hour, minute, second, millisecond, tickWithinMillisecond, CalendarSystem.Iso)
    {
    }

    public LocalDateTime(
      int year,
      int month,
      int day,
      int hour,
      int minute,
      int second,
      int millisecond,
      int tickWithinMillisecond,
      [NotNull] CalendarSystem calendar)
    {
      Preconditions.CheckNotNull<CalendarSystem>(calendar, nameof (calendar));
      this.localInstant = calendar.GetLocalInstant(year, month, day, hour, minute, second, millisecond, tickWithinMillisecond);
      this.calendar = calendar;
    }

    internal LocalInstant LocalInstant => this.localInstant;

    public CalendarSystem Calendar => this.calendar ?? CalendarSystem.Iso;

    public int CenturyOfEra => this.Calendar.GetCenturyOfEra(this.localInstant);

    public int Year => this.Calendar.GetYear(this.localInstant);

    public int YearOfCentury => this.Calendar.GetYearOfCentury(this.localInstant);

    public int YearOfEra => this.Calendar.GetYearOfEra(this.localInstant);

    public Era Era => this.Calendar.Eras[this.Calendar.GetEra(this.localInstant)];

    public int WeekYear => this.Calendar.GetWeekYear(this.localInstant);

    public int Month => this.Calendar.GetMonthOfYear(this.localInstant);

    public int WeekOfWeekYear => this.Calendar.GetWeekOfWeekYear(this.localInstant);

    public int DayOfYear => this.Calendar.GetDayOfYear(this.localInstant);

    public int Day => this.Calendar.GetDayOfMonth(this.localInstant);

    public IsoDayOfWeek IsoDayOfWeek => this.Calendar.GetIsoDayOfWeek(this.localInstant);

    public int DayOfWeek => this.Calendar.GetDayOfWeek(this.localInstant);

    public int Hour => this.Calendar.GetHourOfDay(this.localInstant);

    public int ClockHourOfHalfDay => this.Calendar.GetClockHourOfHalfDay(this.localInstant);

    public int Minute => this.Calendar.GetMinuteOfHour(this.localInstant);

    public int Second => this.Calendar.GetSecondOfMinute(this.localInstant);

    public int Millisecond => this.Calendar.GetMillisecondOfSecond(this.localInstant);

    public int TickOfSecond => this.Calendar.GetTickOfSecond(this.localInstant);

    public long TickOfDay => this.Calendar.GetTickOfDay(this.localInstant);

    public LocalTime TimeOfDay => new LocalTime(this.TickOfDay);

    public LocalDate Date
    {
      get
      {
        long num = this.localInstant.Ticks % 864000000000L;
        if (num < 0L)
          checked { num += 864000000000L; }
        return new LocalDate(new LocalDateTime(new LocalInstant(checked (this.localInstant.Ticks - num)), this.Calendar));
      }
    }

    [Pure]
    public DateTime ToDateTimeUnspecified() => this.localInstant.ToDateTimeUnspecified();

    public static LocalDateTime FromDateTime(DateTime dateTime) => new LocalDateTime(LocalInstant.FromDateTime(dateTime), CalendarSystem.Iso);

    public bool Equals(LocalDateTime other) => this.localInstant == other.localInstant && this.Calendar.Equals((object) other.Calendar);

    public static bool operator ==(LocalDateTime left, LocalDateTime right) => left.Equals(right);

    public static bool operator !=(LocalDateTime left, LocalDateTime right) => !(left == right);

    public static bool operator <(LocalDateTime lhs, LocalDateTime rhs) => lhs.LocalInstant < rhs.LocalInstant && object.Equals((object) lhs.Calendar, (object) rhs.Calendar);

    public static bool operator <=(LocalDateTime lhs, LocalDateTime rhs) => lhs.LocalInstant <= rhs.LocalInstant && object.Equals((object) lhs.Calendar, (object) rhs.Calendar);

    public static bool operator >(LocalDateTime lhs, LocalDateTime rhs) => lhs.LocalInstant > rhs.LocalInstant && object.Equals((object) lhs.Calendar, (object) rhs.Calendar);

    public static bool operator >=(LocalDateTime lhs, LocalDateTime rhs) => lhs.LocalInstant >= rhs.LocalInstant && object.Equals((object) lhs.Calendar, (object) rhs.Calendar);

    public int CompareTo(LocalDateTime other) => this.LocalInstant.CompareTo(other.LocalInstant);

    int IComparable.CompareTo(object obj)
    {
      if (obj == null)
        return 1;
      Preconditions.CheckArgument(obj is LocalDateTime, nameof (obj), "Object must be of type NodaTime.LocalDateTime.");
      return this.CompareTo((LocalDateTime) obj);
    }

    public static LocalDateTime operator +(LocalDateTime localDateTime, Period period) => localDateTime.Plus(period);

    public static LocalDateTime Add(LocalDateTime localDateTime, Period period) => localDateTime.Plus(period);

    [Pure]
    public LocalDateTime Plus(Period period)
    {
      Preconditions.CheckNotNull<Period>(period, nameof (period));
      return new LocalDateTime(period.AddTo(this.localInstant, this.Calendar, 1), this.Calendar);
    }

    public static LocalDateTime operator -(LocalDateTime localDateTime, Period period) => localDateTime.Minus(period);

    public static LocalDateTime Subtract(LocalDateTime localDateTime, Period period) => localDateTime.Minus(period);

    [Pure]
    public LocalDateTime Minus(Period period)
    {
      Preconditions.CheckNotNull<Period>(period, nameof (period));
      return new LocalDateTime(period.AddTo(this.localInstant, this.Calendar, -1), this.Calendar);
    }

    public override bool Equals(object obj) => obj is LocalDateTime other && this.Equals(other);

    public override int GetHashCode() => HashCodeHelper.Hash<CalendarSystem>(HashCodeHelper.Hash<LocalInstant>(HashCodeHelper.Initialize(), this.LocalInstant), this.Calendar);

    [Pure]
    public LocalDateTime WithCalendar([NotNull] CalendarSystem calendarSystem)
    {
      Preconditions.CheckNotNull<CalendarSystem>(calendarSystem, nameof (calendarSystem));
      return new LocalDateTime(this.localInstant, calendarSystem);
    }

    [Pure]
    public LocalDateTime PlusYears(int years) => new LocalDateTime(this.Calendar.PeriodFields.Years.Add(this.localInstant, (long) years), this.Calendar);

    [Pure]
    public LocalDateTime PlusMonths(int months) => new LocalDateTime(this.Calendar.PeriodFields.Months.Add(this.localInstant, (long) months), this.Calendar);

    [Pure]
    public LocalDateTime PlusDays(int days) => new LocalDateTime(this.Calendar.PeriodFields.Days.Add(this.localInstant, (long) days), this.Calendar);

    [Pure]
    public LocalDateTime PlusWeeks(int weeks) => new LocalDateTime(this.Calendar.PeriodFields.Weeks.Add(this.localInstant, (long) weeks), this.Calendar);

    [Pure]
    public LocalDateTime PlusHours(long hours) => new LocalDateTime(this.Calendar.PeriodFields.Hours.Add(this.localInstant, hours), this.Calendar);

    [Pure]
    public LocalDateTime PlusMinutes(long minutes) => new LocalDateTime(this.Calendar.PeriodFields.Minutes.Add(this.localInstant, minutes), this.Calendar);

    [Pure]
    public LocalDateTime PlusSeconds(long seconds) => new LocalDateTime(this.Calendar.PeriodFields.Seconds.Add(this.localInstant, seconds), this.Calendar);

    [Pure]
    public LocalDateTime PlusMilliseconds(long milliseconds) => new LocalDateTime(this.Calendar.PeriodFields.Milliseconds.Add(this.localInstant, milliseconds), this.Calendar);

    [Pure]
    public LocalDateTime PlusTicks(long ticks) => new LocalDateTime(this.Calendar.PeriodFields.Ticks.Add(this.localInstant, ticks), this.Calendar);

    [Pure]
    public LocalDateTime Next(IsoDayOfWeek targetDayOfWeek)
    {
      if (targetDayOfWeek < IsoDayOfWeek.Monday || targetDayOfWeek > IsoDayOfWeek.Sunday)
        throw new ArgumentOutOfRangeException(nameof (targetDayOfWeek));
      IsoDayOfWeek isoDayOfWeek = this.IsoDayOfWeek;
      int days = targetDayOfWeek - isoDayOfWeek;
      if (days <= 0)
        checked { days += 7; }
      return this.PlusDays(days);
    }

    [Pure]
    public LocalDateTime Previous(IsoDayOfWeek targetDayOfWeek)
    {
      if (targetDayOfWeek < IsoDayOfWeek.Monday || targetDayOfWeek > IsoDayOfWeek.Sunday)
        throw new ArgumentOutOfRangeException(nameof (targetDayOfWeek));
      IsoDayOfWeek isoDayOfWeek = this.IsoDayOfWeek;
      int days = targetDayOfWeek - isoDayOfWeek;
      if (days >= 0)
        checked { days -= 7; }
      return this.PlusDays(days);
    }

    [Pure]
    public OffsetDateTime WithOffset(Offset offset) => new OffsetDateTime(this, offset);

    [Pure]
    public ZonedDateTime InUtc() => new ZonedDateTime(this, Offset.Zero, DateTimeZone.Utc);

    [Pure]
    public ZonedDateTime InZoneStrictly([NotNull] DateTimeZone zone)
    {
      Preconditions.CheckNotNull<DateTimeZone>(zone, nameof (zone));
      return zone.AtStrictly(this);
    }

    [Pure]
    public ZonedDateTime InZoneLeniently([NotNull] DateTimeZone zone)
    {
      Preconditions.CheckNotNull<DateTimeZone>(zone, nameof (zone));
      return zone.AtLeniently(this);
    }

    [Pure]
    public ZonedDateTime InZone([NotNull] DateTimeZone zone, [NotNull] ZoneLocalMappingResolver resolver)
    {
      Preconditions.CheckNotNull<DateTimeZone>(zone, nameof (zone));
      Preconditions.CheckNotNull<ZoneLocalMappingResolver>(resolver, nameof (resolver));
      return zone.ResolveLocal(this, resolver);
    }

    public override string ToString() => LocalDateTimePattern.BclSupport.Format(this, (string) null, (IFormatProvider) CultureInfo.CurrentCulture);

    public string ToString(string patternText, IFormatProvider formatProvider) => LocalDateTimePattern.BclSupport.Format(this, patternText, formatProvider);

    XmlSchema IXmlSerializable.GetSchema() => (XmlSchema) null;

    void IXmlSerializable.ReadXml(XmlReader reader)
    {
      Preconditions.CheckNotNull<XmlReader>(reader, nameof (reader));
      LocalDateTimePattern localDateTimePattern = LocalDateTimePattern.ExtendedIsoPattern;
      if (reader.MoveToAttribute("calendar"))
      {
        CalendarSystem calendarSystem = CalendarSystem.ForId(reader.Value);
        LocalDateTime newTemplateValue = localDateTimePattern.TemplateValue.WithCalendar(calendarSystem);
        localDateTimePattern = localDateTimePattern.WithTemplateValue(newTemplateValue);
        reader.MoveToElement();
      }
      string text = reader.ReadElementContentAsString();
      this = localDateTimePattern.Parse(text).Value;
    }

    void IXmlSerializable.WriteXml(XmlWriter writer)
    {
      Preconditions.CheckNotNull<XmlWriter>(writer, nameof (writer));
      if (this.Calendar != CalendarSystem.Iso)
        writer.WriteAttributeString("calendar", this.Calendar.Id);
      writer.WriteString(LocalDateTimePattern.ExtendedIsoPattern.Format(this));
    }
  }
}
