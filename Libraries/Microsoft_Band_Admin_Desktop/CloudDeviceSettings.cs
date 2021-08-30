// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.CloudDeviceSettings
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Band.Admin
{
  [DataContract]
  internal sealed class CloudDeviceSettings
  {
    [DataMember(EmitDefaultValue = true)]
    internal Guid DeviceId { get; set; }

    [DataMember(EmitDefaultValue = false)]
    internal string SerialNumber { get; set; }

    [DataMember(EmitDefaultValue = false, Name = "FirmwareDeviceName")]
    internal string DeviceName { get; set; }

    [DataMember(EmitDefaultValue = false, Name = "FirmwareProfileVersion")]
    internal int DeviceProfileVersion { get; set; }

    [DataMember(EmitDefaultValue = false)]
    internal string FirmwareByteArray { get; set; }

    [DataMember(EmitDefaultValue = false)]
    internal string FirmwareReserved { get; set; }

    [DataMember(EmitDefaultValue = false)]
    internal DateTime? LastReset { get; set; }

    [DataMember(EmitDefaultValue = false)]
    internal DateTime? LastSuccessfulSync { get; set; }

    [DataMember(EmitDefaultValue = false, Name = "FirmwareLocale")]
    internal CloudLocaleSettings? LocaleSettings { get; set; }

    [DataMember(EmitDefaultValue = true, Name = "RunDisplayUnits")]
    internal byte RunDisplayUnits { get; set; }

    [DataMember(EmitDefaultValue = false, Name = "IsTelemetryEnabled")]
    internal bool? TelemetryEnabled { get; set; }

    [DataMember(EmitDefaultValue = false)]
    internal Dictionary<string, string> AdditionalSettings { get; set; }
  }
}
