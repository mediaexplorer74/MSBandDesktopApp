// Decompiled with JetBrains decompiler
// Type: NodaTime.TimeZones.ZoneYearOffset
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using NodaTime.TimeZones.IO;
using NodaTime.Utility;
using System;

namespace NodaTime.TimeZones
{
  internal sealed class ZoneYearOffset : IEquatable<ZoneYearOffset>
  {
    public static readonly ZoneYearOffset StartOfYear = new ZoneYearOffset(TransitionMode.Wall, 1, 1, 0, false, LocalTime.Midnight);
    private readonly bool advance;
    private readonly int dayOfMonth;
    private readonly int dayOfWeek;
    private readonly TransitionMode mode;
    private readonly int monthOfYear;
    private readonly bool addDay;
    private readonly LocalTime timeOfDay;

    internal ZoneYearOffset(
      TransitionMode mode,
      int monthOfYear,
      int dayOfMonth,
      int dayOfWeek,
      bool advance,
      LocalTime timeOfDay)
      : this(mode, monthOfYear, dayOfMonth, dayOfWeek, advance, timeOfDay, false)
    {
    }

    internal ZoneYearOffset(
      TransitionMode mode,
      int monthOfYear,
      int dayOfMonth,
      int dayOfWeek,
      bool advance,
      LocalTime timeOfDay,
      bool addDay)
    {
      ZoneYearOffset.VerifyFieldValue(1L, 12L, nameof (monthOfYear), (long) monthOfYear, false);
      ZoneYearOffset.VerifyFieldValue(1L, 31L, nameof (dayOfMonth), (long) dayOfMonth, true);
      if (dayOfWeek != 0)
        ZoneYearOffset.VerifyFieldValue(1L, 7L, nameof (dayOfWeek), (long) dayOfWeek, false);
      this.mode = mode;
      this.monthOfYear = monthOfYear;
      this.dayOfMonth = dayOfMonth;
      this.dayOfWeek = dayOfWeek;
      this.advance = advance;
      this.timeOfDay = timeOfDay;
      this.addDay = addDay;
    }

    private static void VerifyFieldValue(
      long minimum,
      long maximum,
      string name,
      long value,
      bool allowNegated)
    {
      bool flag = false;
      if (allowNegated && value < 0L)
      {
        if (value < checked (-maximum) || checked (-minimum) < value)
          flag = true;
      }
      else if (value < minimum || maximum < value)
        flag = true;
      if (flag)
      {
        string str1;
        if (!allowNegated)
          str1 = "[" + (object) minimum + ", " + (object) maximum + "]";
        else
          str1 = "[" + (object) minimum + ", " + (object) maximum + "] or [" + (object) checked (-maximum) + ", " + (object) checked (-minimum) + "]";
        string str2 = str1;
        throw new ArgumentOutOfRangeException(name, name + " is not in the valid range: " + str2);
      }
    }

    public TransitionMode Mode => this.mode;

    public bool AdvanceDayOfWeek => this.advance;

    public LocalTime TimeOfDay => this.timeOfDay;

    internal bool AddDay => this.addDay;

    public bool Equals(ZoneYearOffset other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return this.mode == other.mode && this.monthOfYear == other.monthOfYear && this.dayOfMonth == other.dayOfMonth && this.dayOfWeek == other.dayOfWeek && this.advance == other.advance && this.timeOfDay == other.timeOfDay && this.addDay == other.addDay;
    }

    public static TransitionMode NormalizeModeCharacter(char modeCharacter)
    {
      switch (modeCharacter)
      {
        case 'G':
        case 'U':
        case 'Z':
        case 'g':
        case 'u':
        case 'z':
          return TransitionMode.Utc;
        case 'S':
        case 's':
          return TransitionMode.Standard;
        default:
          return TransitionMode.Wall;
      }
    }

    internal Instant MakeInstant(int year, Offset standardOffset, Offset savings)
    {
      CalendarSystem iso = CalendarSystem.Iso;
      if (year > iso.MaxYear)
        return Instant.MaxValue;
      if (year < iso.MinYear)
        return Instant.MinValue;
      LocalDate localDate = new LocalDate(year, this.monthOfYear, this.dayOfMonth > 0 ? this.dayOfMonth : 1);
      if (this.dayOfMonth < 0)
        localDate = localDate.PlusMonths(1).PlusDays(this.dayOfMonth);
      if (this.dayOfWeek != 0 && this.dayOfWeek != localDate.DayOfWeek)
      {
        IsoDayOfWeek dayOfWeek = (IsoDayOfWeek) this.dayOfWeek;
        localDate = this.advance ? localDate.Next(dayOfWeek) : localDate.Previous(dayOfWeek);
      }
      if (this.addDay)
        localDate = localDate.PlusDays(1);
      return (localDate + this.timeOfDay).LocalInstant.Minus(this.GetOffset(standardOffset, savings));
    }

    internal Instant Next(Instant instant, Offset standardOffset, Offset savings)
    {
      Offset offset = this.GetOffset(standardOffset, savings);
      int eligibleYear = this.GetEligibleYear(CalendarSystem.Iso.GetYear(instant.Plus(offset)), 1);
      Instant instant1 = this.MakeInstant(eligibleYear, standardOffset, savings);
      return !(instant1 > instant) ? this.MakeInstant(this.GetEligibleYear(checked (eligibleYear + 1), 1), standardOffset, savings) : instant1;
    }

    internal Instant Previous(Instant instant, Offset standardOffset, Offset savings)
    {
      Offset offset = this.GetOffset(standardOffset, savings);
      int eligibleYear = this.GetEligibleYear(CalendarSystem.Iso.GetYear(instant.Plus(offset)), -1);
      Instant instant1 = this.MakeInstant(eligibleYear, standardOffset, savings);
      return !(instant1 < instant) ? this.MakeInstant(this.GetEligibleYear(checked (eligibleYear - 1), -1), standardOffset, savings) : instant1;
    }

    private int GetEligibleYear(int year, int direction)
    {
      if (this.dayOfMonth != 29 || this.monthOfYear != 2)
        return year;
      while (!CalendarSystem.Iso.IsLeapYear(year))
        checked { year += direction; }
      return year;
    }

    internal void Write(IDateTimeZoneWriter writer)
    {
      int num = (int) this.mode << 5 | this.dayOfWeek << 2 | (this.advance ? 2 : 0) | (this.addDay ? 1 : 0);
      writer.WriteByte(checked ((byte) num));
      writer.WriteCount(this.monthOfYear);
      writer.WriteSignedCount(this.dayOfMonth);
      writer.WriteOffset(Offset.FromTicks(this.timeOfDay.LocalDateTime.LocalInstant.Ticks));
    }

    internal void WriteLegacy(LegacyDateTimeZoneWriter writer)
    {
      writer.WriteCount((int) this.mode);
      writer.WriteCount(checked (this.monthOfYear + 12));
      writer.WriteCount(checked (this.dayOfMonth + 31));
      writer.WriteCount(checked (this.dayOfWeek + 7));
      writer.WriteBoolean(this.advance);
      writer.WriteOffset(Offset.FromTicks(this.timeOfDay.LocalDateTime.LocalInstant.Ticks));
      writer.WriteBoolean(this.addDay);
    }

    public static ZoneYearOffset Read(IDateTimeZoneReader reader)
    {
      Preconditions.CheckNotNull<IDateTimeZoneReader>(reader, nameof (reader));
      int num = (int) reader.ReadByte();
      TransitionMode mode = (TransitionMode) (num >> 5);
      int dayOfWeek = num >> 2 & 7;
      bool advance = (num & 2) != 0;
      bool addDay = (num & 1) != 0;
      int monthOfYear = reader.ReadCount();
      int dayOfMonth = reader.ReadSignedCount();
      Offset offset = reader.ReadOffset();
      return new ZoneYearOffset(mode, monthOfYear, dayOfMonth, dayOfWeek, advance, new LocalTime(offset.Ticks), addDay);
    }

    public static ZoneYearOffset ReadLegacy(LegacyDateTimeZoneReader reader)
    {
      Preconditions.CheckNotNull<LegacyDateTimeZoneReader>(reader, nameof (reader));
      return new ZoneYearOffset((TransitionMode) reader.ReadCount(), checked (reader.ReadCount() - 12), checked (reader.ReadCount() - 31), checked (reader.ReadCount() - 7), reader.ReadBoolean(), new LocalTime(reader.ReadOffset().Ticks), reader.ReadBoolean());
    }

    private Offset GetOffset(Offset standardOffset, Offset savings)
    {
      switch (this.mode)
      {
        case TransitionMode.Wall:
          return standardOffset + savings;
        case TransitionMode.Standard:
          return standardOffset;
        default:
          return Offset.Zero;
      }
    }

    public override bool Equals(object obj) => this.Equals(obj as ZoneYearOffset);

    public override int GetHashCode() => HashCodeHelper.Hash<bool>(HashCodeHelper.Hash<LocalTime>(HashCodeHelper.Hash<bool>(HashCodeHelper.Hash<int>(HashCodeHelper.Hash<int>(HashCodeHelper.Hash<int>(HashCodeHelper.Hash<TransitionMode>(HashCodeHelper.Initialize(), this.mode), this.monthOfYear), this.dayOfMonth), this.dayOfWeek), this.advance), this.timeOfDay), this.addDay);
  }
}
