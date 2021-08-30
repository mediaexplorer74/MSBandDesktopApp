// Decompiled with JetBrains decompiler
// Type: NodaTime.CalendarSystem
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using JetBrains.Annotations;
using NodaTime.Annotations;
using NodaTime.Calendars;
using NodaTime.Fields;
using NodaTime.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace NodaTime
{
  [Immutable]
  public sealed class CalendarSystem
  {
    private const string GregorianName = "Gregorian";
    private const string IsoName = "ISO";
    private const string CopticName = "Coptic";
    private const string JulianName = "Julian";
    private const string IslamicName = "Hijri";
    private const string PersianName = "Persian";
    private const string HebrewName = "Hebrew";
    private static readonly CalendarSystem[] GregorianCalendarSystems;
    private static readonly CalendarSystem[] CopticCalendarSystems;
    private static readonly CalendarSystem[] JulianCalendarSystems;
    private static readonly CalendarSystem[,] IslamicCalendarSystems;
    private static readonly CalendarSystem IsoCalendarSystem;
    private static readonly CalendarSystem PersianCalendarSystem;
    private static readonly CalendarSystem[] HebrewCalendarSystems;
    private static readonly Dictionary<string, Func<CalendarSystem>> IdToFactoryMap = new Dictionary<string, Func<CalendarSystem>>()
    {
      {
        "ISO",
        (Func<CalendarSystem>) (() => CalendarSystem.Iso)
      },
      {
        "Persian",
        new Func<CalendarSystem>(CalendarSystem.GetPersianCalendar)
      },
      {
        "Hebrew-Civil",
        (Func<CalendarSystem>) (() => CalendarSystem.GetHebrewCalendar(HebrewMonthNumbering.Civil))
      },
      {
        "Hebrew-Scriptural",
        (Func<CalendarSystem>) (() => CalendarSystem.GetHebrewCalendar(HebrewMonthNumbering.Scriptural))
      },
      {
        "Gregorian 1",
        (Func<CalendarSystem>) (() => CalendarSystem.GetGregorianCalendar(1))
      },
      {
        "Gregorian 2",
        (Func<CalendarSystem>) (() => CalendarSystem.GetGregorianCalendar(2))
      },
      {
        "Gregorian 3",
        (Func<CalendarSystem>) (() => CalendarSystem.GetGregorianCalendar(3))
      },
      {
        "Gregorian 4",
        (Func<CalendarSystem>) (() => CalendarSystem.GetGregorianCalendar(4))
      },
      {
        "Gregorian 5",
        (Func<CalendarSystem>) (() => CalendarSystem.GetGregorianCalendar(5))
      },
      {
        "Gregorian 6",
        (Func<CalendarSystem>) (() => CalendarSystem.GetGregorianCalendar(6))
      },
      {
        "Gregorian 7",
        (Func<CalendarSystem>) (() => CalendarSystem.GetGregorianCalendar(7))
      },
      {
        "Coptic 1",
        (Func<CalendarSystem>) (() => CalendarSystem.GetCopticCalendar(1))
      },
      {
        "Coptic 2",
        (Func<CalendarSystem>) (() => CalendarSystem.GetCopticCalendar(2))
      },
      {
        "Coptic 3",
        (Func<CalendarSystem>) (() => CalendarSystem.GetCopticCalendar(3))
      },
      {
        "Coptic 4",
        (Func<CalendarSystem>) (() => CalendarSystem.GetCopticCalendar(4))
      },
      {
        "Coptic 5",
        (Func<CalendarSystem>) (() => CalendarSystem.GetCopticCalendar(5))
      },
      {
        "Coptic 6",
        (Func<CalendarSystem>) (() => CalendarSystem.GetCopticCalendar(6))
      },
      {
        "Coptic 7",
        (Func<CalendarSystem>) (() => CalendarSystem.GetCopticCalendar(7))
      },
      {
        "Julian 1",
        (Func<CalendarSystem>) (() => CalendarSystem.GetJulianCalendar(1))
      },
      {
        "Julian 2",
        (Func<CalendarSystem>) (() => CalendarSystem.GetJulianCalendar(2))
      },
      {
        "Julian 3",
        (Func<CalendarSystem>) (() => CalendarSystem.GetJulianCalendar(3))
      },
      {
        "Julian 4",
        (Func<CalendarSystem>) (() => CalendarSystem.GetJulianCalendar(4))
      },
      {
        "Julian 5",
        (Func<CalendarSystem>) (() => CalendarSystem.GetJulianCalendar(5))
      },
      {
        "Julian 6",
        (Func<CalendarSystem>) (() => CalendarSystem.GetJulianCalendar(6))
      },
      {
        "Julian 7",
        (Func<CalendarSystem>) (() => CalendarSystem.GetJulianCalendar(7))
      },
      {
        "Hijri Civil-Indian",
        (Func<CalendarSystem>) (() => CalendarSystem.GetIslamicCalendar(IslamicLeapYearPattern.Indian, IslamicEpoch.Civil))
      },
      {
        "Hijri Civil-Base15",
        (Func<CalendarSystem>) (() => CalendarSystem.GetIslamicCalendar(IslamicLeapYearPattern.Base15, IslamicEpoch.Civil))
      },
      {
        "Hijri Civil-Base16",
        (Func<CalendarSystem>) (() => CalendarSystem.GetIslamicCalendar(IslamicLeapYearPattern.Base16, IslamicEpoch.Civil))
      },
      {
        "Hijri Civil-HabashAlHasib",
        (Func<CalendarSystem>) (() => CalendarSystem.GetIslamicCalendar(IslamicLeapYearPattern.HabashAlHasib, IslamicEpoch.Civil))
      },
      {
        "Hijri Astronomical-Indian",
        (Func<CalendarSystem>) (() => CalendarSystem.GetIslamicCalendar(IslamicLeapYearPattern.Indian, IslamicEpoch.Astronomical))
      },
      {
        "Hijri Astronomical-Base15",
        (Func<CalendarSystem>) (() => CalendarSystem.GetIslamicCalendar(IslamicLeapYearPattern.Base15, IslamicEpoch.Astronomical))
      },
      {
        "Hijri Astronomical-Base16",
        (Func<CalendarSystem>) (() => CalendarSystem.GetIslamicCalendar(IslamicLeapYearPattern.Base16, IslamicEpoch.Astronomical))
      },
      {
        "Hijri Astronomical-HabashAlHasib",
        (Func<CalendarSystem>) (() => CalendarSystem.GetIslamicCalendar(IslamicLeapYearPattern.HabashAlHasib, IslamicEpoch.Astronomical))
      }
    };
    private readonly YearMonthDayCalculator yearMonthDayCalculator;
    private readonly WeekYearCalculator weekYearCalculator;
    private readonly PeriodFieldSet periodFields;
    private readonly string id;
    private readonly string name;
    private readonly IList<Era> eras;
    private readonly int minYear;
    private readonly int maxYear;
    private readonly long minTicks;
    private readonly long maxTicks;

    static CalendarSystem()
    {
      CalendarSystem.IsoCalendarSystem = new CalendarSystem("ISO", "ISO", (YearMonthDayCalculator) new IsoYearMonthDayCalculator(), 4);
      CalendarSystem.PersianCalendarSystem = new CalendarSystem("Persian", "Persian", (YearMonthDayCalculator) new PersianYearMonthDayCalculator(), 4);
      CalendarSystem.HebrewCalendarSystems = new CalendarSystem[2]
      {
        new CalendarSystem("Hebrew", "Hebrew", (YearMonthDayCalculator) new HebrewYearMonthDayCalculator(HebrewMonthNumbering.Civil), 4),
        new CalendarSystem("Hebrew", "Hebrew", (YearMonthDayCalculator) new HebrewYearMonthDayCalculator(HebrewMonthNumbering.Scriptural), 4)
      };
      CalendarSystem.GregorianCalendarSystems = new CalendarSystem[7];
      CalendarSystem.CopticCalendarSystems = new CalendarSystem[7];
      CalendarSystem.JulianCalendarSystems = new CalendarSystem[7];
      int minDaysInFirstWeek = 1;
      while (minDaysInFirstWeek <= 7)
      {
        CalendarSystem.GregorianCalendarSystems[checked (minDaysInFirstWeek - 1)] = new CalendarSystem("Gregorian", (YearMonthDayCalculator) new GregorianYearMonthDayCalculator(), minDaysInFirstWeek);
        CalendarSystem.CopticCalendarSystems[checked (minDaysInFirstWeek - 1)] = new CalendarSystem("Coptic", (YearMonthDayCalculator) new CopticYearMonthDayCalculator(), minDaysInFirstWeek);
        CalendarSystem.JulianCalendarSystems[checked (minDaysInFirstWeek - 1)] = new CalendarSystem("Julian", (YearMonthDayCalculator) new JulianYearMonthDayCalculator(), minDaysInFirstWeek);
        checked { ++minDaysInFirstWeek; }
      }
      CalendarSystem.IslamicCalendarSystems = new CalendarSystem[4, 2];
      int num1 = 1;
      while (num1 <= 4)
      {
        int num2 = 1;
        while (num2 <= 2)
        {
          IslamicLeapYearPattern islamicLeapYearPattern = (IslamicLeapYearPattern) num1;
          IslamicEpoch islamicEpoch = (IslamicEpoch) num2;
          IslamicYearMonthDayCalculator monthDayCalculator = new IslamicYearMonthDayCalculator((IslamicLeapYearPattern) num1, (IslamicEpoch) num2);
          string id = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0} {1}-{2}", new object[3]
          {
            (object) "Hijri",
            (object) islamicEpoch,
            (object) islamicLeapYearPattern
          });
          CalendarSystem.IslamicCalendarSystems[checked (num1 - 1), checked (num2 - 1)] = new CalendarSystem(id, "Hijri", (YearMonthDayCalculator) monthDayCalculator, 4);
          checked { ++num2; }
        }
        checked { ++num1; }
      }
    }

    public static CalendarSystem ForId(string id)
    {
      Func<CalendarSystem> func;
      if (!CalendarSystem.IdToFactoryMap.TryGetValue(id, out func))
        throw new KeyNotFoundException(string.Format("No calendar system for ID {0} exists", new object[1]
        {
          (object) id
        }));
      return func();
    }

    public static IEnumerable<string> Ids => (IEnumerable<string>) CalendarSystem.IdToFactoryMap.Keys;

    public static CalendarSystem Iso => CalendarSystem.IsoCalendarSystem;

    public static CalendarSystem GetPersianCalendar() => CalendarSystem.PersianCalendarSystem;

    public static CalendarSystem GetHebrewCalendar(HebrewMonthNumbering monthNumbering)
    {
      Preconditions.CheckArgumentRange(nameof (monthNumbering), (int) monthNumbering, 1, 2);
      return CalendarSystem.HebrewCalendarSystems[(int) (monthNumbering - 1)];
    }

    public static CalendarSystem GetGregorianCalendar(int minDaysInFirstWeek)
    {
      Preconditions.CheckArgumentRange(nameof (minDaysInFirstWeek), minDaysInFirstWeek, 1, 7);
      return CalendarSystem.GregorianCalendarSystems[checked (minDaysInFirstWeek - 1)];
    }

    public static CalendarSystem GetJulianCalendar(int minDaysInFirstWeek)
    {
      Preconditions.CheckArgumentRange(nameof (minDaysInFirstWeek), minDaysInFirstWeek, 1, 7);
      return CalendarSystem.JulianCalendarSystems[checked (minDaysInFirstWeek - 1)];
    }

    public static CalendarSystem GetCopticCalendar(int minDaysInFirstWeek)
    {
      Preconditions.CheckArgumentRange(nameof (minDaysInFirstWeek), minDaysInFirstWeek, 1, 7);
      return CalendarSystem.CopticCalendarSystems[checked (minDaysInFirstWeek - 1)];
    }

    public static CalendarSystem GetIslamicCalendar(
      IslamicLeapYearPattern leapYearPattern,
      IslamicEpoch epoch)
    {
      Preconditions.CheckArgumentRange(nameof (leapYearPattern), (int) leapYearPattern, 1, 4);
      Preconditions.CheckArgumentRange(nameof (epoch), (int) epoch, 1, 2);
      return CalendarSystem.IslamicCalendarSystems[(int) (leapYearPattern - 1), (int) (epoch - 1)];
    }

    private CalendarSystem(
      string name,
      YearMonthDayCalculator yearMonthDayCalculator,
      int minDaysInFirstWeek)
      : this(CalendarSystem.CreateIdFromNameAndMinDaysInFirstWeek(name, minDaysInFirstWeek), name, yearMonthDayCalculator, minDaysInFirstWeek)
    {
    }

    private static string CreateIdFromNameAndMinDaysInFirstWeek(string name, int minDaysInFirstWeek) => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0} {1}", new object[2]
    {
      (object) name,
      (object) minDaysInFirstWeek
    });

    private CalendarSystem(
      string id,
      string name,
      YearMonthDayCalculator yearMonthDayCalculator,
      int minDaysInFirstWeek)
    {
      this.id = id;
      this.name = name;
      this.yearMonthDayCalculator = yearMonthDayCalculator;
      this.weekYearCalculator = new WeekYearCalculator(yearMonthDayCalculator, minDaysInFirstWeek);
      this.minYear = yearMonthDayCalculator.MinYear;
      this.maxYear = yearMonthDayCalculator.MaxYear;
      this.minTicks = yearMonthDayCalculator.GetStartOfYearInTicks(this.minYear);
      this.maxTicks = checked (yearMonthDayCalculator.GetStartOfYearInTicks(this.maxYear + 1) - 1L);
      this.eras = (IList<Era>) new ReadOnlyCollection<Era>((IList<Era>) yearMonthDayCalculator.Eras);
      this.periodFields = new PeriodFieldSet.Builder(TimeOfDayCalculator.TimeFields)
      {
        Days = FixedDurationPeriodField.Days,
        Weeks = FixedDurationPeriodField.Weeks,
        Months = ((IPeriodField) new MonthsPeriodField(yearMonthDayCalculator)),
        Years = ((IPeriodField) new YearsPeriodField(yearMonthDayCalculator))
      }.Build();
    }

    public string Id => this.id;

    public string Name => this.name;

    public bool UsesIsoDayOfWeek => true;

    public int MinYear => this.minYear;

    public int MaxYear => this.maxYear;

    internal long MinTicks => this.minTicks;

    internal long MaxTicks => this.maxTicks;

    public IList<Era> Eras => this.eras;

    public int GetAbsoluteYear(int yearOfEra, [NotNull] Era era) => this.GetAbsoluteYear(yearOfEra, this.GetEraIndex(era));

    public int GetMaxYearOfEra([NotNull] Era era) => this.GetMaxYearOfEra(this.GetEraIndex(era));

    public int GetMinYearOfEra([NotNull] Era era) => this.GetMinYearOfEra(this.GetEraIndex(era));

    private int GetEraIndex(Era era)
    {
      Preconditions.CheckNotNull<Era>(era, nameof (era));
      int num = this.Eras.IndexOf(era);
      Preconditions.CheckArgument(num != -1, nameof (era), "Era is not used in this calendar");
      return num;
    }

    internal PeriodFieldSet PeriodFields => this.periodFields;

    internal LocalInstant GetLocalInstant(
      int year,
      int monthOfYear,
      int dayOfMonth,
      int hourOfDay,
      int minuteOfHour,
      int secondOfMinute)
    {
      return this.yearMonthDayCalculator.GetLocalInstant(year, monthOfYear, dayOfMonth).PlusTicks(TimeOfDayCalculator.GetTicks(hourOfDay, minuteOfHour, secondOfMinute));
    }

    internal LocalInstant GetLocalInstant(
      int year,
      int monthOfYear,
      int dayOfMonth,
      int hourOfDay,
      int minuteOfHour)
    {
      return this.yearMonthDayCalculator.GetLocalInstant(year, monthOfYear, dayOfMonth).PlusTicks(TimeOfDayCalculator.GetTicks(hourOfDay, minuteOfHour));
    }

    internal LocalInstant GetLocalInstantFromWeekYearWeekAndDayOfWeek(
      int weekYear,
      int weekOfWeekYear,
      IsoDayOfWeek dayOfWeek)
    {
      return this.weekYearCalculator.GetLocalInstant(weekYear, weekOfWeekYear, dayOfWeek);
    }

    internal LocalInstant GetLocalInstant(
      [NotNull] Era era,
      int yearOfEra,
      int monthOfYear,
      int dayOfMonth)
    {
      return this.yearMonthDayCalculator.GetLocalInstant(era, yearOfEra, monthOfYear, dayOfMonth);
    }

    internal LocalInstant GetLocalInstant(
      int year,
      int monthOfYear,
      int dayOfMonth,
      int hourOfDay,
      int minuteOfHour,
      int secondOfMinute,
      int millisecondOfSecond,
      int tickOfMillisecond)
    {
      return this.yearMonthDayCalculator.GetLocalInstant(year, monthOfYear, dayOfMonth).PlusTicks(TimeOfDayCalculator.GetTicks(hourOfDay, minuteOfHour, secondOfMinute, millisecondOfSecond, tickOfMillisecond));
    }

    public override string ToString() => this.id;

    internal IsoDayOfWeek GetIsoDayOfWeek(LocalInstant localInstant)
    {
      if (!this.UsesIsoDayOfWeek)
        throw new InvalidOperationException("Calendar " + this.id + " does not use ISO days of the week");
      return (IsoDayOfWeek) this.GetDayOfWeek(localInstant);
    }

    public int GetDaysInMonth(int year, int month) => this.yearMonthDayCalculator.GetDaysInMonth(year, month);

    public bool IsLeapYear(int year) => this.yearMonthDayCalculator.IsLeapYear(year);

    public int GetMaxMonth(int year) => this.yearMonthDayCalculator.GetMaxMonth(year);

    internal int GetMaxYearOfEra(int eraIndex) => this.yearMonthDayCalculator.GetMaxYearOfEra(eraIndex);

    internal int GetMinYearOfEra(int eraIndex) => this.yearMonthDayCalculator.GetMinYearOfEra(eraIndex);

    internal int GetAbsoluteYear(int yearOfEra, int eraIndex) => this.yearMonthDayCalculator.GetAbsoluteYear(yearOfEra, eraIndex);

    internal int GetTickOfSecond(LocalInstant localInstant) => TimeOfDayCalculator.GetTickOfSecond(localInstant);

    internal int GetTickOfMillisecond(LocalInstant localInstant) => TimeOfDayCalculator.GetTickOfMillisecond(localInstant);

    internal long GetTickOfDay(LocalInstant localInstant) => TimeOfDayCalculator.GetTickOfDay(localInstant);

    internal int GetMillisecondOfSecond(LocalInstant localInstant) => TimeOfDayCalculator.GetMillisecondOfSecond(localInstant);

    internal int GetMillisecondOfDay(LocalInstant localInstant) => TimeOfDayCalculator.GetMillisecondOfDay(localInstant);

    internal int GetSecondOfMinute(LocalInstant localInstant) => TimeOfDayCalculator.GetSecondOfMinute(localInstant);

    internal int GetSecondOfDay(LocalInstant localInstant) => TimeOfDayCalculator.GetSecondOfDay(localInstant);

    internal int GetMinuteOfHour(LocalInstant localInstant) => TimeOfDayCalculator.GetMinuteOfHour(localInstant);

    internal int GetMinuteOfDay(LocalInstant localInstant) => TimeOfDayCalculator.GetMinuteOfDay(localInstant);

    internal int GetHourOfDay(LocalInstant localInstant) => TimeOfDayCalculator.GetHourOfDay(localInstant);

    internal int GetHourOfHalfDay(LocalInstant localInstant) => TimeOfDayCalculator.GetHourOfHalfDay(localInstant);

    internal int GetClockHourOfHalfDay(LocalInstant localInstant) => TimeOfDayCalculator.GetClockHourOfHalfDay(localInstant);

    internal int GetDayOfWeek(LocalInstant localInstant) => WeekYearCalculator.GetDayOfWeek(localInstant);

    internal int GetDayOfMonth(LocalInstant localInstant) => this.yearMonthDayCalculator.GetDayOfMonth(localInstant);

    internal int GetDayOfYear(LocalInstant localInstant) => this.yearMonthDayCalculator.GetDayOfYear(localInstant);

    internal int GetWeekOfWeekYear(LocalInstant localInstant) => this.weekYearCalculator.GetWeekOfWeekYear(localInstant);

    internal int GetWeekYear(LocalInstant localInstant) => this.weekYearCalculator.GetWeekYear(localInstant);

    internal int GetMonthOfYear(LocalInstant localInstant) => this.yearMonthDayCalculator.GetMonthOfYear(localInstant);

    internal int GetYear(LocalInstant localInstant) => this.yearMonthDayCalculator.GetYear(localInstant);

    internal int GetYearOfCentury(LocalInstant localInstant) => this.yearMonthDayCalculator.GetYearOfCentury(localInstant);

    internal int GetYearOfEra(LocalInstant localInstant) => this.yearMonthDayCalculator.GetYearOfEra(localInstant);

    internal int GetCenturyOfEra(LocalInstant localInstant) => this.yearMonthDayCalculator.GetCenturyOfEra(localInstant);

    internal int GetEra(LocalInstant localInstant) => this.yearMonthDayCalculator.GetEra(localInstant);
  }
}
