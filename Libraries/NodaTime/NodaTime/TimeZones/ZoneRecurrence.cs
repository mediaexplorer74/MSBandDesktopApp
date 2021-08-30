// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.ZoneRecurrence
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.TimeZones.IO;
using NodaTime.Utility;
using System;
using System.Text;

namespace NodaTime.TimeZones
{
  internal sealed class ZoneRecurrence : IEquatable<ZoneRecurrence>
  {
    private readonly int fromYear;
    private readonly string name;
    private readonly Offset savings;
    private readonly int toYear;
    private readonly ZoneYearOffset yearOffset;

    public ZoneRecurrence(
      string name,
      Offset savings,
      ZoneYearOffset yearOffset,
      int fromYear,
      int toYear)
    {
      Preconditions.CheckNotNull<string>(name, nameof (name));
      Preconditions.CheckNotNull<ZoneYearOffset>(yearOffset, nameof (yearOffset));
      this.name = name;
      this.savings = savings;
      this.yearOffset = yearOffset;
      this.fromYear = fromYear;
      this.toYear = toYear;
    }

    public string Name => this.name;

    public Offset Savings => this.savings;

    public ZoneYearOffset YearOffset => this.yearOffset;

    public int FromYear => this.fromYear;

    public int ToYear => this.toYear;

    public bool IsInfinite => this.toYear == int.MaxValue;

    internal ZoneRecurrence WithName(string newName) => new ZoneRecurrence(newName, this.savings, this.yearOffset, this.fromYear, this.toYear);

    public bool Equals(ZoneRecurrence other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return this.savings == other.savings && this.fromYear == other.fromYear && this.toYear == other.toYear && this.name == other.name && this.yearOffset.Equals(other.yearOffset);
    }

    internal Transition? Next(
      Instant instant,
      Offset standardOffset,
      Offset previousSavings)
    {
      CalendarSystem iso = CalendarSystem.Iso;
      Offset offset = standardOffset + previousSavings;
      if ((instant == Instant.MinValue ? int.MinValue : iso.GetYear(instant.Plus(offset))) < this.fromYear)
      {
        instant = iso.GetLocalInstant(this.fromYear, 1, 1, 0, 0).Minus(offset);
        instant -= Duration.Epsilon;
      }
      Instant instant1 = this.yearOffset.Next(instant, standardOffset, previousSavings);
      return instant1 >= instant && iso.GetYear(this.yearOffset.AddDay ? instant1.Minus(Duration.OneStandardDay).Plus(offset) : instant1.Plus(offset)) > this.toYear ? new Transition?() : new Transition?(new Transition(instant1, offset, standardOffset + this.Savings));
    }

    internal Transition? Previous(
      Instant instant,
      Offset standardOffset,
      Offset previousSavings)
    {
      CalendarSystem iso = CalendarSystem.Iso;
      Offset offset = standardOffset + previousSavings;
      if ((instant == Instant.MaxValue ? int.MaxValue : iso.GetYear(instant.Plus(offset))) > this.toYear)
        instant = iso.GetLocalInstant(checked (this.toYear + 1), 1, 1, 0, 0).Minus(offset);
      Instant instant1 = this.yearOffset.Previous(instant, standardOffset, previousSavings);
      return instant1 <= instant && iso.GetYear(instant1.Plus(offset)) < this.fromYear ? new Transition?() : new Transition?(new Transition(instant1, offset, standardOffset + this.Savings));
    }

    internal Transition NextOrFail(
      Instant instant,
      Offset standardOffset,
      Offset previousSavings)
    {
      return (this.Next(instant, standardOffset, previousSavings) ?? throw new InvalidOperationException(string.Format("Noda Time bug or bad data: Expected a transition later than {0}; standard offset = {1}; previousSavings = {2}; recurrence = {3}", (object) instant, (object) standardOffset, (object) previousSavings, (object) this))).Value;
    }

    internal Transition PreviousOrFail(
      Instant instant,
      Offset standardOffset,
      Offset previousSavings)
    {
      return (this.Previous(instant, standardOffset, previousSavings) ?? throw new InvalidOperationException(string.Format("Noda Time bug or bad data: Expected a transition earlier than {0}; standard offset = {1}; previousSavings = {2}; recurrence = {3}", (object) instant, (object) standardOffset, (object) previousSavings, (object) this))).Value;
    }

    internal void Write(IDateTimeZoneWriter writer)
    {
      writer.WriteString(this.Name);
      writer.WriteOffset(this.Savings);
      this.YearOffset.Write(writer);
      writer.WriteCount(Math.Max(this.fromYear, 0));
      writer.WriteCount(this.toYear);
    }

    internal void WriteLegacy(LegacyDateTimeZoneWriter writer)
    {
      writer.WriteString(this.Name);
      writer.WriteOffset(this.Savings);
      this.YearOffset.WriteLegacy(writer);
      writer.WriteCount(Math.Max(this.fromYear, 0));
      writer.WriteCount(this.toYear);
    }

    public static ZoneRecurrence Read(IDateTimeZoneReader reader)
    {
      Preconditions.CheckNotNull<IDateTimeZoneReader>(reader, nameof (reader));
      string name = reader.ReadString();
      Offset savings = reader.ReadOffset();
      ZoneYearOffset yearOffset = ZoneYearOffset.Read(reader);
      int fromYear = reader.ReadCount();
      if (fromYear == 0)
        fromYear = int.MinValue;
      int toYear = reader.ReadCount();
      return new ZoneRecurrence(name, savings, yearOffset, fromYear, toYear);
    }

    internal static ZoneRecurrence ReadLegacy(LegacyDateTimeZoneReader reader)
    {
      Preconditions.CheckNotNull<LegacyDateTimeZoneReader>(reader, nameof (reader));
      string name = reader.ReadString();
      Offset savings = reader.ReadOffset();
      ZoneYearOffset yearOffset = ZoneYearOffset.ReadLegacy(reader);
      int fromYear = reader.ReadCount();
      if (fromYear == 0)
        fromYear = int.MinValue;
      int toYear = reader.ReadCount();
      return new ZoneRecurrence(name, savings, yearOffset, fromYear, toYear);
    }

    public override bool Equals(object obj) => this.Equals(obj as ZoneRecurrence);

    public override int GetHashCode() => HashCodeHelper.Hash<ZoneYearOffset>(HashCodeHelper.Hash<string>(HashCodeHelper.Hash<Offset>(HashCodeHelper.Initialize(), this.savings), this.name), this.yearOffset);

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.Name);
      stringBuilder.Append(" ").Append((object) this.Savings);
      stringBuilder.Append(" ").Append((object) this.YearOffset);
      stringBuilder.Append(" [").Append(this.fromYear).Append("-").Append(this.toYear).Append("]");
      return stringBuilder.ToString();
    }

    internal ZoneRecurrence ToStartOfTime() => this.fromYear != int.MinValue ? new ZoneRecurrence(this.name, this.savings, this.yearOffset, int.MinValue, this.toYear) : this;

    internal ZoneRecurrence ToInfinity() => !this.IsInfinite ? new ZoneRecurrence(this.name, this.savings, this.yearOffset, int.MinValue, int.MaxValue) : this;
  }
}
