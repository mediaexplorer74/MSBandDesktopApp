// Decompiled with JetBrains decompiler
// Type: NodaTime.Calendars.RegularYearMonthDayCalculator
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

using System;

namespace NodaTime.Calendars
{
  internal abstract class RegularYearMonthDayCalculator : YearMonthDayCalculator
  {
    private readonly int monthsInYear;

    protected RegularYearMonthDayCalculator(
      int minYear,
      int maxYear,
      int monthsInYear,
      long averageTicksPerYear,
      long ticksAtStartOfYear1,
      params Era[] eras)
      : base(minYear, maxYear, averageTicksPerYear, ticksAtStartOfYear1, eras)
    {
      this.monthsInYear = monthsInYear;
    }

    internal override int GetMaxMonth(int year) => this.monthsInYear;

    internal override LocalInstant AddMonths(LocalInstant localInstant, int months)
    {
      if (months == 0)
        return localInstant;
      long tickOfDay = TimeOfDayCalculator.GetTickOfDay(localInstant);
      int year1 = this.GetYear(localInstant);
      int monthOfYear = this.GetMonthOfYear(localInstant, year1);
      int num1 = checked (monthOfYear - 1 + months);
      int year2;
      int month;
      if (num1 >= 0)
      {
        year2 = checked (year1 + unchecked (num1 / this.monthsInYear));
        month = checked (unchecked (num1 % this.monthsInYear) + 1);
      }
      else
      {
        year2 = checked (year1 + unchecked (num1 / this.monthsInYear) - 1);
        int num2 = Math.Abs(num1) % this.monthsInYear;
        if (num2 == 0)
          num2 = this.monthsInYear;
        month = checked (this.monthsInYear - num2 + 1);
        if (month == 1)
          checked { ++year2; }
      }
      int dayOfMonth = Math.Min(this.GetDayOfMonth(localInstant, year1, monthOfYear), this.GetDaysInMonth(year2, month));
      return new LocalInstant(checked (this.GetYearMonthDayTicks(year2, month, dayOfMonth) + tickOfDay));
    }

    internal override int MonthsBetween(LocalInstant minuendInstant, LocalInstant subtrahendInstant)
    {
      int months = checked ((this.GetYear(minuendInstant) - this.GetYear(subtrahendInstant)) * this.monthsInYear + this.GetMonthOfYear(minuendInstant) - this.GetMonthOfYear(subtrahendInstant));
      LocalInstant localInstant = this.AddMonths(subtrahendInstant, months);
      return subtrahendInstant <= minuendInstant ? (!(localInstant <= minuendInstant) ? checked (months - 1) : months) : (!(localInstant >= minuendInstant) ? checked (months + 1) : months);
    }
  }
}
