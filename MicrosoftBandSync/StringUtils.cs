// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.StringUtils
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

namespace DesktopSyncApp
{
  public static class StringUtils
  {
    public static bool IsAllNumeric(this string s)
    {
      foreach (char c in s)
      {
        if (!char.IsDigit(c))
          return false;
      }
      return true;
    }
  }
}
