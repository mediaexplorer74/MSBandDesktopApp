// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Band.BandUserProfile
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Models;
using System;
using System.Globalization;

namespace Microsoft.Health.App.Core.Band
{
  public class BandUserProfile
  {
    private const string AllowMarketingEmail = "AllowMarketingEmail";
    private const string AllowSummaryEmail = "AllowSummaryEmail";

    public BandUserProfile()
    {
    }

    public BandUserProfile(IUserProfile profile, RegionInfo region)
    {
      if (profile == null)
        return;
      this.FirstName = profile.FirstName;
      this.Birthdate = profile.Birthdate;
      this.Gender = profile.Gender;
      this.RestingHeartRate = profile.RestingHeartRate;
      this.HeightMM = profile.Height;
      this.ProfileId = profile.UserID;
      this.Weight = profile.Weight;
      this.ZipCode = profile.ZipCode;
      this.IsOobeCompleted = profile.HasCompletedOOBE;
      if (profile.DeviceSettings != null)
      {
        this.BandName = profile.DeviceSettings.DeviceName;
        this.TelemetryEnabled = profile.DeviceSettings.TelemetryEnabled;
        this.LocaleSettings = profile.DeviceSettings.LocaleSettings;
      }
      bool flag1 = false;
      if (BandUserProfile.IsRegionUS(region))
        flag1 = true;
      bool flag2 = flag1;
      if (profile.ApplicationSettings != null)
      {
        if (profile.ApplicationSettings.AdditionalSettings != null && profile.ApplicationSettings.AdditionalSettings.ContainsKey(nameof (AllowMarketingEmail)))
          flag1 = profile.ApplicationSettings.AdditionalSettings[nameof (AllowMarketingEmail)].ToLower() == "true";
        if (BandUserProfile.IsRegionUS(region))
          flag2 = flag1;
        if (profile.ApplicationSettings.AdditionalSettings != null && profile.ApplicationSettings.AdditionalSettings.ContainsKey(nameof (AllowSummaryEmail)))
          flag2 = profile.ApplicationSettings.AdditionalSettings[nameof (AllowSummaryEmail)].ToLower() == "true";
        this.PreferredLocale = profile.ApplicationSettings.PreferredLocale;
        this.PreferredRegion = profile.ApplicationSettings.PreferredRegion;
      }
      this.MarketingEnabled = flag1;
      this.SummaryEmailEnabled = flag2;
    }

    public string FirstName { get; set; }

    public DateTime Birthdate { get; set; }

    public Gender Gender { get; set; }

    public byte RestingHeartRate { get; set; }

    public ushort HeightMM { get; set; }

    public Guid ProfileId { get; set; }

    public uint Weight { get; set; }

    public string ZipCode { get; set; }

    public string BandName { get; set; }

    public bool TelemetryEnabled { get; set; }

    public bool MarketingEnabled { get; set; }

    public bool SummaryEmailEnabled { get; set; }

    public CargoLocaleSettings LocaleSettings { get; set; }

    public bool IsOobeCompleted { get; set; }

    public string PreferredLocale { get; set; }

    public string PreferredRegion { get; set; }

    public void ApplyToProfile(IUserProfile profile)
    {
      profile.FirstName = this.FirstName;
      profile.Birthdate = this.Birthdate;
      profile.Gender = this.Gender;
      profile.Height = this.HeightMM;
      profile.Weight = this.Weight;
      profile.ZipCode = this.ZipCode;
      profile.HasCompletedOOBE = this.IsOobeCompleted;
      profile.DeviceSettings.DeviceName = this.BandName;
      profile.DeviceSettings.TelemetryEnabled = this.TelemetryEnabled;
      profile.DeviceSettings.LocaleSettings = this.LocaleSettings;
      if (profile.ApplicationSettings == null)
        return;
      if (profile.ApplicationSettings.AdditionalSettings != null)
      {
        bool flag = this.MarketingEnabled;
        string lower1 = flag.ToString().ToLower();
        flag = this.SummaryEmailEnabled;
        string lower2 = flag.ToString().ToLower();
        this.SetAdditionalSetting(profile, "AllowMarketingEmail", lower1);
        this.SetAdditionalSetting(profile, "AllowSummaryEmail", lower2);
      }
      profile.ApplicationSettings.PreferredLocale = this.PreferredLocale;
      profile.ApplicationSettings.PreferredRegion = this.PreferredRegion;
    }

    public void ApplyLocaleSettings(ILocaleSettings localeSettings)
    {
      this.LocaleSettings.LocaleId = localeSettings.Locale;
      this.LocaleSettings.LocaleName = localeSettings.Configuration.LocaleName;
      this.LocaleSettings.DateSeparator = localeSettings.Configuration.DateSeparator;
      this.LocaleSettings.NumberSeparator = localeSettings.Configuration.NumberSeparator;
      this.LocaleSettings.DecimalSeparator = localeSettings.Configuration.DecimalSeparator;
      this.LocaleSettings.TimeFormat = localeSettings.Configuration.DisplayTime;
      this.LocaleSettings.DateFormat = localeSettings.Configuration.DisplayDate;
      this.LocaleSettings.VolumeUnits = localeSettings.Configuration.DisplayVolumeUnit;
      this.LocaleSettings.EnergyUnits = localeSettings.Configuration.DisplayCaloriesUnit;
    }

    private void SetAdditionalSetting(
      IUserProfile profile,
      string settingName,
      string settingValue)
    {
      if (profile.ApplicationSettings.AdditionalSettings.ContainsKey(settingName))
        profile.ApplicationSettings.AdditionalSettings[settingName] = settingValue;
      else
        profile.ApplicationSettings.AdditionalSettings.Add(settingName, settingValue);
    }

    private static bool IsRegionUS(RegionInfo region) => region != null && region.TwoLetterISORegionName == "US";
  }
}
