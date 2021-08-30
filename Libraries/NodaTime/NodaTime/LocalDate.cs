// Decompiled with JetBrains decompiler
// Type: NodaTime.LocalDate
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using JetBrains.Annotations;
using NodaTime.Calendars;
using NodaTime.Text;
using NodaTime.Utility;
using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NodaTime
{
  public struct LocalDate : 
    IEquatable<LocalDate>,
    IComparable<LocalDate>,
    IComparable,
    IFormattable,
    IXmlSerializable
  {
    private readonly LocalDateTime localTime;

    public LocalDate(int year, int month, int day)
      : this(year, month, day, CalendarSystem.Iso)
    {
    }

    public LocalDate(int year, int month, int day, [NotNull] CalendarSystem calendar)
      : this(new LocalDateTime(year, month, day, 0, 0, calendar))
    {
    }

    public LocalDate(Era era, int yearOfEra, int month, int day)
      : this(era, yearOfEra, month, day, CalendarSystem.Iso)
    {
    }

    public LocalDate(Era era, int yearOfEra, int month, int day, [NotNull] CalendarSystem calendar)
      : this(new LocalDateTime(Preconditions.CheckNotNull<CalendarSystem>(calendar, nameof (calendar)).GetLocalInstant(era, yearOfEra, month, day), calendar))
    {
    }

    internal LocalDate(LocalDateTime localTime) => this.localTime = localTime;

    public CalendarSystem Calendar => this.localTime.Calendar;

    public int Year => this.localTime.Year;

    public int Month => this.localTime.Month;

    public int Day => this.localTime.Day;

    public IsoDayOfWeek IsoDayOfWeek => this.localTime.IsoDayOfWeek;

    public int DayOfWeek => this.localTime.DayOfWeek;

    public int WeekYear => this.localTime.WeekYear;

    public int WeekOfWeekYear => this.localTime.WeekOfWeekYear;

    public int YearOfCentury => this.localTime.YearOfCentury;

    public int YearOfEra => this.localTime.YearOfEra;

    public Era Era => this.localTime.Era;

    public int DayOfYear => this.localTime.DayOfYear;

    [Pure]
    public LocalDateTime AtMidnight() => this.localTime;

    public static LocalDate FromWeekYearWeekAndDay(
      int weekYear,
      int weekOfWeekYear,
      IsoDayOfWeek dayOfWeek)
    {
      return new LocalDate(new LocalDateTime(CalendarSystem.Iso.GetLocalInstantFromWeekYearWeekAndDayOfWeek(weekYear, weekOfWeekYear, dayOfWeek)));
    }

    public static LocalDate operator +(LocalDate date, Period period)
    {
      Preconditions.CheckNotNull<Period>(period, nameof (period));
      Preconditions.CheckArgument(!period.HasTimeComponent, nameof (period), "Cannot add a period with a time component to a date");
      return new LocalDate(date.localTime + period);
    }

    public static LocalDate Add(LocalDate date, Period period) => date + period;

    [Pure]
    public LocalDate Plus(Period period) => this + period;

    public static LocalDateTime operator +(LocalDate date, LocalTime time) => new LocalDateTime(new LocalInstant(checked (date.localTime.LocalInstant.Ticks + time.TickOfDay)), date.localTime.Calendar);

    public static LocalDate operator -(LocalDate date, Period period)
    {
      Preconditions.CheckNotNull<Period>(period, nameof (period));
      Preconditions.CheckArgument(!period.HasTimeComponent, nameof (period), "Cannot subtract a period with a time component from a date");
      return new LocalDate(date.localTime - period);
    }

    public static LocalDate Subtract(LocalDate date, Period period) => date - period;

    [Pure]
    public LocalDate Minus(Period period) => this - period;

    public static bool operator ==(LocalDate lhs, LocalDate rhs) => lhs.localTime == rhs.localTime;

    public static bool operator !=(LocalDate lhs, LocalDate rhs) => lhs.localTime != rhs.localTime;

    public static bool operator <(LocalDate lhs, LocalDate rhs) => lhs.localTime < rhs.localTime;

    public static bool operator <=(LocalDate lhs, LocalDate rhs) => lhs.localTime <= rhs.localTime;

    public static bool operator >(LocalDate lhs, LocalDate rhs) => lhs.localTime > rhs.localTime;

    public static bool operator >=(LocalDate lhs, LocalDate rhs) => lhs.localTime >= rhs.localTime;

    public int CompareTo(LocalDate other) => this.localTime.CompareTo(other.localTime);

    int IComparable.CompareTo(object obj)
    {
      if (obj == null)
        return 1;
      Preconditions.CheckArgument(obj is LocalDate, nameof (obj), "Object must be of type NodaTime.LocalDate.");
      return this.CompareTo((LocalDate) obj);
    }

    public override int GetHashCode() => this.localTime.GetHashCode();

    public override bool Equals(object obj) => obj is LocalDate localDate && this == localDate;

    public bool Equals(LocalDate other) => this == other;

    [Pure]
    public LocalDate WithCalendar([NotNull] CalendarSystem calendarSystem) => new LocalDate(this.localTime.WithCalendar(calendarSystem));

    [Pure]
    public LocalDate PlusYears(int years) => new LocalDate(this.localTime.PlusYears(years));

    [Pure]
    public LocalDate PlusMonths(int months) => new LocalDate(this.localTime.PlusMonths(months));

    [Pure]
    public LocalDate PlusDays(int days) => new LocalDate(this.localTime.PlusDays(days));

    [Pure]
    public LocalDate PlusWeeks(int weeks) => new LocalDate(this.localTime.PlusWeeks(weeks));

    [Pure]
    public LocalDate Next(IsoDayOfWeek targetDayOfWeek) => new LocalDate(this.localTime.Next(targetDayOfWeek));

    [Pure]
    public LocalDate Previous(IsoDayOfWeek targetDayOfWeek) => new LocalDate(this.localTime.Previous(targetDayOfWeek));

    [Pure]
    public LocalDateTime At(LocalTime time) => this + time;

    public override string ToString() => LocalDatePattern.BclSupport.Format(this, (string) null, (IFormatProvider) CultureInfo.CurrentCulture);

    public string ToString(string patternText, IFormatProvider formatProvider) => LocalDatePattern.BclSupport.Format(this, patternText, formatProvider);

    XmlSchema IXmlSerializable.GetSchema() => (XmlSchema) null;

    void IXmlSerializable.ReadXml(XmlReader reader)
    {
      Preconditions.CheckNotNull<XmlReader>(reader, nameof (reader));
      LocalDatePattern localDatePattern = LocalDatePattern.IsoPattern;
      if (reader.MoveToAttribute("calendar"))
      {
        CalendarSystem calendarSystem = CalendarSystem.ForId(reader.Value);
        LocalDate newTemplateValue = localDatePattern.TemplateValue.WithCalendar(calendarSystem);
        localDatePattern = localDatePattern.WithTemplateValue(newTemplateValue);
        reader.MoveToElement();
      }
      string text = reader.ReadElementContentAsString();
      this = localDatePattern.Parse(text).Value;
    }

    void IXmlSerializable.WriteXml(XmlWriter writer)
    {
      Preconditions.CheckNotNull<XmlWriter>(writer, nameof (writer));
      if (this.Calendar != CalendarSystem.Iso)
        writer.WriteAttributeString("calendar", this.Calendar.Id);
      writer.WriteString(LocalDatePattern.IsoPattern.Format(this));
    }
  }
}
