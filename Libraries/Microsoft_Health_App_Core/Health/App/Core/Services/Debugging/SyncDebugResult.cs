// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Debugging.SyncDebugResult
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System;

namespace Microsoft.Health.App.Core.Services.Debugging
{
  public class SyncDebugResult
  {
    public string SyncType { get; set; }

    public DateTimeOffset StartTime { get; set; }

    public long SyncElapsed { get; set; }

    public long SdeElapsed { get; set; }

    public long UserProfileFirmwareBytes { get; set; }

    public long UserProfileFull { get; set; }

    public long CloudProcessing { get; set; }

    public long SendPhoneSensorToCloud { get; set; }

    public long TimeZone { get; set; }

    public long EphemerisCheckElapsed { get; set; }

    public long EphemerisUpdateElapsed { get; set; }

    public long CrashDump { get; set; }

    public long WebTiles { get; set; }

    public long TilesUpdate { get; set; }

    public long Goals { get; set; }

    public long Calendar { get; set; }

    public long Finance { get; set; }

    public long Weather { get; set; }

    public long GuidedWorkout { get; set; }

    public long FetchLogFromBand { get; set; }

    public long SendLogToCloud { get; set; }
  }
}
