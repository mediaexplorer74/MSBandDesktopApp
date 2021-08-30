// Decompiled with JetBrains decompiler
// Type: NodaTime.LocalTime
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
  public struct LocalTime : 
    IEquatable<LocalTime>,
    IComparable<LocalTime>,
    IFormattable,
    IComparable,
    IXmlSerializable
  {
    public static readonly LocalTime Midnight = new LocalTime(0, 0, 0);
    public static readonly LocalTime Noon = new LocalTime(12, 0, 0);
    private readonly long ticks;

    [Pure]
    public LocalDateTime On(LocalDate date) => date + this;

    public LocalTime(int hour, int minute)
    {
      Preconditions.CheckArgumentRange(nameof (hour), hour, 0, 23);
      Preconditions.CheckArgumentRange(nameof (minute), minute, 0, 59);
      this.ticks = (long) hour * 36000000000L + (long) minute * 600000000L;
    }

    public LocalTime(int hour, int minute, int second)
    {
      Preconditions.CheckArgumentRange(nameof (hour), hour, 0, 23);
      Preconditions.CheckArgumentRange(nameof (minute), minute, 0, 59);
      Preconditions.CheckArgumentRange(nameof (second), second, 0, 59);
      this.ticks = (long) hour * 36000000000L + (long) minute * 600000000L + (long) second * 10000000L;
    }

    public LocalTime(int hour, int minute, int second, int millisecond)
    {
      Preconditions.CheckArgumentRange(nameof (hour), hour, 0, 23);
      Preconditions.CheckArgumentRange(nameof (minute), minute, 0, 59);
      Preconditions.CheckArgumentRange(nameof (second), second, 0, 59);
      Preconditions.CheckArgumentRange(nameof (millisecond), millisecond, 0, 999);
      this.ticks = (long) hour * 36000000000L + (long) minute * 600000000L + (long) second * 10000000L + (long) millisecond * 10000L;
    }

    public LocalTime(
      int hour,
      int minute,
      int second,
      int millisecond,
      int tickWithinMillisecond)
    {
      Preconditions.CheckArgumentRange(nameof (hour), hour, 0, 23);
      Preconditions.CheckArgumentRange(nameof (minute), minute, 0, 59);
      Preconditions.CheckArgumentRange(nameof (second), second, 0, 59);
      Preconditions.CheckArgumentRange(nameof (millisecond), millisecond, 0, 999);
      Preconditions.CheckArgumentRange(nameof (tickWithinMillisecond), (long) tickWithinMillisecond, 0L, 9999L);
      this.ticks = (long) hour * 36000000000L + (long) minute * 600000000L + (long) second * 10000000L + (long) millisecond * 10000L + (long) tickWithinMillisecond;
    }

    public static LocalTime FromHourMinuteSecondTick(
      int hour,
      int minute,
      int second,
      int tickWithinSecond)
    {
      Preconditions.CheckArgumentRange(nameof (hour), hour, 0, 23);
      Preconditions.CheckArgumentRange(nameof (minute), minute, 0, 59);
      Preconditions.CheckArgumentRange(nameof (second), second, 0, 59);
      Preconditions.CheckArgumentRange(nameof (tickWithinSecond), (long) tickWithinSecond, 0L, 9999999L);
      return new LocalTime((long) hour * 36000000000L + (long) minute * 600000000L + (long) second * 10000000L + (long) tickWithinSecond);
    }

    public static LocalTime FromTicksSinceMidnight(long ticks)
    {
      Preconditions.CheckArgumentRange(nameof (ticks), ticks, 0L, 863999999999L);
      return new LocalTime(ticks);
    }

    public static LocalTime FromMillisecondsSinceMidnight(int milliseconds)
    {
      Preconditions.CheckArgumentRange(nameof (milliseconds), milliseconds, 0, 86399999);
      return new LocalTime((long) milliseconds * 10000L);
    }

    public static LocalTime FromSecondsSinceMidnight(int seconds)
    {
      Preconditions.CheckArgumentRange(nameof (seconds), seconds, 0, 86399);
      return new LocalTime((long) seconds * 10000000L);
    }

    internal LocalTime(long ticks) => this.ticks = ticks;

    public int Hour => TimeOfDayCalculator.GetHourOfDayFromTickOfDay(this.ticks);

    public int ClockHourOfHalfDay => CalendarSystem.Iso.GetClockHourOfHalfDay(new LocalInstant(this.ticks));

    public int Minute => TimeOfDayCalculator.GetMinuteOfHourFromTickOfDay(this.ticks);

    public int Second => TimeOfDayCalculator.GetSecondOfMinuteFromTickOfDay(this.ticks);

    public int Millisecond => TimeOfDayCalculator.GetMillisecondOfSecondFromTickOfDay(this.ticks);

    public int TickOfSecond => TimeOfDayCalculator.GetTickOfSecondFromTickOfDay(this.ticks);

    public long TickOfDay => this.ticks;

    public LocalDateTime LocalDateTime => new LocalDateTime(new LocalInstant(this.ticks));

    public static LocalTime operator +(LocalTime time, Period period)
    {
      Preconditions.CheckNotNull<Period>(period, nameof (period));
      Preconditions.CheckArgument(!period.HasDateComponent, nameof (period), "Cannot add a period with a date component to a time");
      return (time.LocalDateTime + period).TimeOfDay;
    }

    public static LocalTime Add(LocalTime time, Period period) => time + period;

    [Pure]
    public LocalTime Plus(Period period) => this + period;

    public static LocalTime operator -(LocalTime time, Period period)
    {
      Preconditions.CheckNotNull<Period>(period, nameof (period));
      Preconditions.CheckArgument(!period.HasDateComponent, nameof (period), "Cannot subtract a period with a date component from a time");
      return (time.LocalDateTime - period).TimeOfDay;
    }

    public static LocalTime Subtract(LocalTime time, Period period) => time - period;

    [Pure]
    public LocalTime Minus(Period period) => this - period;

    public static bool operator ==(LocalTime lhs, LocalTime rhs) => lhs.ticks == rhs.ticks;

    public static bool operator !=(LocalTime lhs, LocalTime rhs) => lhs.ticks != rhs.ticks;

    public static bool operator <(LocalTime lhs, LocalTime rhs) => lhs.ticks < rhs.ticks;

    public static bool operator <=(LocalTime lhs, LocalTime rhs) => lhs.ticks <= rhs.ticks;

    public static bool operator >(LocalTime lhs, LocalTime rhs) => lhs.ticks > rhs.ticks;

    public static bool operator >=(LocalTime lhs, LocalTime rhs) => lhs.ticks >= rhs.ticks;

    public int CompareTo(LocalTime other) => this.ticks.CompareTo(other.ticks);

    int IComparable.CompareTo(object obj)
    {
      if (obj == null)
        return 1;
      Preconditions.CheckArgument(obj is LocalTime, nameof (obj), "Object must be of type NodaTime.LocalTime.");
      return this.CompareTo((LocalTime) obj);
    }

    public override int GetHashCode() => this.ticks.GetHashCode();

    public bool Equals(LocalTime other) => this == other;

    public override bool Equals(object obj) => obj is LocalTime localTime && this == localTime;

    [Pure]
    public LocalTime PlusHours(long hours) => this.LocalDateTime.PlusHours(hours).TimeOfDay;

    [Pure]
    public LocalTime PlusMinutes(long minutes) => this.LocalDateTime.PlusMinutes(minutes).TimeOfDay;

    [Pure]
    public LocalTime PlusSeconds(long seconds) => this.LocalDateTime.PlusSeconds(seconds).TimeOfDay;

    [Pure]
    public LocalTime PlusMilliseconds(long milliseconds) => this.LocalDateTime.PlusMilliseconds(milliseconds).TimeOfDay;

    [Pure]
    public LocalTime PlusTicks(long ticks) => this.LocalDateTime.PlusTicks(ticks).TimeOfDay;

    public override string ToString() => LocalTimePattern.BclSupport.Format(this, (string) null, (IFormatProvider) CultureInfo.CurrentCulture);

    public string ToString(string patternText, IFormatProvider formatProvider) => LocalTimePattern.BclSupport.Format(this, patternText, formatProvider);

    XmlSchema IXmlSerializable.GetSchema() => (XmlSchema) null;

    void IXmlSerializable.ReadXml(XmlReader reader)
    {
      Preconditions.CheckNotNull<XmlReader>(reader, nameof (reader));
      this = LocalTimePattern.ExtendedIsoPattern.Parse(reader.ReadElementContentAsString()).Value;
    }

    void IXmlSerializable.WriteXml(XmlWriter writer)
    {
      Preconditions.CheckNotNull<XmlWriter>(writer, nameof (writer));
      writer.WriteString(LocalTimePattern.ExtendedIsoPattern.Format(this));
    }
  }
}
