// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.AppUpdateCheckStatus
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using System;

namespace DesktopSyncApp
{
  public class AppUpdateCheckStatus
  {
    public AppUpdateCheckStatus()
    {
    }

    public AppUpdateCheckStatus(string downloadURL, DateTime lastChecked)
    {
      this.DownloadURL = downloadURL;
      this.LastChecked = lastChecked;
    }

    public string DownloadURL { get; private set; }

    public DateTime LastChecked { get; private set; }
  }
}
