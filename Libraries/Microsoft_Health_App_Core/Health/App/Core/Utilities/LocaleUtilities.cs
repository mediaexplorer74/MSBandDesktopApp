// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.LocaleUtilities
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Services.Configuration.Dynamic;

namespace Microsoft.Health.App.Core.Utilities
{
  public static class LocaleUtilities
  {
    public static CargoLocaleSettings GetLocaleSettings(
      IOobeConfigurationDefaults defaults)
    {
      CargoLocaleSettings cargoLocaleSettings = CargoLocaleSettings.Default();
      switch (defaults.DistanceUnit)
      {
        case Microsoft.Health.App.Core.Services.Configuration.Dynamic.MeasurementUnitType.Imperial:
          cargoLocaleSettings.DistanceLongUnits = DistanceUnitType.Imperial;
          cargoLocaleSettings.DistanceShortUnits = DistanceUnitType.Imperial;
          break;
        default:
          cargoLocaleSettings.DistanceLongUnits = DistanceUnitType.Metric;
          cargoLocaleSettings.DistanceShortUnits = DistanceUnitType.Metric;
          break;
      }
      switch (defaults.WeightUnit)
      {
        case Microsoft.Health.App.Core.Services.Configuration.Dynamic.MeasurementUnitType.Imperial:
          cargoLocaleSettings.MassUnits = MassUnitType.Imperial;
          break;
        default:
          cargoLocaleSettings.MassUnits = MassUnitType.Metric;
          break;
      }
      switch (defaults.TemperatureUnit)
      {
        case TemperatureUnit.Fahrenheit:
          cargoLocaleSettings.TemperatureUnits = TemperatureUnitType.Imperial;
          break;
        default:
          cargoLocaleSettings.TemperatureUnits = TemperatureUnitType.Metric;
          break;
      }
      return cargoLocaleSettings;
    }
  }
}
