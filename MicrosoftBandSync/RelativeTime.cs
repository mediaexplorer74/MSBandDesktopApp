// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.RelativeTime
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;

namespace DesktopSyncApp
{
  public class RelativeTime
  {
    public static string FormatRelativeTime(TimeSpan timeSpan, RelativeTimeFormat format)
    {
      if (timeSpan.TotalSeconds < 5.0)
        return format != RelativeTimeFormat.Long ? RelativeTimeStrings.AMoment_Short : RelativeTimeStrings.AMoment_Long;
      if (timeSpan.TotalSeconds < 50.0)
        return format != RelativeTimeFormat.Long ? RelativeTimeStrings.AFewSeconds_Short : RelativeTimeStrings.AFewSeconds_Long;
      if (timeSpan.TotalMinutes < 50.0)
      {
        int num = (int) Math.Round(timeSpan.TotalMinutes, 0);
        if (num != 1)
          return string.Format(format == RelativeTimeFormat.Long ? RelativeTimeStrings.AboutXMinutes_Long : RelativeTimeStrings.AboutXMinutes_Short, (object) num);
        return format != RelativeTimeFormat.Long ? RelativeTimeStrings.AboutAMinute_Short : RelativeTimeStrings.AboutAMinute_Long;
      }
      if (timeSpan.TotalHours < 22.0)
      {
        int num = (int) Math.Round(timeSpan.TotalHours, 0);
        if (num != 1)
          return string.Format(format == RelativeTimeFormat.Long ? RelativeTimeStrings.AboutXHours_Long : RelativeTimeStrings.AboutXHours_Short, (object) num);
        return format != RelativeTimeFormat.Long ? RelativeTimeStrings.AboutAnHour_Short : RelativeTimeStrings.AboutAnHour_Long;
      }
      if (timeSpan.TotalDays < 6.5)
      {
        int num = (int) Math.Round(timeSpan.TotalDays, 0);
        if (num != 1)
          return string.Format(format == RelativeTimeFormat.Long ? RelativeTimeStrings.AboutXDays_Long : RelativeTimeStrings.AboutXDays_Short, (object) num);
        return format != RelativeTimeFormat.Long ? RelativeTimeStrings.AboutADay_Short : RelativeTimeStrings.AboutADay_Long;
      }
      if (timeSpan.TotalDays < 28.0)
      {
        int num = (int) Math.Round(timeSpan.TotalDays / 7.0, 0);
        if (num != 1)
          return string.Format(format == RelativeTimeFormat.Long ? RelativeTimeStrings.AboutXWeeks_Long : RelativeTimeStrings.AboutXWeeks_Short, (object) num);
        return format != RelativeTimeFormat.Long ? RelativeTimeStrings.AboutAWeek_Short : RelativeTimeStrings.AboutAWeek_Long;
      }
      if (timeSpan.TotalDays < 350.0)
      {
        int num = (int) Math.Round(timeSpan.TotalDays / 30.0, 0);
        if (num != 1)
          return string.Format(format == RelativeTimeFormat.Long ? RelativeTimeStrings.AboutXMonths_Long : RelativeTimeStrings.AboutXMonths_Short, (object) num);
        return format != RelativeTimeFormat.Long ? RelativeTimeStrings.AboutAMonth_Short : RelativeTimeStrings.AboutAMonth_Long;
      }
      return timeSpan.TotalDays < 370.0 ? (format != RelativeTimeFormat.Long ? RelativeTimeStrings.AboutAYear_Short : RelativeTimeStrings.AboutAYear_Long) : (format != RelativeTimeFormat.Long ? RelativeTimeStrings.MoreThanAYear_Short : RelativeTimeStrings.MoreThanAYear_Long);
    }
  }
}
