// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.DeviceSettings
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;
using System.Collections.Generic;

namespace Microsoft.Band.Admin
{
  public sealed class DeviceSettings
  {
    public DeviceSettings()
    {
      this.LocaleSettings = CargoLocaleSettings.Default();
      this.AdditionalSettings = new Dictionary<string, string>();
    }

    public Guid DeviceId { get; set; }

    public string SerialNumber { get; set; }

    public string DeviceName { get; set; }

    public int ProfileDeviceVersion { get; set; }

    public DateTime? LastReset { get; set; }

    public DateTime? LastSuccessfulSync { get; set; }

    public CargoLocaleSettings LocaleSettings { get; set; }

    public RunMeasurementUnitType RunDisplayUnits { get; set; }

    public bool TelemetryEnabled { get; set; }

    public Dictionary<string, string> AdditionalSettings { get; set; }

    public byte[] Reserved { get; set; }

    public byte[] FirmwareByteArray { get; set; }
  }
}
