// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.LogProcessingStatus
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;

namespace Microsoft.Band.Admin
{
  public sealed class LogProcessingStatus
  {
    public LogProcessingStatus(string uploadId, DateTime knownStatus)
    {
      this.UploadId = uploadId;
      this.KnownStatus = knownStatus;
    }

    public LogFileTypes FileType => LogFileTypes.Sensor;

    public string UploadId { get; private set; }

    public DateTime KnownStatus { get; set; }
  }
}
