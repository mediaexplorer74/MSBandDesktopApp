// Decompiled with JetBrains decompiler
// Type: NodaTime.Calendars.HebrewYearMonthDayCalculator
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using System;

namespace NodaTime.Calendars
{
  internal sealed class HebrewYearMonthDayCalculator : YearMonthDayCalculator
  {
    private const int ScripturalYearStartMonth = 7;
    private const int AbsoluteDayOfUnixEpoch = 719163;
    private const int AbsoluteDayOfHebrewEpoch = -1373427;
    private const int MonthsPerLeapCycle = 235;
    private const int YearsPerLeapCycle = 19;
    private readonly Func<int, int, int> calendarToScriptural;
    private readonly Func<int, int, int> scripturalToCalendar;

    internal HebrewYearMonthDayCalculator(HebrewMonthNumbering monthNumbering)
      : base(1, 20000, 315705600000000L, -1807997760000000000L, new Era[1]
      {
        Era.AnnoMundi
      })
    {
      switch (monthNumbering)
      {
        case HebrewMonthNumbering.Civil:
          this.calendarToScriptural = new Func<int, int, int>(HebrewMonthConverter.CivilToScriptural);
          this.scripturalToCalendar = new Func<int, int, int>(HebrewMonthConverter.ScripturalToCivil);
          break;
        case HebrewMonthNumbering.Scriptural:
          this.calendarToScriptural = new Func<int, int, int>(HebrewYearMonthDayCalculator.NoOp);
          this.scripturalToCalendar = new Func<int, int, int>(HebrewYearMonthDayCalculator.NoOp);
          break;
      }
    }

    private static int NoOp(int year, int month) => month;

    internal override bool IsLeapYear(int year) => HebrewScripturalCalculator.IsLeapYear(year);

    protected override long GetTicksFromStartOfYearToStartOfMonth(int year, int month)
    {
      int month1 = this.calendarToScriptural(year, month);
      return checked ((long) (HebrewScripturalCalculator.AbsoluteFromHebrew(year, month1, 1) - HebrewScripturalCalculator.AbsoluteFromHebrew(year, 7, 1)) * 864000000000L);
    }

    protected override int CalculateStartOfYearDays(int year) => checked (HebrewScripturalCalculator.AbsoluteFromHebrew(year, 7, 1) - 719163);

    internal override int GetMonthOfYear(LocalInstant localInstant)
    {
      YearMonthDay yearMonthDay = HebrewScripturalCalculator.HebrewFromAbsolute(HebrewYearMonthDayCalculator.AbsoluteDayFromLocalInstant(localInstant));
      return this.scripturalToCalendar(yearMonthDay.Year, yearMonthDay.Month);
    }

    internal override int GetDaysInYear(int year) => HebrewScripturalCalculator.DaysInYear(year);

    protected override long GetTicksInYear(int year) => checked ((long) this.GetDaysInYear(year) * 864000000000L);

    internal override int GetDayOfMonth(LocalInstant localInstant) => HebrewScripturalCalculator.HebrewFromAbsolute(HebrewYearMonthDayCalculator.AbsoluteDayFromLocalInstant(localInstant)).Day;

    protected override int GetMonthOfYear(LocalInstant localInstant, int year) => this.GetMonthOfYear(localInstant);

    internal override int GetMaxMonth(int year) => !this.IsLeapYear(year) ? 12 : 13;

    internal override LocalInstant SetYear(LocalInstant localInstant, int year)
    {
      long tickOfDay = TimeOfDayCalculator.GetTickOfDay(localInstant);
      YearMonthDay yearMonthDay = HebrewScripturalCalculator.HebrewFromAbsolute(HebrewYearMonthDayCalculator.AbsoluteDayFromLocalInstant(localInstant));
      int day = yearMonthDay.Day;
      int month = yearMonthDay.Month;
      if (month == 13 && !this.IsLeapYear(year))
        month = 12;
      else if (month == 12 && this.IsLeapYear(year) && !this.IsLeapYear(yearMonthDay.Year))
        month = 13;
      if (day == 30 && (month == 8 || month == 9 || month == 12) && HebrewScripturalCalculator.DaysInMonth(year, month) != 30)
      {
        day = 1;
        checked { ++month; }
        if (month == 13)
          month = 1;
      }
      return HebrewYearMonthDayCalculator.LocalInstantFromAbsoluteDay(HebrewScripturalCalculator.AbsoluteFromHebrew(year, month, day), tickOfDay);
    }

    internal override int GetDaysInMonth(int year, int month) => HebrewScripturalCalculator.DaysInMonth(year, this.calendarToScriptural(year, month));

    internal override LocalInstant AddMonths(LocalInstant localInstant, int months)
    {
      if (months == 0)
        return localInstant;
      long tickOfDay = TimeOfDayCalculator.GetTickOfDay(localInstant);
      YearMonthDay yearMonthDay = HebrewScripturalCalculator.HebrewFromAbsolute(HebrewYearMonthDayCalculator.AbsoluteDayFromLocalInstant(localInstant));
      int year1 = yearMonthDay.Year;
      int civil = HebrewMonthConverter.ScripturalToCivil(year1, yearMonthDay.Month);
      int year2 = checked (year1 + unchecked (months / 235) * 19);
      months %= 235;
      int month;
      if (months > 0)
      {
        checked { months += civil - 1; }
        while (months >= this.GetMaxMonth(year2))
        {
          checked { months -= this.GetMaxMonth(year2); }
          checked { ++year2; }
        }
        month = checked (months + 1);
      }
      else
      {
        checked { months -= this.GetMaxMonth(year2) - civil; }
        while (checked (months + this.GetMaxMonth(year2)) <= 0)
        {
          checked { months += this.GetMaxMonth(year2); }
          checked { --year2; }
        }
        month = checked (this.GetMaxMonth(year2) + months);
      }
      int scriptural = HebrewMonthConverter.CivilToScriptural(year2, month);
      int day = Math.Min(HebrewScripturalCalculator.DaysInMonth(year2, scriptural), yearMonthDay.Day);
      return HebrewYearMonthDayCalculator.LocalInstantFromAbsoluteDay(HebrewScripturalCalculator.AbsoluteFromHebrew(year2, scriptural, day), tickOfDay);
    }

    internal override int MonthsBetween(LocalInstant minuendInstant, LocalInstant subtrahendInstant)
    {
      YearMonthDay yearMonthDay1 = HebrewScripturalCalculator.HebrewFromAbsolute(HebrewYearMonthDayCalculator.AbsoluteDayFromLocalInstant(minuendInstant));
      YearMonthDay yearMonthDay2 = HebrewScripturalCalculator.HebrewFromAbsolute(HebrewYearMonthDayCalculator.AbsoluteDayFromLocalInstant(subtrahendInstant));
      int months = checked ((int) unchecked ((double) checked (yearMonthDay1.Year * 235) / 19.0 + (double) HebrewMonthConverter.ScripturalToCivil(yearMonthDay1.Year, yearMonthDay1.Month) - (double) checked (yearMonthDay2.Year * 235) / 19.0 + (double) HebrewMonthConverter.ScripturalToCivil(yearMonthDay2.Year, yearMonthDay2.Month)));
      if (subtrahendInstant <= minuendInstant)
      {
        while (this.AddMonths(subtrahendInstant, months) > minuendInstant)
          checked { --months; }
        while (this.AddMonths(subtrahendInstant, months) <= minuendInstant)
          checked { ++months; }
        return checked (months - 1);
      }
      while (this.AddMonths(subtrahendInstant, months) < minuendInstant)
        checked { ++months; }
      while (this.AddMonths(subtrahendInstant, months) >= minuendInstant)
        checked { --months; }
      return checked (months + 1);
    }

    private static int AbsoluteDayFromLocalInstant(LocalInstant localInstant) => checked (TickArithmetic.TicksToDays(localInstant.Ticks) + 719163);

    private static LocalInstant LocalInstantFromAbsoluteDay(
      int absoluteDay,
      long tickOfDay)
    {
      return new LocalInstant(checked ((long) (absoluteDay - 719163) * 864000000000L + tickOfDay));
    }
  }
}
