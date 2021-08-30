// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.CloudDataResource
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System;
using System.Runtime.Serialization;

namespace Microsoft.Band.Admin
{
  [DataContract]
  internal sealed class CloudDataResource
  {
    [DataMember(EmitDefaultValue = false)]
    public string UploadId { get; set; }

    public LogFileTypes LogType { get; set; }

    [DataMember(EmitDefaultValue = false, Name = "LogType")]
    public string LogTypeString
    {
      get => this.LogType == LogFileTypes.Unknown ? (string) null : this.LogType.ToString();
      set
      {
        LogFileTypes result;
        this.LogType = Enum.TryParse<LogFileTypes>(value, true, out result) ? result : LogFileTypes.Unknown;
      }
    }

    [DataMember(EmitDefaultValue = false)]
    public string Location { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public bool Committed { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public UploadMetaData UploadMetadata { get; set; }

    public LogUploadStatus UploadStatus { get; set; }

    [DataMember(EmitDefaultValue = false, Name = "UploadStatus")]
    private string UploadStatusString
    {
      get => this.UploadStatus == LogUploadStatus.Unknown ? (string) null : this.UploadStatus.ToString();
      set
      {
        LogUploadStatus result;
        this.UploadStatus = Enum.TryParse<LogUploadStatus>(value, true, out result) ? result : LogUploadStatus.Unknown;
      }
    }

    [DataMember(EmitDefaultValue = false)]
    public float UploadSizeInKb { get; set; }
  }
}
