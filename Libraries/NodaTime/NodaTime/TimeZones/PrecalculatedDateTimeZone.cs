// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.PrecalculatedDateTimeZone
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.Annotations;
using NodaTime.TimeZones.IO;
using NodaTime.Utility;
using System;
using System.Collections.Generic;

namespace NodaTime.TimeZones
{
  internal sealed class PrecalculatedDateTimeZone : DateTimeZone
  {
    private readonly ZoneInterval[] periods;
    private readonly DateTimeZone tailZone;
    private readonly Instant tailZoneStart;
    private readonly ZoneInterval firstTailZoneInterval;

    [VisibleForTesting]
    internal PrecalculatedDateTimeZone(string id, ZoneInterval[] periods, DateTimeZone tailZone)
      : base(id, false, PrecalculatedDateTimeZone.ComputeOffset(periods, tailZone, new PrecalculatedDateTimeZone.OffsetAggregator(Offset.Min)), PrecalculatedDateTimeZone.ComputeOffset(periods, tailZone, new PrecalculatedDateTimeZone.OffsetAggregator(Offset.Max)))
    {
      this.tailZone = tailZone;
      this.periods = periods;
      this.tailZone = tailZone;
      this.tailZoneStart = periods[checked (periods.Length - 1)].End;
      if (tailZone != null)
        this.firstTailZoneInterval = tailZone.GetZoneInterval(this.tailZoneStart).WithStart(this.tailZoneStart);
      PrecalculatedDateTimeZone.ValidatePeriods(periods, tailZone);
    }

    internal static void ValidatePeriods(ZoneInterval[] periods, DateTimeZone tailZone)
    {
      Preconditions.CheckArgument(periods.Length > 0, nameof (periods), "No periods specified in precalculated time zone");
      Preconditions.CheckArgument(periods[0].Start == Instant.MinValue, nameof (periods), "Periods in precalculated time zone must start with the beginning of time");
      int index = 0;
      while (index < checked (periods.Length - 1))
      {
        Preconditions.CheckArgument(periods[index].End == periods[checked (index + 1)].Start, nameof (periods), "Non-adjoining ZoneIntervals for precalculated time zone");
        checked { ++index; }
      }
      Preconditions.CheckArgument(tailZone != null || periods[checked (periods.Length - 1)].End == Instant.MaxValue, nameof (tailZone), "Null tail zone given but periods don't cover all of time");
    }

    public override ZoneInterval GetZoneInterval(Instant instant)
    {
      if (this.tailZone != null && instant >= this.tailZoneStart)
      {
        ZoneInterval zoneInterval = this.tailZone.GetZoneInterval(instant);
        return !(zoneInterval.Start < this.tailZoneStart) ? zoneInterval : this.firstTailZoneInterval;
      }
      if (instant == Instant.MaxValue)
        return this.periods[checked (this.periods.Length - 1)];
      int num1 = 0;
      int num2 = this.periods.Length;
      while (num1 < num2)
      {
        int index = checked (num1 + num2) / 2;
        ZoneInterval period = this.periods[index];
        if (period.Start > instant)
        {
          num2 = index;
        }
        else
        {
          if (!(period.End <= instant))
            return period;
          num1 = checked (index + 1);
        }
      }
      throw new InvalidOperationException(string.Format("Instant {0} did not exist in time zone {1}", new object[2]
      {
        (object) instant,
        (object) this.Id
      }));
    }

    public bool IsCachable() => true;

    internal void Write(IDateTimeZoneWriter writer)
    {
      Preconditions.CheckNotNull<IDateTimeZoneWriter>(writer, nameof (writer));
      writer.WriteCount(this.periods.Length);
      Instant? previous = new Instant?();
      foreach (ZoneInterval period in this.periods)
      {
        writer.WriteZoneIntervalTransition(previous, (previous = new Instant?(period.Start)).Value);
        writer.WriteString(period.Name);
        writer.WriteOffset(period.WallOffset);
        writer.WriteOffset(period.Savings);
      }
      writer.WriteZoneIntervalTransition(previous, this.tailZoneStart);
      writer.WriteByte(this.tailZone == null ? (byte) 0 : (byte) 1);
      if (this.tailZone == null)
        return;
      ((DaylightSavingsDateTimeZone) this.tailZone).Write(writer);
    }

    public static DateTimeZone Read(IDateTimeZoneReader reader, string id)
    {
      int length = reader.ReadCount();
      ZoneInterval[] periods = new ZoneInterval[length];
      Instant start = reader.ReadZoneIntervalTransition(new Instant?());
      int index = 0;
      while (index < length)
      {
        string name = reader.ReadString();
        Offset wallOffset = reader.ReadOffset();
        Offset savings = reader.ReadOffset();
        Instant end = reader.ReadZoneIntervalTransition(new Instant?(start));
        periods[index] = new ZoneInterval(name, start, end, wallOffset, savings);
        start = end;
        checked { ++index; }
      }
      DaylightSavingsDateTimeZone savingsDateTimeZone = reader.ReadByte() == (byte) 1 ? DaylightSavingsDateTimeZone.Read(reader, id + "-tail") : (DaylightSavingsDateTimeZone) null;
      return (DateTimeZone) new PrecalculatedDateTimeZone(id, periods, (DateTimeZone) savingsDateTimeZone);
    }

    internal void WriteLegacy(LegacyDateTimeZoneWriter writer)
    {
      Preconditions.CheckNotNull<LegacyDateTimeZoneWriter>(writer, nameof (writer));
      List<string> stringList = new List<string>();
      foreach (ZoneInterval period in this.periods)
      {
        string name = period.Name;
        if (!stringList.Contains(name))
          stringList.Add(name);
      }
      writer.WriteCount(stringList.Count);
      foreach (string str in stringList)
        writer.WriteString(str);
      writer.WriteCount(this.periods.Length);
      Instant? previous = new Instant?();
      foreach (ZoneInterval period in this.periods)
      {
        writer.WriteZoneIntervalTransition(previous, (previous = new Instant?(period.Start)).Value);
        int num = stringList.IndexOf(period.Name);
        if (stringList.Count < 256)
          writer.WriteByte(checked ((byte) num));
        else
          writer.WriteInt32(num);
        writer.WriteOffset(period.WallOffset);
        writer.WriteOffset(period.Savings);
      }
      writer.WriteZoneIntervalTransition(previous, this.tailZoneStart);
      writer.WriteTimeZone(this.tailZone);
    }

    public static DateTimeZone ReadLegacy(LegacyDateTimeZoneReader reader, string id)
    {
      string[] strArray = new string[reader.ReadCount()];
      int index1 = 0;
      while (index1 < strArray.Length)
      {
        strArray[index1] = reader.ReadString();
        checked { ++index1; }
      }
      int length = reader.ReadCount();
      ZoneInterval[] periods = new ZoneInterval[length];
      Instant start = reader.ReadZoneIntervalTransition(new Instant?());
      int index2 = 0;
      while (index2 < length)
      {
        int index3 = strArray.Length < 256 ? (int) reader.ReadByte() : reader.ReadInt32();
        string name = strArray[index3];
        Offset wallOffset = reader.ReadOffset();
        Offset savings = reader.ReadOffset();
        Instant end = reader.ReadZoneIntervalTransition(new Instant?(start));
        periods[index2] = new ZoneInterval(name, start, end, wallOffset, savings);
        start = end;
        checked { ++index2; }
      }
      DateTimeZone tailZone = reader.ReadTimeZone(id + "-tail");
      return (DateTimeZone) new PrecalculatedDateTimeZone(id, periods, tailZone);
    }

    private static Offset ComputeOffset(
      ZoneInterval[] intervals,
      DateTimeZone tailZone,
      PrecalculatedDateTimeZone.OffsetAggregator aggregator)
    {
      Preconditions.CheckNotNull<ZoneInterval[]>(intervals, nameof (intervals));
      Preconditions.CheckArgument(intervals.Length > 0, nameof (intervals), "No intervals specified");
      Offset x = intervals[0].WallOffset;
      int index = 1;
      while (index < intervals.Length)
      {
        x = aggregator(x, intervals[index].WallOffset);
        checked { ++index; }
      }
      if (tailZone != null)
      {
        Offset y = aggregator(tailZone.MinOffset, tailZone.MaxOffset);
        x = aggregator(x, y);
      }
      return x;
    }

    protected override bool EqualsImpl(DateTimeZone zone)
    {
      PrecalculatedDateTimeZone precalculatedDateTimeZone = (PrecalculatedDateTimeZone) zone;
      if (this.Id != precalculatedDateTimeZone.Id || !object.Equals((object) this.tailZone, (object) precalculatedDateTimeZone.tailZone) || this.tailZoneStart != precalculatedDateTimeZone.tailZoneStart || !object.Equals((object) this.firstTailZoneInterval, (object) precalculatedDateTimeZone.firstTailZoneInterval) || this.periods.Length != precalculatedDateTimeZone.periods.Length)
        return false;
      int index = 0;
      while (index < this.periods.Length)
      {
        if (!this.periods[index].Equals(precalculatedDateTimeZone.periods[index]))
          return false;
        checked { ++index; }
      }
      return true;
    }

    public override int GetHashCode()
    {
      int code = HashCodeHelper.Hash<DateTimeZone>(HashCodeHelper.Hash<ZoneInterval>(HashCodeHelper.Hash<Instant>(HashCodeHelper.Hash<string>(HashCodeHelper.Initialize(), this.Id), this.tailZoneStart), this.firstTailZoneInterval), this.tailZone);
      foreach (ZoneInterval period in this.periods)
        code = HashCodeHelper.Hash<ZoneInterval>(code, period);
      return code;
    }

    private delegate Offset OffsetAggregator(Offset x, Offset y);

    private delegate Offset OffsetExtractor<in T>(T input);
  }
}
