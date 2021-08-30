// Decompiled with JetBrains decompiler
// Type: NodaTime.Interval
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using JetBrains.Annotations;
using NodaTime.Text;
using NodaTime.Utility;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NodaTime
{
  public struct Interval : IEquatable<Interval>, IXmlSerializable
  {
    private readonly Instant start;
    private readonly Instant end;

    public Interval(Instant start, Instant end)
    {
      this.start = !(end < start) ? start : throw new ArgumentOutOfRangeException(nameof (end), "The end parameter must be equal to or later than the start parameter");
      this.end = end;
    }

    public Instant Start => this.start;

    public Instant End => this.end;

    public Duration Duration => this.end - this.start;

    [Pure]
    public bool Contains(Instant instant)
    {
      if (!(instant >= this.start))
        return false;
      return instant < this.end || this.end == Instant.MaxValue;
    }

    public bool Equals(Interval other) => this.Start == other.Start && this.End == other.End;

    public override bool Equals(object obj) => obj is Interval other && this.Equals(other);

    public override int GetHashCode() => HashCodeHelper.Hash<Instant>(HashCodeHelper.Hash<Instant>(HashCodeHelper.Initialize(), this.Start), this.End);

    public override string ToString()
    {
      InstantPattern extendedIsoPattern = InstantPattern.ExtendedIsoPattern;
      return extendedIsoPattern.Format(this.Start) + "/" + extendedIsoPattern.Format(this.End);
    }

    public static bool operator ==(Interval left, Interval right) => left.Equals(right);

    public static bool operator !=(Interval left, Interval right) => !(left == right);

    XmlSchema IXmlSerializable.GetSchema() => (XmlSchema) null;

    void IXmlSerializable.ReadXml(XmlReader reader)
    {
      Preconditions.CheckNotNull<XmlReader>(reader, nameof (reader));
      InstantPattern extendedIsoPattern = InstantPattern.ExtendedIsoPattern;
      if (!reader.MoveToAttribute("start"))
        throw new ArgumentException("No start specified in XML for Interval");
      Instant start = extendedIsoPattern.Parse(reader.Value).Value;
      if (!reader.MoveToAttribute("end"))
        throw new ArgumentException("No end specified in XML for Interval");
      Instant end = extendedIsoPattern.Parse(reader.Value).Value;
      this = new Interval(start, end);
      reader.Skip();
    }

    void IXmlSerializable.WriteXml(XmlWriter writer)
    {
      Preconditions.CheckNotNull<XmlWriter>(writer, nameof (writer));
      InstantPattern extendedIsoPattern = InstantPattern.ExtendedIsoPattern;
      writer.WriteAttributeString("start", extendedIsoPattern.Format(this.start));
      writer.WriteAttributeString("end", extendedIsoPattern.Format(this.end));
    }
  }
}
