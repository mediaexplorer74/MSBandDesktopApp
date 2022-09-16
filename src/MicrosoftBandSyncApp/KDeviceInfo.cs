// Decompiled with JetBrains decompiler
// Type: DesktopSyncApp.KDeviceInfo
// Assembly: Microsoft Band Sync, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 85967930-2DEF-43AB-AC73-6FA058C5AE66
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\MicrosoftBandSync.exe

using Microsoft.Band;
using Microsoft.Band.Admin;
using System;
using System.ComponentModel;

namespace DesktopSyncApp
{
  public class KDeviceInfo : INotifyPropertyChanged
  {
    private FirmwareVersions versions;
    private FirmwareCheckStatus firmwareCheckStatus;

    public event PropertyChangedEventHandler PropertyChanged;

    public KDeviceInfo(
      ViewModel model,
      Guid uniqueID,
      IBandInfo deviceInfo,
      FirmwareVersions versions,
      RunningAppType appType,
      int tries,
      string failures)
    {
      this.UniqueID = uniqueID;
      this.DeviceInfo = deviceInfo;
      this.Versions = versions;
      this.AppType = appType;
      this.Tries = tries;
      this.Failures = failures;
      this.CanReportOOBEComplete = true;
      model.HeartBeat01Sec.Beat += new HeartBeatHandler(this.heartBeat_Beat);
    }

    public bool Loaded { get; set; }

    public Guid UniqueID { get; private set; }

    public IBandInfo DeviceInfo { get; private set; }

    public FirmwareVersions Versions
    {
      get => this.versions;
      set
      {
        if (this.versions == value)
          return;
        this.versions = value;
        this.OnPropertyChanged(nameof (Versions), this.PropertyChanged);
      }
    }

    public bool CanReportOOBEComplete { get; set; }

    public bool IsOOBEComplete { get; set; }

    public RunningAppType AppType { get; set; }

    public DeviceProfileStatus DeviceProfileStatus { get; set; }

    public int Tries { get; private set; }

    public string Failures { get; private set; }

    public FirmwareCheckStatus FirmwareCheckStatus
    {
      get => this.firmwareCheckStatus;
      set
      {
        if (this.firmwareCheckStatus == value)
          return;
        this.firmwareCheckStatus = value;
        this.OnPropertyChanged(nameof (FirmwareCheckStatus), this.PropertyChanged);
        this.OnPropertyChanged("LastFirmwareCheckRelativeTime", this.PropertyChanged);
      }
    }

    public TimeSpan? LastFirmwareCheckRelativeTime => this.FirmwareCheckStatus != null ? new TimeSpan?(DateTime.UtcNow - this.FirmwareCheckStatus.LastChecked) : new TimeSpan?();

    private void heartBeat_Beat(object sender, EventArgs e)
    {
      if (this.FirmwareCheckStatus == null)
        return;
      this.OnPropertyChanged("LastFirmwareCheckRelativeTime", this.PropertyChanged);
    }
  }
}
