// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.CargoLocaleSettingsExtensions
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System;

namespace Microsoft.Band.Admin
{
  internal static class CargoLocaleSettingsExtensions
  {
    public static CloudLocaleSettings ToCloudLocaleSettings(
      this CargoLocaleSettings cargoLocaleSettings)
    {
      return new CloudLocaleSettings()
      {
        DateFormat = Convert.ToByte((object) cargoLocaleSettings.DateFormat),
        DateSeparator = cargoLocaleSettings.DateSeparator,
        DecimalSeparator = cargoLocaleSettings.DecimalSeparator,
        DistanceLongUnits = Convert.ToByte((object) cargoLocaleSettings.DistanceLongUnits),
        DistanceShortUnits = Convert.ToByte((object) cargoLocaleSettings.DistanceShortUnits),
        EnergyUnits = Convert.ToByte((object) cargoLocaleSettings.EnergyUnits),
        MassUnits = Convert.ToByte((object) cargoLocaleSettings.MassUnits),
        TemperatureUnits = Convert.ToByte((object) cargoLocaleSettings.TemperatureUnits),
        VolumeUnits = Convert.ToByte((object) cargoLocaleSettings.VolumeUnits),
        Language = (ushort) cargoLocaleSettings.Language,
        LocaleId = (ushort) cargoLocaleSettings.LocaleId,
        LocaleName = cargoLocaleSettings.LocaleName,
        NumberSeparator = cargoLocaleSettings.NumberSeparator,
        TimeFormat = cargoLocaleSettings.TimeFormat.ToTimeFormatByte()
      };
    }

    private static byte ToTimeFormatByte(this DisplayTimeFormat format)
    {
      switch (format)
      {
        case DisplayTimeFormat.HHmmss:
          return 1;
        case DisplayTimeFormat.Hmmss:
          return 2;
        case DisplayTimeFormat.hhmmss:
          return 3;
        case DisplayTimeFormat.hmmss:
          return 4;
        default:
          return 0;
      }
    }
  }
}
