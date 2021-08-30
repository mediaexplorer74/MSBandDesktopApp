// Decompiled with JetBrains decompiler
// Type: NodaTime.Calendars.YearStartCacheEntry
// Assembly: NodaTime, Version=1.3.0.0, Culture=neutral, PublicKeyToken=4226afe0d9b296d1
// MVID: AC214B47-4DA1-4E29-B7E9-2BD491A0A6EE
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\NodaTime.dll

namespace NodaTime.Calendars
{
  internal struct YearStartCacheEntry
  {
    private const int CacheIndexBits = 10;
    private const int CacheIndexMask = 1023;
    private const int EntryValidationBits = 7;
    private const int EntryValidationMask = 127;
    private const int CacheSize = 1024;
    internal const int InvalidEntryYear = 64512;
    private static readonly YearStartCacheEntry Invalid = new YearStartCacheEntry(64512, 0);
    private readonly int value;

    internal YearStartCacheEntry(int year, int days) => this.value = days << 7 | YearStartCacheEntry.GetValidator(year);

    internal static YearStartCacheEntry[] CreateCache()
    {
      YearStartCacheEntry[] yearStartCacheEntryArray = new YearStartCacheEntry[1024];
      int index = 0;
      while (index < yearStartCacheEntryArray.Length)
      {
        yearStartCacheEntryArray[index] = YearStartCacheEntry.Invalid;
        checked { ++index; }
      }
      return yearStartCacheEntryArray;
    }

    private static int GetValidator(int year) => year >> 10 & (int) sbyte.MaxValue;

    internal static int GetCacheIndex(int year) => year & 1023;

    internal bool IsValidForYear(int year) => YearStartCacheEntry.GetValidator(year) == (this.value & (int) sbyte.MaxValue);

    internal int StartOfYearDays => this.value >> 7;
  }
}
