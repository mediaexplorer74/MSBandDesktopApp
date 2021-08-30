// Decompiled with JetBrains decompiler
// Type: NodaTime.Calendars.JulianYearMonthDayCalculator
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

namespace NodaTime.Calendars
{
  internal sealed class JulianYearMonthDayCalculator : GJYearMonthDayCalculator
  {
    private const long AverageTicksPerJulianYear = 315576000000000;

    internal JulianYearMonthDayCalculator()
      : base(-27256, 31196, 315576000000000L, -621357696000000000L)
    {
    }

    internal override bool IsLeapYear(int year) => (year & 3) == 0;

    protected override int CalculateStartOfYearDays(int year)
    {
      int num1 = checked (year - 1968);
      int num2;
      if (num1 <= 0)
      {
        num2 = checked (num1 + 3) >> 2;
      }
      else
      {
        num2 = num1 >> 2;
        if (!this.IsLeapYear(year))
          checked { ++num2; }
      }
      return checked (num1 * 365 + num2 - 718);
    }
  }
}
