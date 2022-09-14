// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.Math2
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;

namespace DesktopSyncApp
{
  public static class Math2
  {
    public static bool IsBetween<T>(T value, T low, T high) where T : IComparable
    {
      IComparable comparable = (IComparable) value;
      return comparable.CompareTo((object) low) >= 0 && comparable.CompareTo((object) high) <= 0;
    }

    public static T Between<T>(T value, T low, T high) where T : IComparable
    {
      IComparable comparable = (IComparable) value;
      if (comparable.CompareTo((object) low) < 0)
        return low;
      return comparable.CompareTo((object) high) > 0 ? high : value;
    }
  }
}
