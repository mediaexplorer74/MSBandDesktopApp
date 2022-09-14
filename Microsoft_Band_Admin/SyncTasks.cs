// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.SyncTasks
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;

namespace Microsoft.Band.Admin
{
  [Flags]
  internal enum SyncTasks
  {
    None = 0,
    TimeAndTimeZone = 1,
    EphemerisFile = 2,
    TimeZoneFile = 4,
    DeviceCrashDump = 8,
    DeviceInstrumentation = 16, // 0x00000010
    UserProfileFirmwareBytes = 32, // 0x00000020
    UserProfile = 64, // 0x00000040
    SensorLog = 128, // 0x00000080
    WebTiles = 256, // 0x00000100
    WebTilesForced = 512, // 0x00000200
  }
}
