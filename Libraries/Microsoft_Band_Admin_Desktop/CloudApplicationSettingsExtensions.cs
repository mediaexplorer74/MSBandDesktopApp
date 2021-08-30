// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.CloudApplicationSettingsExtensions
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System;

namespace Microsoft.Band.Admin
{
  internal static class CloudApplicationSettingsExtensions
  {
    internal static ApplicationSettings ToApplicationSettings(
      this CloudApplicationSettings cloudApplicationSettings)
    {
      ApplicationSettings applicationSettings = new ApplicationSettings();
      Guid? nullable = cloudApplicationSettings.ApplicationId;
      applicationSettings.ApplicationId = nullable ?? Guid.Empty;
      nullable = cloudApplicationSettings.PairedDeviceId;
      applicationSettings.PairedDeviceId = nullable ?? Guid.Empty;
      applicationSettings.Locale = cloudApplicationSettings.Locale;
      applicationSettings.AllowPersonalization = cloudApplicationSettings.AllowPersonalization;
      applicationSettings.AllowToRunInBackground = cloudApplicationSettings.AllowToRunInBackground;
      applicationSettings.SyncInterval = cloudApplicationSettings.SyncInterval;
      applicationSettings.AvatarFileURL = cloudApplicationSettings.AvatarFileURL;
      applicationSettings.HomeScreenWallpaperURL = cloudApplicationSettings.HomeScreenWallpaperURL;
      applicationSettings.ThemeColor = cloudApplicationSettings.ThemeColor;
      applicationSettings.TemperatureDisplayType = cloudApplicationSettings.TemperatureDisplayType;
      applicationSettings.MeasurementDisplayType = cloudApplicationSettings.MeasurementDisplayType;
      applicationSettings.AdditionalSettings = cloudApplicationSettings.AdditionalSettings;
      applicationSettings.ThirdPartyPartnersPortalEndpoint = cloudApplicationSettings.ThirdPartyPartnersPortalEndpoint;
      applicationSettings.PreferredLocale = cloudApplicationSettings.PreferredLocale;
      applicationSettings.PreferredRegion = cloudApplicationSettings.PreferredRegion;
      return applicationSettings;
    }
  }
}
