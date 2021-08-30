// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.LogProcessingStatusWrapper
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;

namespace Microsoft.Health.App.Core.Models
{
  public class LogProcessingStatusWrapper
  {
    public LogProcessingStatusWrapper()
    {
    }

    public LogProcessingStatusWrapper(LogProcessingStatus logProcessingStatus)
    {
      this.FileType = logProcessingStatus.FileType;
      this.UploadId = logProcessingStatus.UploadId;
      this.LogProcessingStatus = logProcessingStatus;
    }

    public LogFileTypes FileType { get; set; }

    public string UploadId { get; set; }

    public LogProcessingStatus LogProcessingStatus { get; set; }
  }
}
