// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.FirmwareCheckStatus
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;
using System;

namespace DesktopSyncApp
{
  public class FirmwareCheckStatus
  {
    public FirmwareCheckStatus()
    {
    }

    public FirmwareCheckStatus(IFirmwareUpdateInfo info, DateTime lastChecked)
    {
      this.Info = info;
      this.LastChecked = lastChecked;
    }

    public IFirmwareUpdateInfo Info { get; private set; }

    public DateTime LastChecked { get; private set; }
  }
}
