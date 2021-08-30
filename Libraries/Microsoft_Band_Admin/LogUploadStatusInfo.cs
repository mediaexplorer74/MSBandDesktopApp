// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.LogUploadStatusInfo
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System.Runtime.Serialization;

namespace Microsoft.Band.Admin
{
  [DataContract]
  internal sealed class LogUploadStatusInfo
  {
    public LogUploadStatusInfo() => this.UploadStatus = LogUploadStatus.Unknown;

    [DataMember(Name = "UploadStatus")]
    public string UploadStatusDeserializer
    {
      set
      {
        if (value == null)
          return;
        this.UploadStatus = LogUploadStatusInfo.LogProcessingResponseContentToUploadStatus(value);
      }
    }

    public LogUploadStatus UploadStatus { get; private set; }

    private static LogUploadStatus LogProcessingResponseContentToUploadStatus(
      string content)
    {
      switch (content.ToLower())
      {
        case "activitiesprocessingdone":
          return LogUploadStatus.ActivitiesProcessingDone;
        case "eventsprocessingblocked":
          return LogUploadStatus.EventsProcessingBlocked;
        case "eventsprocessingdone":
          return LogUploadStatus.EventsProcessingDone;
        case "queuedforetl":
          return LogUploadStatus.QueuedForETL;
        case "uploaddone":
          return LogUploadStatus.UploadDone;
        case "uploadpathsent":
          return LogUploadStatus.UploadPathSent;
        default:
          return LogUploadStatus.Unknown;
      }
    }
  }
}
