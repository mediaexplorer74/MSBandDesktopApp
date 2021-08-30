// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.LogSyncResult
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System.Collections.Generic;

namespace Microsoft.Band.Admin
{
  internal sealed class LogSyncResult
  {
    internal long DownloadedSensorLogBytes { get; set; }

    internal long UploadedSensorLogBytes { get; set; }

    internal double DownloadKbitsPerSecond { get; set; }

    internal double DownloadKbytesPerSecond { get; set; }

    internal double UploadKbitsPerSecond { get; set; }

    internal double UploadKbytesPerSecond { get; set; }

    internal long DevicePendingSensorLogBytes { get; set; }

    internal bool RanToCompletion { get; set; }

    internal long UploadTime { get; set; }

    internal long DownloadTime { get; set; }

    internal List<LogProcessingStatus> LogFilesProcessing { get; set; }
  }
}
