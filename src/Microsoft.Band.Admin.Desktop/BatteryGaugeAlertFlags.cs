// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.BatteryGaugeAlertFlags
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System;

namespace Microsoft.Band.Admin
{
  [Flags]
  internal enum BatteryGaugeAlertFlags : ushort
  {
    LowVoltage = 1,
    CriticalVoltage = 2,
    TerminationVoltage = 4,
    WirelessFWUpdateAllowed = 8,
    MotorNotAllowed = 16, // 0x0010
    SampleNotAvailable = 32, // 0x0020
  }
}
