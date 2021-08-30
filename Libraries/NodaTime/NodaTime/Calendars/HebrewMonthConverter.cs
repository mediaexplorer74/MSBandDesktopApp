// Decompiled with JetBrains decompiler
// Type: NodaTime.Calendars.HebrewMonthConverter
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

namespace NodaTime.Calendars
{
  internal static class HebrewMonthConverter
  {
    internal static int CivilToScriptural(int year, int month)
    {
      if (month < 7)
        return checked (month + 6);
      bool flag = HebrewScripturalCalculator.IsLeapYear(year);
      return month == 7 ? (!flag ? 1 : 13) : (!flag ? checked (month - 6) : checked (month - 7));
    }

    internal static int ScripturalToCivil(int year, int month)
    {
      if (month >= 7)
        return checked (month - 6);
      return !HebrewScripturalCalculator.IsLeapYear(year) ? checked (month + 6) : checked (month + 7);
    }
  }
}
