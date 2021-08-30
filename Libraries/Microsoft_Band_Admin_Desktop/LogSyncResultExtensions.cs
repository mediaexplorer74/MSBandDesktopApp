// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.LogSyncResultExtensions
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

namespace Microsoft.Band.Admin
{
  internal static class LogSyncResultExtensions
  {
    internal static void CopyToSyncResult(this LogSyncResult logSyncResult, SyncResult syncResult)
    {
      syncResult.DownloadedSensorLogBytes = logSyncResult.DownloadedSensorLogBytes;
      syncResult.UploadedSensorLogBytes = logSyncResult.UploadedSensorLogBytes;
      syncResult.DownloadKbitsPerSecond = logSyncResult.DownloadKbitsPerSecond;
      syncResult.DownloadKbytesPerSecond = logSyncResult.DownloadKbytesPerSecond;
      syncResult.UploadKbitsPerSecond = logSyncResult.UploadKbitsPerSecond;
      syncResult.UploadKbytesPerSecond = logSyncResult.UploadKbytesPerSecond;
      syncResult.DevicePendingSensorLogBytes = logSyncResult.DevicePendingSensorLogBytes;
      syncResult.RanToCompletion = logSyncResult.RanToCompletion;
      syncResult.LogFilesProcessing = logSyncResult.LogFilesProcessing;
      syncResult.UploadTime = logSyncResult.UploadTime;
      syncResult.DownloadTime = logSyncResult.DownloadTime;
    }
  }
}
