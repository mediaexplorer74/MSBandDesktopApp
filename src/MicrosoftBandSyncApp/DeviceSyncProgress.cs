// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.DeviceSyncProgress
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band.Admin;

namespace DesktopSyncApp
{
  public class DeviceSyncProgress : KDKProgress<SyncProgress>
  {
    private double latestProgressPercentage;

    public double LatestProgressPercentage
    {
      get => this.latestProgressPercentage;
      set
      {
        if (this.latestProgressPercentage == value)
          return;
        this.latestProgressPercentage = value;
        this.OnPropertyChanged(nameof (LatestProgressPercentage));
      }
    }

    public override void Report(SyncProgress progress) => this.LatestProgressPercentage = progress.PercentageCompletion;
  }
}
