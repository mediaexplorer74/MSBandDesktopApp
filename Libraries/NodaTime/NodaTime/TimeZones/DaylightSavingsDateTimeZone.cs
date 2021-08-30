// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.DaylightSavingsDateTimeZone
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.TimeZones.IO;
using NodaTime.Utility;

namespace NodaTime.TimeZones
{
  internal sealed class DaylightSavingsDateTimeZone : DateTimeZone
  {
    private readonly Offset standardOffset;
    private readonly ZoneRecurrence standardRecurrence;
    private readonly ZoneRecurrence dstRecurrence;

    internal DaylightSavingsDateTimeZone(
      string id,
      Offset standardOffset,
      ZoneRecurrence startRecurrence,
      ZoneRecurrence endRecurrence)
      : base(id, false, standardOffset + Offset.Min(startRecurrence.Savings, endRecurrence.Savings), standardOffset + Offset.Max(startRecurrence.Savings, endRecurrence.Savings))
    {
      this.standardOffset = standardOffset;
      startRecurrence = startRecurrence.ToStartOfTime();
      endRecurrence = endRecurrence.ToStartOfTime();
      Preconditions.CheckArgument(startRecurrence.IsInfinite, nameof (startRecurrence), "Start recurrence must extend to the end of time");
      Preconditions.CheckArgument(endRecurrence.IsInfinite, nameof (endRecurrence), "End recurrence must extend to the end of time");
      ZoneRecurrence zoneRecurrence1 = startRecurrence;
      ZoneRecurrence zoneRecurrence2 = endRecurrence;
      if (startRecurrence.Savings == Offset.Zero)
      {
        zoneRecurrence1 = endRecurrence;
        zoneRecurrence2 = startRecurrence;
      }
      Preconditions.CheckArgument(zoneRecurrence2.Savings == Offset.Zero, nameof (startRecurrence), "At least one recurrence must not have savings applied");
      this.dstRecurrence = zoneRecurrence1;
      this.standardRecurrence = zoneRecurrence2;
    }

    protected override bool EqualsImpl(DateTimeZone other)
    {
      DaylightSavingsDateTimeZone savingsDateTimeZone = (DaylightSavingsDateTimeZone) other;
      return this.Id == savingsDateTimeZone.Id && this.standardOffset == savingsDateTimeZone.standardOffset && this.dstRecurrence.Equals(savingsDateTimeZone.dstRecurrence) && this.standardRecurrence.Equals(savingsDateTimeZone.standardRecurrence);
    }

    public override int GetHashCode() => HashCodeHelper.Hash<ZoneRecurrence>(HashCodeHelper.Hash<ZoneRecurrence>(HashCodeHelper.Hash<Offset>(HashCodeHelper.Hash<string>(HashCodeHelper.Initialize(), this.Id), this.standardOffset), this.dstRecurrence), this.standardRecurrence);

    public override ZoneInterval GetZoneInterval(Instant instant)
    {
      Transition transition1 = this.PreviousTransition(instant + Duration.Epsilon);
      Transition transition2 = this.NextTransition(instant);
      ZoneRecurrence matchingRecurrence = this.FindMatchingRecurrence(instant);
      return new ZoneInterval(matchingRecurrence.Name, transition1.Instant, transition2.Instant, this.standardOffset + matchingRecurrence.Savings, matchingRecurrence.Savings);
    }

    private ZoneRecurrence FindMatchingRecurrence(Instant instant) => !(this.dstRecurrence.NextOrFail(instant, this.standardOffset, this.standardRecurrence.Savings).Instant > this.standardRecurrence.NextOrFail(instant, this.standardOffset, this.dstRecurrence.Savings).Instant) ? this.standardRecurrence : this.dstRecurrence;

    private Transition NextTransition(Instant instant)
    {
      Transition transition1 = this.dstRecurrence.NextOrFail(instant, this.standardOffset, this.standardRecurrence.Savings);
      Transition transition2 = this.standardRecurrence.NextOrFail(instant, this.standardOffset, this.dstRecurrence.Savings);
      return !(transition1.Instant > transition2.Instant) ? transition1 : transition2;
    }

    private Transition PreviousTransition(Instant instant)
    {
      Transition transition1 = this.dstRecurrence.PreviousOrFail(instant, this.standardOffset, this.standardRecurrence.Savings);
      Transition transition2 = this.standardRecurrence.PreviousOrFail(instant, this.standardOffset, this.dstRecurrence.Savings);
      return !(transition1.Instant > transition2.Instant) ? transition2 : transition1;
    }

    public override Offset GetUtcOffset(Instant instant) => this.FindMatchingRecurrence(instant).Savings + this.standardOffset;

    internal void Write(IDateTimeZoneWriter writer)
    {
      Preconditions.CheckNotNull<IDateTimeZoneWriter>(writer, nameof (writer));
      writer.WriteOffset(this.standardOffset);
      writer.WriteString(this.standardRecurrence.Name);
      this.standardRecurrence.YearOffset.Write(writer);
      writer.WriteString(this.dstRecurrence.Name);
      this.dstRecurrence.YearOffset.Write(writer);
      writer.WriteOffset(this.dstRecurrence.Savings);
    }

    internal void WriteLegacy(LegacyDateTimeZoneWriter writer)
    {
      Preconditions.CheckNotNull<LegacyDateTimeZoneWriter>(writer, nameof (writer));
      writer.WriteOffset(this.standardOffset);
      this.dstRecurrence.WriteLegacy(writer);
      this.standardRecurrence.WriteLegacy(writer);
    }

    internal static DaylightSavingsDateTimeZone Read(
      IDateTimeZoneReader reader,
      string id)
    {
      Preconditions.CheckNotNull<IDateTimeZoneReader>(reader, nameof (reader));
      Offset standardOffset = reader.ReadOffset();
      string name1 = reader.ReadString();
      ZoneYearOffset yearOffset1 = ZoneYearOffset.Read(reader);
      string name2 = reader.ReadString();
      ZoneYearOffset yearOffset2 = ZoneYearOffset.Read(reader);
      Offset savings = reader.ReadOffset();
      ZoneRecurrence startRecurrence = new ZoneRecurrence(name1, Offset.Zero, yearOffset1, int.MinValue, int.MaxValue);
      ZoneRecurrence endRecurrence = new ZoneRecurrence(name2, savings, yearOffset2, int.MinValue, int.MaxValue);
      return new DaylightSavingsDateTimeZone(id, standardOffset, startRecurrence, endRecurrence);
    }

    internal static DateTimeZone ReadLegacy(LegacyDateTimeZoneReader reader, string id)
    {
      Preconditions.CheckNotNull<LegacyDateTimeZoneReader>(reader, nameof (reader));
      Offset standardOffset = reader.ReadOffset();
      ZoneRecurrence startRecurrence = ZoneRecurrence.ReadLegacy(reader);
      ZoneRecurrence endRecurrence = ZoneRecurrence.ReadLegacy(reader);
      return (DateTimeZone) new DaylightSavingsDateTimeZone(id, standardOffset, startRecurrence, endRecurrence);
    }
  }
}
