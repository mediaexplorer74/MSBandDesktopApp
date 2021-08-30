// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.FirmwareUpdateInfo
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System.Runtime.Serialization;

namespace Microsoft.Band.Admin
{
  [DataContract]
  internal sealed class FirmwareUpdateInfo : IFirmwareUpdateInfo
  {
    [DataMember]
    internal string DeviceFamily { get; set; }

    [DataMember]
    public string UniqueVersion { get; set; }

    [DataMember]
    public string FirmwareVersion { get; internal set; }

    [DataMember]
    internal string PrimaryUrl { get; set; }

    [DataMember]
    internal string FallbackUrl { get; set; }

    [DataMember]
    internal string MirrorUrl { get; set; }

    [DataMember]
    internal string HashMd5 { get; set; }

    [DataMember]
    internal string SizeInBytes { get; set; }

    [DataMember]
    public bool IsFirmwareUpdateAvailable { get; internal set; }
  }
}
