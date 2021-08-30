// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.UploadMetaData
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System;
using System.Runtime.Serialization;

namespace Microsoft.Band.Admin
{
  [DataContract]
  public sealed class UploadMetaData
  {
    public UploadMetaData() => this.CompressionAlgorithm = new LogCompressionAlgorithm?(LogCompressionAlgorithm.uncompressed);

    [DataMember]
    public string HostOS { get; set; }

    [DataMember]
    public string HostOSVersion { get; set; }

    [DataMember]
    public string HostAppVersion { get; set; }

    [DataMember]
    public string DeviceMetadataHint { get; set; }

    [DataMember]
    public string DeviceVersion { get; set; }

    public LogCompressionAlgorithm? CompressionAlgorithm { get; set; }

    [DataMember(Name = "CompressionAlgorithm")]
    private string CompressionAlgorithmString
    {
      get => !this.CompressionAlgorithm.HasValue ? (string) null : this.CompressionAlgorithm.ToString();
      set
      {
        LogCompressionAlgorithm result;
        this.CompressionAlgorithm = Enum.TryParse<LogCompressionAlgorithm>(value, true, out result) ? new LogCompressionAlgorithm?(result) : new LogCompressionAlgorithm?();
      }
    }

    [DataMember]
    public string CompressedFileCRC { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public int? StartSequenceId { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public int? EndSequenceId { get; set; }

    [DataMember]
    public string DeviceId { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public string DeviceSerialNumber { get; set; }

    [DataMember]
    public int? LogVersion { get; set; }

    [DataMember]
    public int? UTCTimeZoneOffsetInMinutes { get; set; }

    [DataMember(EmitDefaultValue = false)]
    public string PcbId { get; set; }
  }
}
