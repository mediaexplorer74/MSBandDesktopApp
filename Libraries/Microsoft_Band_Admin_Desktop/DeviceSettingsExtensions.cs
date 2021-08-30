// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.DeviceSettingsExtensions
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System;

namespace Microsoft.Band.Admin
{
  internal static class DeviceSettingsExtensions
  {
    internal static CloudDeviceSettings ToCloudDeviceSettings(
      this DeviceSettings deviceSettings,
      int Version)
    {
      CloudDeviceSettings cloudDeviceSettings = new CloudDeviceSettings();
      cloudDeviceSettings.DeviceId = Guid.Empty;
      cloudDeviceSettings.SerialNumber = (string) null;
      cloudDeviceSettings.DeviceProfileVersion = Version;
      cloudDeviceSettings.DeviceName = deviceSettings.DeviceName;
      cloudDeviceSettings.LocaleSettings = new CloudLocaleSettings?(deviceSettings.LocaleSettings.ToCloudLocaleSettings());
      cloudDeviceSettings.LastReset = deviceSettings.LastReset;
      cloudDeviceSettings.LastSuccessfulSync = deviceSettings.LastSuccessfulSync;
      cloudDeviceSettings.TelemetryEnabled = new bool?(deviceSettings.TelemetryEnabled);
      cloudDeviceSettings.RunDisplayUnits = Convert.ToByte((object) deviceSettings.RunDisplayUnits);
      cloudDeviceSettings.AdditionalSettings = deviceSettings.AdditionalSettings;
      if (deviceSettings.FirmwareByteArray != null && deviceSettings.FirmwareByteArray.Length != 0)
        cloudDeviceSettings.FirmwareByteArray = Convert.ToBase64String(deviceSettings.FirmwareByteArray);
      if (deviceSettings.Reserved != null && deviceSettings.Reserved.Length != 0)
        cloudDeviceSettings.FirmwareReserved = Convert.ToBase64String(deviceSettings.Reserved);
      return cloudDeviceSettings;
    }
  }
}
