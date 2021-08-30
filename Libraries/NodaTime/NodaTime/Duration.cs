// Decompiled with JetBrains decompiler
// Type: NodaTime.Duration
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
  public struct Duration : 
    IEquatable<Duration>,
    IComparable<Duration>,
    IComparable,
    IXmlSerializable,
    IFormattable
  {
    public static readonly Duration Zero = new Duration(0L);
    public static readonly Duration Epsilon = new Duration(1L);
    internal static readonly Duration OneStandardWeek = new Duration(6048000000000L);
    internal static readonly Duration OneStandardDay = new Duration(864000000000L);
    private static readonly Duration OneHour = new Duration(36000000000L);
    private static readonly Duration OneMinute = new Duration(600000000L);
    private static readonly Duration OneSecond = new Duration(10000000L);
    private static readonly Duration OneMillisecond = new Duration(10000L);
    private readonly long ticks;

    internal Duration(long ticks) => this.ticks = ticks;

    public long Ticks => this.ticks;

    public override bool Equals(object obj) => obj is Duration other && this.Equals(other);

    public override int GetHashCode() => this.Ticks.GetHashCode();

    public override string ToString() => DurationPattern.BclSupport.Format(this, (string) null, (IFormatProvider) CultureInfo.CurrentCulture);

    public string ToString(string patternText, IFormatProvider formatProvider) => DurationPattern.BclSupport.Format(this, patternText, formatProvider);

    public static Duration operator +(Duration left, Duration right) => new Duration(checked (left.Ticks + right.Ticks));

    public static Duration Add(Duration left, Duration right) => left + right;

    [Pure]
    public Duration Plus(Duration other) => this + other;

    public static Duration operator -(Duration left, Duration right) => new Duration(checked (left.Ticks - right.Ticks));

    public static Duration Subtract(Duration left, Duration right) => left - right;

    [Pure]
    public Duration Minus(Duration other) => this - other;

    public static Duration operator /(Duration left, long right) => new Duration(left.Ticks / right);

    public static Duration Divide(Duration left, long right) => left / right;

    public static Duration operator *(Duration left, long right) => new Duration(checked (left.Ticks * right));

    public static Duration operator *(long left, Duration right) => new Duration(checked (left * right.Ticks));

    public static Duration Multiply(Duration left, long right) => left * right;

    public static Duration Multiply(long left, Duration right) => left * right;

    public static bool operator ==(Duration left, Duration right) => left.Equals(right);

    public static bool operator !=(Duration left, Duration right) => !(left == right);

    public static bool operator <(Duration left, Duration right) => left.CompareTo(right) < 0;

    public static bool operator <=(Duration left, Duration right) => left.CompareTo(right) <= 0;

    public static bool operator >(Duration left, Duration right) => left.CompareTo(right) > 0;

    public static bool operator >=(Duration left, Duration right) => left.CompareTo(right) >= 0;

    public static Duration operator -(Duration duration) => new Duration(checked (-duration.Ticks));

    public static Duration Negate(Duration duration) => -duration;

    public int CompareTo(Duration other) => this.Ticks.CompareTo(other.Ticks);

    int IComparable.CompareTo(object obj)
    {
      if (obj == null)
        return 1;
      Preconditions.CheckArgument(obj is Duration, nameof (obj), "Object must be of type NodaTime.Duration.");
      return this.CompareTo((Duration) obj);
    }

    public bool Equals(Duration other) => this.Ticks == other.Ticks;

    public static Duration FromStandardWeeks(long weeks) => Duration.OneStandardWeek * weeks;

    public static Duration FromStandardDays(long days) => Duration.OneStandardDay * days;

    public static Duration FromHours(long hours) => Duration.OneHour * hours;

    public static Duration FromMinutes(long minutes) => Duration.OneMinute * minutes;

    public static Duration FromSeconds(long seconds) => Duration.OneSecond * seconds;

    public static Duration FromMilliseconds(long milliseconds) => Duration.OneMillisecond * milliseconds;

    public static Duration FromTicks(long ticks) => new Duration(ticks);

    public static Duration FromTimeSpan(TimeSpan timeSpan) => Duration.FromTicks(timeSpan.Ticks);

    [Pure]
    public TimeSpan ToTimeSpan() => new TimeSpan(this.ticks);

    XmlSchema IXmlSerializable.GetSchema() => (XmlSchema) null;

    void IXmlSerializable.ReadXml(XmlReader reader)
    {
      Preconditions.CheckNotNull<XmlReader>(reader, nameof (reader));
      this = DurationPattern.RoundtripPattern.Parse(reader.ReadElementContentAsString()).Value;
    }

    void IXmlSerializable.WriteXml(XmlWriter writer)
    {
      Preconditions.CheckNotNull<XmlWriter>(writer, nameof (writer));
      writer.WriteString(DurationPattern.RoundtripPattern.Format(this));
    }
  }
}
