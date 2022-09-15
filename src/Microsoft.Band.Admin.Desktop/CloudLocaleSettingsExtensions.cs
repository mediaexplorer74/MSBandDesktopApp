// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.CloudLocaleSettingsExtensions
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System;

namespace Microsoft.Band.Admin
{
  internal static class CloudLocaleSettingsExtensions
  {
    internal static CargoLocaleSettings ToCargoLocaleSettings(
      this CloudLocaleSettings? cloudLocaleSettings)
    {
      return !cloudLocaleSettings.HasValue ? CargoLocaleSettings.Default() : cloudLocaleSettings.Value.ToCargoLocaleSettings();
    }

    internal static CargoLocaleSettings ToCargoLocaleSettings(
      this CloudLocaleSettings cloudLocaleSettings)
    {
      CargoLocaleSettings cargoLocaleSettings = new CargoLocaleSettings()
      {
        LocaleId = (Locale) cloudLocaleSettings.LocaleId,
        LocaleName = cloudLocaleSettings.LocaleName,
        Language = (LocaleLanguage) cloudLocaleSettings.Language
      };
      cargoLocaleSettings.DistanceLongUnits = (DistanceUnitType) CloudLocaleSettingsExtensions.ConvertEnumFromCloud(cargoLocaleSettings.DistanceLongUnits.GetType(), cloudLocaleSettings.DistanceLongUnits);
      cargoLocaleSettings.DistanceShortUnits = (DistanceUnitType) CloudLocaleSettingsExtensions.ConvertEnumFromCloud(cargoLocaleSettings.DistanceShortUnits.GetType(), cloudLocaleSettings.DistanceShortUnits);
      cargoLocaleSettings.EnergyUnits = (EnergyUnitType) CloudLocaleSettingsExtensions.ConvertEnumFromCloud(cargoLocaleSettings.EnergyUnits.GetType(), cloudLocaleSettings.EnergyUnits);
      cargoLocaleSettings.MassUnits = (MassUnitType) CloudLocaleSettingsExtensions.ConvertEnumFromCloud(cargoLocaleSettings.MassUnits.GetType(), cloudLocaleSettings.MassUnits);
      cargoLocaleSettings.TemperatureUnits = (TemperatureUnitType) CloudLocaleSettingsExtensions.ConvertEnumFromCloud(cargoLocaleSettings.TemperatureUnits.GetType(), cloudLocaleSettings.TemperatureUnits);
      cargoLocaleSettings.VolumeUnits = (VolumeUnitType) CloudLocaleSettingsExtensions.ConvertEnumFromCloud(cargoLocaleSettings.VolumeUnits.GetType(), cloudLocaleSettings.VolumeUnits);
      cargoLocaleSettings.TimeFormat = (DisplayTimeFormat) cloudLocaleSettings.TimeFormat;
      cargoLocaleSettings.DateFormat = (DisplayDateFormat) cloudLocaleSettings.DateFormat;
      cargoLocaleSettings.DateSeparator = cloudLocaleSettings.DateSeparator;
      cargoLocaleSettings.NumberSeparator = cloudLocaleSettings.NumberSeparator;
      cargoLocaleSettings.DecimalSeparator = cloudLocaleSettings.DecimalSeparator;
      return cargoLocaleSettings;
    }

    private static int ConvertEnumFromCloud(Type enumType, byte enumValue)
    {
      int int32 = Convert.ToInt32(enumValue);
      if (!Enum.IsDefined(enumType, (object) int32))
      {
        Array values = Enum.GetValues(enumType);
        if (values.Length > 0)
          int32 = Convert.ToInt32(values.GetValue(0));
      }
      return int32;
    }
  }
}
