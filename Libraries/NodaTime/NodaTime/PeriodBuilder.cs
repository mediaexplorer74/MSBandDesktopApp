// Decompiled with JetBrains decompiler
// Type: NodaTime.PeriodBuilder
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using JetBrains.Annotations;
using NodaTime.Annotations;
using NodaTime.Text;
using NodaTime.Utility;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NodaTime
{
  [Mutable]
  public sealed class PeriodBuilder : IXmlSerializable
  {
    public long Years { get; set; }

    public long Months { get; set; }

    public long Weeks { get; set; }

    public long Days { get; set; }

    public long Hours { get; set; }

    public long Minutes { get; set; }

    public long Seconds { get; set; }

    public long Milliseconds { get; set; }

    public long Ticks { get; set; }

    public PeriodBuilder()
    {
    }

    public PeriodBuilder([NotNull] Period period)
    {
      Preconditions.CheckNotNull<Period>(period, nameof (period));
      this.Years = period.Years;
      this.Months = period.Months;
      this.Weeks = period.Weeks;
      this.Days = period.Days;
      this.Hours = period.Hours;
      this.Minutes = period.Minutes;
      this.Seconds = period.Seconds;
      this.Milliseconds = period.Milliseconds;
      this.Ticks = period.Ticks;
    }

    public long this[PeriodUnits unit]
    {
      get
      {
        switch (unit)
        {
          case PeriodUnits.Years:
            return this.Years;
          case PeriodUnits.Months:
            return this.Months;
          case PeriodUnits.Weeks:
            return this.Weeks;
          case PeriodUnits.Days:
            return this.Days;
          case PeriodUnits.Hours:
            return this.Hours;
          case PeriodUnits.Minutes:
            return this.Minutes;
          case PeriodUnits.Seconds:
            return this.Seconds;
          case PeriodUnits.Milliseconds:
            return this.Milliseconds;
          case PeriodUnits.Ticks:
            return this.Ticks;
          default:
            throw new ArgumentOutOfRangeException(nameof (unit), "Indexer for PeriodBuilder only takes a single unit");
        }
      }
      set
      {
        switch (unit)
        {
          case PeriodUnits.Years:
            this.Years = value;
            break;
          case PeriodUnits.Months:
            this.Months = value;
            break;
          case PeriodUnits.Weeks:
            this.Weeks = value;
            break;
          case PeriodUnits.Days:
            this.Days = value;
            break;
          case PeriodUnits.Hours:
            this.Hours = value;
            break;
          case PeriodUnits.Minutes:
            this.Minutes = value;
            break;
          case PeriodUnits.Seconds:
            this.Seconds = value;
            break;
          case PeriodUnits.Milliseconds:
            this.Milliseconds = value;
            break;
          case PeriodUnits.Ticks:
            this.Ticks = value;
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof (unit), "Indexer for PeriodBuilder only takes a single unit");
        }
      }
    }

    public Period Build() => new Period(this.Years, this.Months, this.Weeks, this.Days, this.Hours, this.Minutes, this.Seconds, this.Milliseconds, this.Ticks);

    XmlSchema IXmlSerializable.GetSchema() => (XmlSchema) null;

    void IXmlSerializable.ReadXml(XmlReader reader)
    {
      string text = reader.ReadElementContentAsString();
      Period period = PeriodPattern.RoundtripPattern.Parse(text).Value;
      this.Years = period.Years;
      this.Months = period.Months;
      this.Weeks = period.Weeks;
      this.Days = period.Days;
      this.Hours = period.Hours;
      this.Minutes = period.Minutes;
      this.Seconds = period.Seconds;
      this.Milliseconds = period.Milliseconds;
      this.Ticks = period.Ticks;
    }

    void IXmlSerializable.WriteXml(XmlWriter writer) => writer.WriteString(PeriodPattern.RoundtripPattern.Format(this.Build()));
  }
}
