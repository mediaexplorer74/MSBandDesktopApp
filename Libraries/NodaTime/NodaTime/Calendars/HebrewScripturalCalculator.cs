// Decompiled with JetBrains decompiler
// Type: NodaTime.Calendars.HebrewScripturalCalculator
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

namespace NodaTime.Calendars
{
  internal static class HebrewScripturalCalculator
  {
    internal const int MaxYear = 20000;
    internal const int MinYear = 1;
    private const int ElapsedDaysCacheMask = 8388607;
    private const int IsHeshvanLongCacheBit = 8388608;
    private const int IsKislevShortCacheBit = 16777216;
    private static readonly YearStartCacheEntry[] YearCache = YearStartCacheEntry.CreateCache();

    internal static bool IsLeapYear(int year) => checked (year * 7 + 1) % 19 < 7;

    private static int MonthsInYear(int year) => !HebrewScripturalCalculator.IsLeapYear(year) ? 12 : 13;

    internal static int DaysInMonth(int year, int month)
    {
      switch (month)
      {
        case 2:
        case 4:
        case 6:
        case 10:
        case 13:
          return 29;
        case 8:
          return !HebrewScripturalCalculator.IsHeshvanLong(year) ? 29 : 30;
        case 9:
          return !HebrewScripturalCalculator.IsKislevShort(year) ? 30 : 29;
        case 12:
          return !HebrewScripturalCalculator.IsLeapYear(year) ? 29 : 30;
        default:
          return 30;
      }
    }

    private static bool IsHeshvanLong(int year) => (HebrewScripturalCalculator.GetOrPopulateCache(year) & 8388608) != 0;

    private static bool IsKislevShort(int year) => (HebrewScripturalCalculator.GetOrPopulateCache(year) & 16777216) != 0;

    private static int ElapsedDays(int year) => HebrewScripturalCalculator.GetOrPopulateCache(year) & 8388607;

    private static int ElapsedDaysNoCache(int year)
    {
      int num1 = checked (235 * unchecked (checked (year - 1) / 19) + 12 * unchecked (checked (year - 1) % 19) + unchecked (checked (unchecked (checked (year - 1) % 19) * 7 + 1) / 19));
      int num2 = checked (204 + 793 * unchecked (num1 % 1080));
      int num3 = checked (5 + 12 * num1 + 793 * unchecked (num1 / 1080) + unchecked (num2 / 1080));
      int num4 = checked (1 + 29 * num1 + unchecked (num3 / 24));
      int num5 = checked (unchecked (num3 % 24) * 1080 + unchecked (num2 % 1080));
      int num6 = num5 >= 19440 || num4 % 7 == 2 && num5 >= 9924 && !HebrewScripturalCalculator.IsLeapYear(year) || num4 % 7 == 1 && num5 >= 16789 && HebrewScripturalCalculator.IsLeapYear(checked (year - 1)) ? checked (1 + num4) : num4;
      switch (num6 % 7)
      {
        case 0:
        case 3:
        case 5:
          return checked (num6 + 1);
        default:
          return num6;
      }
    }

    private static int GetOrPopulateCache(int year)
    {
      if (year < 1 || year > 20000)
        return HebrewScripturalCalculator.ComputeCacheEntry(year);
      int cacheIndex = YearStartCacheEntry.GetCacheIndex(year);
      YearStartCacheEntry yearStartCacheEntry = HebrewScripturalCalculator.YearCache[cacheIndex];
      if (!yearStartCacheEntry.IsValidForYear(year))
      {
        int cacheEntry = HebrewScripturalCalculator.ComputeCacheEntry(year);
        yearStartCacheEntry = new YearStartCacheEntry(year, cacheEntry);
        HebrewScripturalCalculator.YearCache[cacheIndex] = yearStartCacheEntry;
      }
      return yearStartCacheEntry.StartOfYearDays;
    }

    private static int ComputeCacheEntry(int year)
    {
      int num1 = HebrewScripturalCalculator.ElapsedDaysNoCache(year);
      int year1 = checked (year + 1);
      int num2;
      if (year1 <= 20000)
      {
        int cacheIndex = YearStartCacheEntry.GetCacheIndex(year1);
        YearStartCacheEntry yearStartCacheEntry = HebrewScripturalCalculator.YearCache[cacheIndex];
        num2 = yearStartCacheEntry.IsValidForYear(year1) ? yearStartCacheEntry.StartOfYearDays & 8388607 : HebrewScripturalCalculator.ElapsedDaysNoCache(year1);
      }
      else
        num2 = HebrewScripturalCalculator.ElapsedDaysNoCache(year);
      int num3 = checked (num2 - num1);
      bool flag1 = num3 % 10 == 5;
      bool flag2 = num3 % 10 == 3;
      return num1 | (flag1 ? 8388608 : 0) | (flag2 ? 16777216 : 0);
    }

    internal static int DaysInYear(int year) => checked (HebrewScripturalCalculator.ElapsedDays(year + 1) - HebrewScripturalCalculator.ElapsedDays(year));

    internal static int AbsoluteFromHebrew(int year, int month, int day)
    {
      int num1 = checked (HebrewScripturalCalculator.ElapsedDays(year) + day - 1373429);
      if (month < 7)
      {
        int num2 = HebrewScripturalCalculator.MonthsInYear(year);
        int month1 = 7;
        while (month1 <= num2)
        {
          checked { num1 += HebrewScripturalCalculator.DaysInMonth(year, month1); }
          checked { ++month1; }
        }
        int month2 = 1;
        while (month2 < month)
        {
          checked { num1 += HebrewScripturalCalculator.DaysInMonth(year, month2); }
          checked { ++month2; }
        }
      }
      else
      {
        int month3 = 7;
        while (month3 < month)
        {
          checked { num1 += HebrewScripturalCalculator.DaysInMonth(year, month3); }
          checked { ++month3; }
        }
      }
      return num1;
    }

    internal static YearMonthDay HebrewFromAbsolute(int days)
    {
      int year = checked (days + 1373429) / 366;
      while (days >= HebrewScripturalCalculator.AbsoluteFromHebrew(checked (year + 1), 7, 1))
        checked { ++year; }
      int month = days < HebrewScripturalCalculator.AbsoluteFromHebrew(year, 1, 1) ? 7 : 1;
      while (days > HebrewScripturalCalculator.AbsoluteFromHebrew(year, month, HebrewScripturalCalculator.DaysInMonth(year, month)))
        checked { ++month; }
      int day = checked (days - HebrewScripturalCalculator.AbsoluteFromHebrew(year, month, 1) - 1);
      return new YearMonthDay(year, month, day);
    }
  }
}
