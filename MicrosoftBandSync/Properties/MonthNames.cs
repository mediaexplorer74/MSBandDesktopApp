// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.Properties.MonthNames
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;

namespace DesktopSyncApp.Properties
{
  public static class MonthNames
  {
    private static string jan;
    private static string feb;
    private static string mar;
    private static string apr;
    private static string may;
    private static string jun;
    private static string jul;
    private static string aug;
    private static string sep;
    private static string oct;
    private static string nov;
    private static string dec;

    public static string January => MonthNames.jan ?? (MonthNames.jan = MonthNames.FormatMonth(1));

    public static string February => MonthNames.feb ?? (MonthNames.feb = MonthNames.FormatMonth(2));

    public static string March => MonthNames.mar ?? (MonthNames.mar = MonthNames.FormatMonth(3));

    public static string April => MonthNames.apr ?? (MonthNames.apr = MonthNames.FormatMonth(4));

    public static string May => MonthNames.may ?? (MonthNames.may = MonthNames.FormatMonth(5));

    public static string June => MonthNames.jun ?? (MonthNames.jun = MonthNames.FormatMonth(6));

    public static string July => MonthNames.jul ?? (MonthNames.jul = MonthNames.FormatMonth(7));

    public static string August => MonthNames.aug ?? (MonthNames.aug = MonthNames.FormatMonth(8));

    public static string September => MonthNames.sep ?? (MonthNames.sep = MonthNames.FormatMonth(9));

    public static string October => MonthNames.oct ?? (MonthNames.oct = MonthNames.FormatMonth(10));

    public static string November => MonthNames.nov ?? (MonthNames.nov = MonthNames.FormatMonth(11));

    public static string December => MonthNames.dec ?? (MonthNames.dec = MonthNames.FormatMonth(12));

    private static string FormatMonth(int month) => string.Format("{0:MMMM}", (object) new DateTime(1, month, 1));
  }
}
