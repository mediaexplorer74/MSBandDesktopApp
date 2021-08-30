// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.WindowsDateTime
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System.Runtime.InteropServices;

namespace Microsoft.Band.Admin
{
  internal static class WindowsDateTime
  {
    private static WindowsDateTime.TimezoneDaylightStatus GetTimeZoneInformation(
      out WindowsDateTime.TIME_ZONE_INFORMATION tzInfo)
    {
      return WindowsDateTime.SafeNativeMethods.GetTimeZoneInformation(out tzInfo).ToTimezoneDaylightStatus();
    }

    private static WindowsDateTime.TimezoneDaylightStatus ToTimezoneDaylightStatus(
      this int result)
    {
      switch (result)
      {
        case 0:
          return WindowsDateTime.TimezoneDaylightStatus.NotApplicable;
        case 1:
          return WindowsDateTime.TimezoneDaylightStatus.Standard;
        case 2:
          return WindowsDateTime.TimezoneDaylightStatus.Daylight;
        default:
          throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
      }
    }

    public static CargoTimeZoneInfo GetWindowsCurrentTimeZone()
    {
      WindowsDateTime.TIME_ZONE_INFORMATION tzInfo;
      int timeZoneInformation = (int) WindowsDateTime.GetTimeZoneInformation(out tzInfo);
      return tzInfo.ToCargoTimeZone();
    }

    public static CargoTimeZoneInfo ToCargoTimeZone(
      this WindowsDateTime.TIME_ZONE_INFORMATION timeZoneInfo)
    {
      CargoTimeZoneInfo cargoTimeZoneInfo = new CargoTimeZoneInfo();
      if (timeZoneInfo.DaylightDate.Day == (short) 0 && timeZoneInfo.DaylightDate.DayOfWeek == (short) 0)
      {
        cargoTimeZoneInfo.Name = timeZoneInfo.StandardName;
        cargoTimeZoneInfo.ZoneOffsetMinutes = (short) -(timeZoneInfo.Bias + timeZoneInfo.StandardBias);
        cargoTimeZoneInfo.DaylightOffsetMinutes = (short) 0;
        cargoTimeZoneInfo.StandardDate = new CargoSystemTime();
        cargoTimeZoneInfo.DaylightDate = new CargoSystemTime();
      }
      else
      {
        cargoTimeZoneInfo.Name = timeZoneInfo.StandardName;
        cargoTimeZoneInfo.ZoneOffsetMinutes = (short) -(timeZoneInfo.Bias + timeZoneInfo.StandardBias);
        cargoTimeZoneInfo.DaylightOffsetMinutes = (short) -timeZoneInfo.DaylightBias;
        cargoTimeZoneInfo.StandardDate = WindowsDateTime.TranslateWindowsDSTTransitionDateToCargo(timeZoneInfo.StandardDate);
        cargoTimeZoneInfo.DaylightDate = WindowsDateTime.TranslateWindowsDSTTransitionDateToCargo(timeZoneInfo.DaylightDate);
      }
      return cargoTimeZoneInfo;
    }

    private static CargoSystemTime TranslateWindowsDSTTransitionDateToCargo(
      WindowsDateTime.SYSTEMTIME windowsSystemTime)
    {
      ushort num1 = 0;
      ushort num2 = 0;
      if (windowsSystemTime.Year == (short) 0)
      {
        num2 = (ushort) ((uint) windowsSystemTime.DayOfWeek + 1U);
        if (windowsSystemTime.Day < (short) 5)
          num1 = (ushort) (((int) windowsSystemTime.Day - 1) * 7 + 1);
      }
      else
        num1 = (ushort) windowsSystemTime.Day;
      return new CargoSystemTime()
      {
        Year = 0,
        Month = (ushort) windowsSystemTime.Month,
        Day = num1,
        DayOfWeek = num2,
        Hour = (ushort) windowsSystemTime.Hour,
        Minute = (ushort) windowsSystemTime.Minute,
        Second = (ushort) windowsSystemTime.Second,
        Milliseconds = (ushort) windowsSystemTime.Milliseconds
      };
    }

    public struct SYSTEMTIME
    {
      [MarshalAs(UnmanagedType.U2)]
      public short Year;
      [MarshalAs(UnmanagedType.U2)]
      public short Month;
      [MarshalAs(UnmanagedType.U2)]
      public short DayOfWeek;
      [MarshalAs(UnmanagedType.U2)]
      public short Day;
      [MarshalAs(UnmanagedType.U2)]
      public short Hour;
      [MarshalAs(UnmanagedType.U2)]
      public short Minute;
      [MarshalAs(UnmanagedType.U2)]
      public short Second;
      [MarshalAs(UnmanagedType.U2)]
      public short Milliseconds;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct TIME_ZONE_INFORMATION
    {
      [MarshalAs(UnmanagedType.I4)]
      public int Bias;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
      public string StandardName;
      public WindowsDateTime.SYSTEMTIME StandardDate;
      [MarshalAs(UnmanagedType.I4)]
      public int StandardBias;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
      public string DaylightName;
      public WindowsDateTime.SYSTEMTIME DaylightDate;
      [MarshalAs(UnmanagedType.I4)]
      public int DaylightBias;
    }

    public enum TimezoneDaylightStatus
    {
      NotApplicable,
      Standard,
      Daylight,
    }

    private static class SafeNativeMethods
    {
      [DllImport("api-ms-win-core-timezone-l1-1-0.dll", SetLastError = true)]
      public static extern int GetTimeZoneInformation(
        out WindowsDateTime.TIME_ZONE_INFORMATION timeZoneInformation);
    }
  }
}
