// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.FirmwareAppExtensions
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System;

namespace Microsoft.Band.Admin
{
  internal static class FirmwareAppExtensions
  {
    internal static RunningAppType ToRunningAppType(this FirmwareApp firmwareApp)
    {
      switch (firmwareApp)
      {
        case FirmwareApp.OneBL:
          return RunningAppType.OneBL;
        case FirmwareApp.TwoUp:
          return RunningAppType.TwoUp;
        case FirmwareApp.App:
          return RunningAppType.App;
        case FirmwareApp.UpApp:
          return RunningAppType.UpApp;
        case FirmwareApp.Invalid:
          return RunningAppType.Invalid;
        default:
          throw new ArgumentException("Unknown FirmwareApp value.");
      }
    }
  }
}
