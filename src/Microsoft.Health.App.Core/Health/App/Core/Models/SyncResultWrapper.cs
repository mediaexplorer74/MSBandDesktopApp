// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.SyncResultWrapper
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Models
{
  public class SyncResultWrapper
  {
    public SyncResultWrapper(SyncResult syncResult)
    {
      this.DownloadedSensorLogBytes = syncResult.DownloadedSensorLogBytes;
      this.UploadedSensorLogBytes = syncResult.UploadedSensorLogBytes;
      this.DownloadKbitsPerSecond = syncResult.DownloadKbitsPerSecond;
      this.DownloadKbytesPerSecond = syncResult.DownloadKbytesPerSecond;
      this.UploadKbitsPerSecond = syncResult.UploadKbitsPerSecond;
      this.UploadKbytesPerSecond = syncResult.UploadKbytesPerSecond;
      this.TotalTimeElapsed = syncResult.TotalTimeElapsed;
      this.UploadTime = syncResult.UploadTime;
      this.DownloadTime = syncResult.DownloadTime;
      this.BandPendingSensorLogBytes = syncResult.DevicePendingSensorLogBytes;
      this.RanToCompletion = syncResult.RanToCompletion;
      this.LogFilesProcessing = (IList<LogProcessingStatusWrapper>) new List<LogProcessingStatusWrapper>();
      if (syncResult.LogFilesProcessing != null)
      {
        foreach (LogProcessingStatus logProcessingStatus in syncResult.LogFilesProcessing)
          this.LogFilesProcessing.Add(new LogProcessingStatusWrapper(logProcessingStatus));
      }
      this.TimeZoneElapsed = syncResult.TimeZoneElapsed;
      this.EphemerisCheckElapsed = syncResult.EphemerisCheckElapsed;
      this.EphemerisUpdateElapsed = syncResult.EphemerisUpdateElapsed;
      this.CrashDumpElapsed = syncResult.CrashDumpElapsed;
      this.WebTilesElapsed = syncResult.WebTilesElapsed;
      this.SensorLogElapsed = syncResult.SensorLogElapsed;
      this.UserProfileFirmwareBytesElapsed = syncResult.UserProfileFirmwareBytesElapsed;
      this.UserProfileFullElapsed = syncResult.UserProfileFullElapsed;
    }

    public SyncResultWrapper()
    {
    }

    public long DownloadedSensorLogBytes { get; set; }

    public long UploadedSensorLogBytes { get; set; }

    public double DownloadKbitsPerSecond { get; set; }

    public double DownloadKbytesPerSecond { get; set; }

    public double UploadKbitsPerSecond { get; set; }

    public double UploadKbytesPerSecond { get; set; }

    public long TotalTimeElapsed { get; set; }

    public long UploadTime { get; set; }

    public long DownloadTime { get; set; }

    public long BandPendingSensorLogBytes { get; set; }

    public bool RanToCompletion { get; set; }

    public IList<LogProcessingStatusWrapper> LogFilesProcessing { get; set; }

    public long? TimeZoneElapsed { get; set; }

    public long? EphemerisCheckElapsed { get; set; }

    public long? EphemerisUpdateElapsed { get; set; }

    public long? CrashDumpElapsed { get; set; }

    public long? WebTilesElapsed { get; set; }

    public long? SensorLogElapsed { get; set; }

    public long? UserProfileFirmwareBytesElapsed { get; set; }

    public long? UserProfileFullElapsed { get; set; }
  }
}
