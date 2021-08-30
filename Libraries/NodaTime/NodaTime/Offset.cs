// Decompiled with JetBrains decompiler
// Type: NodaTime.Offset
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
  public struct Offset : 
    IEquatable<Offset>,
    IComparable<Offset>,
    IFormattable,
    IComparable,
    IXmlSerializable
  {
    public static readonly Offset Zero = Offset.FromMilliseconds(0);
    public static readonly Offset MinValue = Offset.FromMilliseconds(-86399999);
    public static readonly Offset MaxValue = Offset.FromMilliseconds(86399999);
    private readonly int milliseconds;

    private Offset(int milliseconds)
    {
      Preconditions.CheckArgumentRange(nameof (milliseconds), milliseconds, -86399999, 86399999);
      this.milliseconds = milliseconds;
    }

    public int Milliseconds => this.milliseconds;

    public long Ticks => checked ((long) this.Milliseconds * 10000L);

    public static Offset Max(Offset x, Offset y) => !(x > y) ? y : x;

    public static Offset Min(Offset x, Offset y) => !(x < y) ? y : x;

    public static Offset operator -(Offset offset) => Offset.FromMilliseconds(checked (-offset.Milliseconds));

    public static Offset Negate(Offset offset) => -offset;

    public static Offset operator +(Offset offset) => offset;

    public static Offset operator +(Offset left, Offset right) => Offset.FromMilliseconds(checked (left.Milliseconds + right.Milliseconds));

    public static Offset Add(Offset left, Offset right) => left + right;

    [Pure]
    public Offset Plus(Offset other) => this + other;

    public static Offset operator -(Offset minuend, Offset subtrahend) => Offset.FromMilliseconds(checked (minuend.Milliseconds - subtrahend.Milliseconds));

    public static Offset Subtract(Offset minuend, Offset subtrahend) => minuend - subtrahend;

    [Pure]
    public Offset Minus(Offset other) => this - other;

    public static bool operator ==(Offset left, Offset right) => left.Equals(right);

    public static bool operator !=(Offset left, Offset right) => !(left == right);

    public static bool operator <(Offset left, Offset right) => left.CompareTo(right) < 0;

    public static bool operator <=(Offset left, Offset right) => left.CompareTo(right) <= 0;

    public static bool operator >(Offset left, Offset right) => left.CompareTo(right) > 0;

    public static bool operator >=(Offset left, Offset right) => left.CompareTo(right) >= 0;

    public int CompareTo(Offset other) => this.Milliseconds.CompareTo(other.Milliseconds);

    int IComparable.CompareTo(object obj)
    {
      if (obj == null)
        return 1;
      Preconditions.CheckArgument(obj is Offset, nameof (obj), "Object must be of type NodaTime.Offset.");
      return this.CompareTo((Offset) obj);
    }

    public bool Equals(Offset other) => this.Milliseconds == other.Milliseconds;

    public override bool Equals(object obj) => obj is Offset other && this.Equals(other);

    public override int GetHashCode() => this.Milliseconds.GetHashCode();

    public override string ToString() => OffsetPattern.BclSupport.Format(this, (string) null, (IFormatProvider) CultureInfo.CurrentCulture);

    public string ToString(string patternText, IFormatProvider formatProvider) => OffsetPattern.BclSupport.Format(this, patternText, formatProvider);

    public static Offset FromMilliseconds(int milliseconds) => new Offset(milliseconds);

    public static Offset FromTicks(long ticks) => new Offset(checked ((int) unchecked (ticks / 10000L)));

    public static Offset FromHours(int hours) => new Offset(checked (hours * 3600000));

    public static Offset FromHoursAndMinutes(int hours, int minutes) => new Offset(checked (hours * 3600000 + minutes * 60000));

    [Pure]
    public TimeSpan ToTimeSpan() => TimeSpan.FromMilliseconds((double) this.milliseconds);

    internal static Offset FromTimeSpan(TimeSpan timeSpan)
    {
      long totalMilliseconds = checked ((long) timeSpan.TotalMilliseconds);
      Preconditions.CheckArgumentRange(nameof (timeSpan), totalMilliseconds, (long) Offset.MinValue.Milliseconds, (long) Offset.MaxValue.Milliseconds);
      return new Offset(checked ((int) totalMilliseconds));
    }

    XmlSchema IXmlSerializable.GetSchema() => (XmlSchema) null;

    void IXmlSerializable.ReadXml(XmlReader reader)
    {
      Preconditions.CheckNotNull<XmlReader>(reader, nameof (reader));
      this = OffsetPattern.GeneralInvariantPattern.Parse(reader.ReadElementContentAsString()).Value;
    }

    void IXmlSerializable.WriteXml(XmlWriter writer)
    {
      Preconditions.CheckNotNull<XmlWriter>(writer, nameof (writer));
      writer.WriteString(OffsetPattern.GeneralInvariantPattern.Format(this));
    }
  }
}
