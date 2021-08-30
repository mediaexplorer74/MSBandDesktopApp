// Decompiled with JetBrains decompiler
// Type: NodaTime.Period
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using JetBrains.Annotations;
using NodaTime.Annotations;
using NodaTime.Fields;
using NodaTime.Text;
using NodaTime.Utility;
using System;
using System.Collections.Generic;

namespace NodaTime
{
  [Immutable]
  public sealed class Period : IEquatable<Period>
  {
    private const int ValuesArraySize = 9;
    private const int YearIndex = 0;
    private const int MonthIndex = 1;
    private const int WeekIndex = 2;
    private const int DayIndex = 3;
    private const int HourIndex = 4;
    private const int MinuteIndex = 5;
    private const int SecondIndex = 6;
    private const int MillisecondIndex = 7;
    private const int TickIndex = 8;
    public static readonly Period Zero = new Period(new long[9]);
    private readonly long ticks;
    private readonly long milliseconds;
    private readonly long seconds;
    private readonly long minutes;
    private readonly long hours;
    private readonly long days;
    private readonly long weeks;
    private readonly long months;
    private readonly long years;

    public static IEqualityComparer<Period> NormalizingEqualityComparer => (IEqualityComparer<Period>) Period.NormalizingPeriodEqualityComparer.Instance;

    private Period(long[] values)
    {
      this.years = values[0];
      this.months = values[1];
      this.weeks = values[2];
      this.days = values[3];
      this.hours = values[4];
      this.minutes = values[5];
      this.seconds = values[6];
      this.milliseconds = values[7];
      this.ticks = values[8];
    }

    internal Period(
      long years,
      long months,
      long weeks,
      long days,
      long hours,
      long minutes,
      long seconds,
      long milliseconds,
      long ticks)
    {
      this.years = years;
      this.months = months;
      this.weeks = weeks;
      this.days = days;
      this.hours = hours;
      this.minutes = minutes;
      this.seconds = seconds;
      this.milliseconds = milliseconds;
      this.ticks = ticks;
    }

    private Period(PeriodUnits periodUnit, long value)
    {
      switch (periodUnit)
      {
        case PeriodUnits.Years:
          this.years = value;
          break;
        case PeriodUnits.Months:
          this.months = value;
          break;
        case PeriodUnits.Weeks:
          this.weeks = value;
          break;
        case PeriodUnits.Days:
          this.days = value;
          break;
        case PeriodUnits.Hours:
          this.hours = value;
          break;
        case PeriodUnits.Minutes:
          this.minutes = value;
          break;
        case PeriodUnits.Seconds:
          this.seconds = value;
          break;
        case PeriodUnits.Milliseconds:
          this.milliseconds = value;
          break;
        case PeriodUnits.Ticks:
          this.ticks = value;
          break;
        default:
          throw new ArgumentException("Unit must be singular", nameof (periodUnit));
      }
    }

    [NotNull]
    public static Period FromYears(long years) => new Period(PeriodUnits.Years, years);

    public static Period FromWeeks(long weeks) => new Period(PeriodUnits.Weeks, weeks);

    public static Period FromMonths(long months) => new Period(PeriodUnits.Months, months);

    public static Period FromDays(long days) => new Period(PeriodUnits.Days, days);

    public static Period FromHours(long hours) => new Period(PeriodUnits.Hours, hours);

    public static Period FromMinutes(long minutes) => new Period(PeriodUnits.Minutes, minutes);

    public static Period FromSeconds(long seconds) => new Period(PeriodUnits.Seconds, seconds);

    public static Period FromMilliseconds(long milliseconds) => new Period(PeriodUnits.Milliseconds, milliseconds);

    public static Period FromTicks(long ticks) => new Period(PeriodUnits.Ticks, ticks);

    public static Period operator +(Period left, Period right)
    {
      Preconditions.CheckNotNull<Period>(left, nameof (left));
      Preconditions.CheckNotNull<Period>(right, nameof (right));
      long[] array = left.ToArray();
      right.AddValuesTo(array);
      return new Period(array);
    }

    public static IComparer<Period> CreateComparer(LocalDateTime baseDateTime) => (IComparer<Period>) new Period.PeriodComparer(baseDateTime);

    private long[] ToArray() => new long[9]
    {
      this.years,
      this.months,
      this.weeks,
      this.days,
      this.hours,
      this.minutes,
      this.seconds,
      this.milliseconds,
      this.ticks
    };

    private void AddValuesTo(long[] values)
    {
      checked { values[0] += this.years; }
      checked { values[1] += this.months; }
      checked { values[2] += this.weeks; }
      checked { values[3] += this.days; }
      checked { values[4] += this.hours; }
      checked { values[5] += this.minutes; }
      checked { values[6] += this.seconds; }
      checked { values[7] += this.milliseconds; }
      checked { values[8] += this.ticks; }
    }

    private void SubtractValuesFrom(long[] values)
    {
      checked { values[0] -= this.years; }
      checked { values[1] -= this.months; }
      checked { values[2] -= this.weeks; }
      checked { values[3] -= this.days; }
      checked { values[4] -= this.hours; }
      checked { values[5] -= this.minutes; }
      checked { values[6] -= this.seconds; }
      checked { values[7] -= this.milliseconds; }
      checked { values[8] -= this.ticks; }
    }

    public static Period operator -(Period minuend, Period subtrahend)
    {
      Preconditions.CheckNotNull<Period>(minuend, nameof (minuend));
      Preconditions.CheckNotNull<Period>(subtrahend, nameof (subtrahend));
      long[] array = minuend.ToArray();
      subtrahend.SubtractValuesFrom(array);
      return new Period(array);
    }

    public static Period Between(LocalDateTime start, LocalDateTime end, PeriodUnits units)
    {
      Preconditions.CheckArgument(units != PeriodUnits.None, nameof (units), "Units must not be empty");
      Preconditions.CheckArgument<PeriodUnits>((units & ~PeriodUnits.AllUnits) == PeriodUnits.None, nameof (units), "Units contains an unknown value: {0}", units);
      CalendarSystem calendar = start.Calendar;
      Preconditions.CheckArgument(calendar.Equals((object) end.Calendar), nameof (end), "start and end must use the same calendar system");
      LocalInstant localInstant1 = start.LocalInstant;
      LocalInstant localInstant2 = end.LocalInstant;
      if (localInstant1 == localInstant2)
        return Period.Zero;
      PeriodFieldSet periodFields = calendar.PeriodFields;
      IPeriodField singleField = Period.GetSingleField(periodFields, units);
      if (singleField != null)
      {
        long num = singleField.Subtract(end.LocalInstant, start.LocalInstant);
        return new Period(units, num);
      }
      long[] values = new long[9];
      LocalInstant localInstant3 = localInstant1;
      int num1 = (int) units;
      int index = 0;
      while (index < 9)
      {
        if ((num1 & 1 << index) != 0)
        {
          IPeriodField fieldForIndex = Period.GetFieldForIndex(periodFields, index);
          values[index] = fieldForIndex.Subtract(localInstant2, localInstant3);
          localInstant3 = fieldForIndex.Add(localInstant3, values[index]);
        }
        checked { ++index; }
      }
      return new Period(values);
    }

    internal LocalInstant AddTo(
      LocalInstant localInstant,
      CalendarSystem calendar,
      int scalar)
    {
      Preconditions.CheckNotNull<CalendarSystem>(calendar, nameof (calendar));
      PeriodFieldSet periodFields = calendar.PeriodFields;
      LocalInstant localInstant1 = localInstant;
      if (this.years != 0L)
        localInstant1 = periodFields.Years.Add(localInstant1, checked (this.years * (long) scalar));
      if (this.months != 0L)
        localInstant1 = periodFields.Months.Add(localInstant1, checked (this.months * (long) scalar));
      if (this.weeks != 0L)
        localInstant1 = periodFields.Weeks.Add(localInstant1, checked (this.weeks * (long) scalar));
      if (this.days != 0L)
        localInstant1 = periodFields.Days.Add(localInstant1, checked (this.days * (long) scalar));
      if (this.hours != 0L)
        localInstant1 = periodFields.Hours.Add(localInstant1, checked (this.hours * (long) scalar));
      if (this.minutes != 0L)
        localInstant1 = periodFields.Minutes.Add(localInstant1, checked (this.minutes * (long) scalar));
      if (this.seconds != 0L)
        localInstant1 = periodFields.Seconds.Add(localInstant1, checked (this.seconds * (long) scalar));
      if (this.milliseconds != 0L)
        localInstant1 = periodFields.Milliseconds.Add(localInstant1, checked (this.milliseconds * (long) scalar));
      if (this.ticks != 0L)
        localInstant1 = periodFields.Ticks.Add(localInstant1, checked (this.ticks * (long) scalar));
      return localInstant1;
    }

    public static Period Between(LocalDateTime start, LocalDateTime end) => Period.Between(start, end, PeriodUnits.DateAndTime);

    public static Period Between(LocalDate start, LocalDate end, PeriodUnits units)
    {
      Preconditions.CheckArgument<PeriodUnits>((units & PeriodUnits.AllTimeUnits) == PeriodUnits.None, nameof (units), "Units contains time units: {0}", units);
      return Period.Between(start.AtMidnight(), end.AtMidnight(), units);
    }

    public static Period Between(LocalDate start, LocalDate end) => Period.Between(start.AtMidnight(), end.AtMidnight(), PeriodUnits.YearMonthDay);

    public static Period Between(LocalTime start, LocalTime end, PeriodUnits units)
    {
      Preconditions.CheckArgument<PeriodUnits>((units & PeriodUnits.AllDateUnits) == PeriodUnits.None, nameof (units), "Units contains date units: {0}", units);
      return Period.Between(start.LocalDateTime, end.LocalDateTime, units);
    }

    public static Period Between(LocalTime start, LocalTime end) => Period.Between(start.LocalDateTime, end.LocalDateTime, PeriodUnits.AllTimeUnits);

    public bool HasTimeComponent => this.hours != 0L || this.minutes != 0L || this.seconds != 0L || this.milliseconds != 0L || this.ticks != 0L;

    public bool HasDateComponent => this.years != 0L || this.months != 0L || this.weeks != 0L || this.days != 0L;

    [Pure]
    public Duration ToDuration()
    {
      if (this.Months != 0L || this.Years != 0L)
        throw new InvalidOperationException("Cannot construct duration of period with non-zero months or years.");
      return Duration.FromTicks(this.TotalStandardTicks);
    }

    private long TotalStandardTicks => checked (this.ticks + this.milliseconds * 10000L + this.seconds * 10000000L + this.minutes * 600000000L + this.hours * 36000000000L + this.days * 864000000000L + this.weeks * 6048000000000L);

    [Pure]
    public PeriodBuilder ToBuilder() => new PeriodBuilder(this);

    [Pure]
    public Period Normalize()
    {
      long totalStandardTicks = this.TotalStandardTicks;
      return new Period(this.years, this.months, 0L, totalStandardTicks / 864000000000L, totalStandardTicks / 36000000000L % 24L, totalStandardTicks / 600000000L % 60L, totalStandardTicks / 10000000L % 60L, totalStandardTicks / 10000L % 1000L, totalStandardTicks % 10000L);
    }

    private static IPeriodField GetSingleField(PeriodFieldSet fields, PeriodUnits units)
    {
      switch (units)
      {
        case PeriodUnits.Years:
          return fields.Years;
        case PeriodUnits.Months:
          return fields.Months;
        case PeriodUnits.Weeks:
          return fields.Weeks;
        case PeriodUnits.Days:
          return fields.Days;
        case PeriodUnits.Hours:
          return fields.Hours;
        case PeriodUnits.Minutes:
          return fields.Minutes;
        case PeriodUnits.Seconds:
          return fields.Seconds;
        case PeriodUnits.Milliseconds:
          return fields.Milliseconds;
        case PeriodUnits.Ticks:
          return fields.Ticks;
        default:
          return (IPeriodField) null;
      }
    }

    private static IPeriodField GetFieldForIndex(PeriodFieldSet fields, int index)
    {
      switch (index)
      {
        case 0:
          return fields.Years;
        case 1:
          return fields.Months;
        case 2:
          return fields.Weeks;
        case 3:
          return fields.Days;
        case 4:
          return fields.Hours;
        case 5:
          return fields.Minutes;
        case 6:
          return fields.Seconds;
        case 7:
          return fields.Milliseconds;
        case 8:
          return fields.Ticks;
        default:
          throw new ArgumentOutOfRangeException(nameof (index));
      }
    }

    public long Years => this.years;

    public long Months => this.months;

    public long Weeks => this.weeks;

    public long Days => this.days;

    public long Hours => this.hours;

    public long Minutes => this.minutes;

    public long Seconds => this.seconds;

    public long Milliseconds => this.milliseconds;

    public long Ticks => this.ticks;

    public override string ToString() => PeriodPattern.RoundtripPattern.Format(this);

    public override bool Equals(object other) => this.Equals(other as Period);

    public override int GetHashCode() => HashCodeHelper.Hash<long>(HashCodeHelper.Hash<long>(HashCodeHelper.Hash<long>(HashCodeHelper.Hash<long>(HashCodeHelper.Hash<long>(HashCodeHelper.Hash<long>(HashCodeHelper.Hash<long>(HashCodeHelper.Hash<long>(HashCodeHelper.Hash<long>(HashCodeHelper.Initialize(), this.years), this.months), this.weeks), this.days), this.hours), this.minutes), this.seconds), this.milliseconds), this.ticks);

    public bool Equals(Period other) => other != null && this.years == other.years && this.months == other.months && this.weeks == other.weeks && this.days == other.days && this.hours == other.hours && this.minutes == other.minutes && this.seconds == other.seconds && this.milliseconds == other.milliseconds && this.ticks == other.ticks;

    private sealed class NormalizingPeriodEqualityComparer : EqualityComparer<Period>
    {
      internal static readonly Period.NormalizingPeriodEqualityComparer Instance = new Period.NormalizingPeriodEqualityComparer();

      private NormalizingPeriodEqualityComparer()
      {
      }

      public override bool Equals(Period x, Period y)
      {
        if (object.ReferenceEquals((object) x, (object) y))
          return true;
        return !object.ReferenceEquals((object) x, (object) null) && !object.ReferenceEquals((object) y, (object) null) && x.Normalize().Equals(y.Normalize());
      }

      public override int GetHashCode(Period obj) => Preconditions.CheckNotNull<Period>(obj, nameof (obj)).Normalize().GetHashCode();
    }

    private sealed class PeriodComparer : Comparer<Period>
    {
      private readonly LocalDateTime baseDateTime;

      internal PeriodComparer(LocalDateTime baseDateTime) => this.baseDateTime = baseDateTime;

      public override int Compare(Period x, Period y)
      {
        if (object.ReferenceEquals((object) x, (object) y))
          return 0;
        if (x == null)
          return -1;
        if (y == null)
          return 1;
        return x.Months == 0L && y.Months == 0L && x.Years == 0L && y.Years == 0L ? x.ToDuration().CompareTo(y.ToDuration()) : (this.baseDateTime + x).CompareTo(this.baseDateTime + y);
      }
    }
  }
}
