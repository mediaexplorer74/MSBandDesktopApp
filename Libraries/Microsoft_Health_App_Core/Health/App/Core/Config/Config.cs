// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Config.Config
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Sync;
using Microsoft.Health.App.Core.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Config
{
  public class Config : IConfig
  {
    private const string EnvironmentKey = "Environment";
    private const string UseOAuthKey = "UseOAuth";
    private const string EnabledTilesKey = "EnabledStrapps";
    private const string EnabledTilesMatchesBandKey = "EnabledStrappsMatchesDevice";
    private const string BackgroundLoggingKey = "BackgroundLogging";
    private const string BackgroundRefreshPendingKey = "BackgroundRefreshPending";
    private const string CachingKey = "Caching";
    private const string MockInsightsKey = "MockInsights";
    private const string CustomEnvironmentUrlKey = "CustomEnvironmentUrl";
    private const string ApiVersionKey = "ApiVersion";
    private const string LastSentInsightIdKey = "LastSentInsightId";
    private const string ViewedEventsJsonKey = "ViewedEventsJson";
    private const string LastCompletedFullOobeKey = "LastCompletedFullOobe";
    private const string LastCompletedRefreshKey = "LastCompletedRefresh";
    private const string IsFirmwareUpdateCheckingEnabledKey = "IsFirmwareUpdateCheckingEnabled";
    private const string IsForegroundSyncOnAppStartupEnabledKey = "IsForegroundSyncOnAppStartupEnabled";
    private const string UserStocksKey = "RecoveredUserStocks";
    private const string CachedUserVipsKey = "CachedUserVips";
    private const string IsCachedUserVipsEnabledKey = "IsCachedUserVipsEnabled";
    private const string LastRegisteredBackgroundVersionKey = "LastRegisteredBackgroundVersion";
    private const string IsNavigatedAwayDuringFirmwareUpdatePromptNeededKey = "IsNavigatedAwayDuringFirmwareUpdatePromptNeeded";
    private const string TotalTimesSyncedKey = "TotalTimesSynced";
    private const string GWTimeProgressedDaysKey = "GWTimeProgressedDays";
    private const string IsStarbucksEnabledKey = "IsStarbucksEnabled";
    private const string StarbucksCardNumberKey = "StarbucksCardNumber";
    private const string SyncFrequencyKey = "SyncFrequency";
    private const SyncFrequency DefaultBackgroundSyncFrequency = SyncFrequency.ThirtyMinutes;
    private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings()
    {
      ContractResolver = (IContractResolver) new LocaleContractResolver()
    };
    private const string LiveTileEnabledKey = "PhoneSettings.LiveTileEnabled";
    private const bool DefaultLiveTileEnabledSetting = true;
    private const string TransparentLiveTileEnabledKey = "PhoneSettings.TransparentLiveTileEnabled";
    private const bool DefaultTransparentLiveTileEnabledSetting = true;
    private const string StepsTileEnabledKey = "PhoneSettings.StepsTileEnabled";
    private const bool DefaultStepsTileEnabledSetting = true;
    private const string CaloriesTileEnabledKey = "PhoneSettings.CaloriesTileEnabled";
    private const bool DefaultCaloriesTileEnabledSetting = true;
    private const string BatteryTileEnabledKey = "PhoneSettings.BatteryTileEnabled";
    private const bool DefaultBatteryTileEnabledSetting = true;
    private const string LastEventTileEnabledKey = "PhoneSettings.LastEventTileEnabled";
    private const bool DefaultLastEventTileEnabledSetting = true;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Config\\Config.cs");
    private readonly IConfigProvider configProvider;

    public event EventHandler BackgroundSyncFrequencyChanged;

    public Config(IConfigProvider configProvider) => this.configProvider = configProvider;

    public string Environment
    {
      get
      {
        string environment = this.configProvider.Get<string>(nameof (Environment), CloudEnvironment.Default, ConfigDomain.System);
        if (!CloudEnvironment.IsValid(environment))
          environment = CloudEnvironment.Default;
        return environment;
      }
      set
      {
        if (!CloudEnvironment.IsValid(value))
          throw new InvalidOperationException("Unrecognized environment " + value);
        this.configProvider.Set<string>(nameof (Environment), value, ConfigDomain.System);
      }
    }

    public string ApiVersion
    {
      get => this.configProvider.Get<string>(nameof (ApiVersion), "V1");
      set => this.configProvider.Set<string>(nameof (ApiVersion), value);
    }

    public string CustomEnvironmentUrl
    {
      get => this.configProvider.Get<string>(nameof (CustomEnvironmentUrl), string.Empty);
      set => this.configProvider.Set<string>(nameof (CustomEnvironmentUrl), value);
    }

    public string AuthBaseUrl
    {
      get
      {
        string environment = this.Environment;
        if (environment != null)
        {
          if (CloudEnvironment.IsCustom(environment))
          {
            string customEnvironmentUrl = this.CustomEnvironmentUrl;
            return !string.IsNullOrEmpty(customEnvironmentUrl) ? customEnvironmentUrl : throw new InvalidOperationException("Custom URL was null or empty");
          }
          string url = CloudEnvironment.GetUrl(environment);
          if (url != null)
            return url;
        }
        throw new InvalidOperationException("Could not determine the base cloud environment URL for environment: " + environment);
      }
    }

    public bool UseOAuth
    {
      get => this.configProvider.Get<bool>(nameof (UseOAuth), false);
      set => this.configProvider.Set<bool>(nameof (UseOAuth), value);
    }

    public IList<string> EnabledTiles
    {
      get
      {
        string str = this.configProvider.Get<string>("EnabledStrapps", (string) null);
        if (str == null)
          return (IList<string>) null;
        return (IList<string>) new List<string>((IEnumerable<string>) str.Split('|'));
      }
      set => this.configProvider.Set<string>("EnabledStrapps", string.Join("|", (IEnumerable<string>) value));
    }

    public bool EnabledTilesMatchesBand
    {
      get => this.configProvider.Get<bool>("EnabledStrappsMatchesDevice", true);
      set => this.configProvider.Set<bool>("EnabledStrappsMatchesDevice", value);
    }

    public bool IsBackgroundLoggingEnabled
    {
      get => this.configProvider.Get<bool>("BackgroundLogging", true);
      set => this.configProvider.Set<bool>("BackgroundLogging", value);
    }

    public bool IsBackgroundRefreshPending
    {
      get => this.configProvider.Get<bool>("BackgroundRefreshPending", false);
      set => this.configProvider.Set<bool>("BackgroundRefreshPending", value);
    }

    public bool IsCachingEnabled
    {
      get => this.configProvider.Get<bool>("Caching", true);
      set => this.configProvider.Set<bool>("Caching", value);
    }

    public bool IsMockInsightsEnabled
    {
      get => this.configProvider.Get<bool>("MockInsights", false);
      set => this.configProvider.Set<bool>("MockInsights", value);
    }

    public bool IsSmsEnabled
    {
      get => this.configProvider.Get<bool>(nameof (IsSmsEnabled), true);
      set => this.configProvider.Set<bool>(nameof (IsSmsEnabled), value);
    }

    public bool IsPhoneEnabled
    {
      get => this.configProvider.Get<bool>(nameof (IsPhoneEnabled), true);
      set => this.configProvider.Set<bool>(nameof (IsPhoneEnabled), value);
    }

    public bool IsWeatherEnabled
    {
      get => this.configProvider.Get<bool>(nameof (IsWeatherEnabled), true);
      set => this.configProvider.Set<bool>(nameof (IsWeatherEnabled), value);
    }

    public bool IsCalendarEnabled
    {
      get => this.configProvider.Get<bool>(nameof (IsCalendarEnabled), true);
      set => this.configProvider.Set<bool>(nameof (IsCalendarEnabled), value);
    }

    public bool IsFinanceEnabled
    {
      get => this.configProvider.Get<bool>(nameof (IsFinanceEnabled), true);
      set => this.configProvider.Set<bool>(nameof (IsFinanceEnabled), value);
    }

    public bool IsExerciseEnabled
    {
      get => this.configProvider.Get<bool>(nameof (IsExerciseEnabled), true);
      set => this.configProvider.Set<bool>(nameof (IsExerciseEnabled), value);
    }

    public OobeStatus OobeStatus
    {
      get => this.configProvider.GetEnum<OobeStatus>(this.OobeKey, OobeStatus.NotShown);
      set => this.configProvider.SetEnum<OobeStatus>(this.OobeKey, value);
    }

    public bool IsWiFiOnlySettingEnabled
    {
      get => this.configProvider.Get<bool>("UpdateOnlyOnWiFi", false);
      set => this.configProvider.Set<bool>("UpdateOnlyOnWiFi", value);
    }

    public bool IsBackgroundSyncEnabled
    {
      get => this.configProvider.Get<bool>("Sync", true);
      set => this.configProvider.Set<bool>("Sync", value);
    }

    public int LastSentInsightId
    {
      get => this.configProvider.Get<int>(nameof (LastSentInsightId), 0);
      set => this.configProvider.Set<int>(nameof (LastSentInsightId), value);
    }

    public string ViewedEventsJson
    {
      get => this.configProvider.Get<string>(nameof (ViewedEventsJson), (string) null);
      set => this.configProvider.Set<string>(nameof (ViewedEventsJson), value);
    }

    public DateTimeOffset LastCompletedFullOobe
    {
      get => this.configProvider.GetDateTimeOffset(nameof (LastCompletedFullOobe), DateTimeOffset.MinValue);
      set => this.configProvider.SetDateTimeOffset(nameof (LastCompletedFullOobe), value);
    }

    public IList<Stock> UserStocks
    {
      get
      {
        string str = this.configProvider.Get<string>("RecoveredUserStocks", (string) null);
        if (string.IsNullOrEmpty(str))
          return (IList<Stock>) null;
        try
        {
          return (IList<Stock>) JsonConvert.DeserializeObject<List<Stock>>(str, Microsoft.Health.App.Core.Config.Config.SerializerSettings);
        }
        catch (JsonException ex)
        {
          return (IList<Stock>) null;
        }
      }
      set => this.configProvider.Set<string>("RecoveredUserStocks", JsonConvert.SerializeObject((object) value, Microsoft.Health.App.Core.Config.Config.SerializerSettings));
    }

    public IList<EmailAddress> EmailVips
    {
      get
      {
        string str = this.configProvider.Get<string>("CachedUserVips", (string) null);
        if (string.IsNullOrEmpty(str))
          return (IList<EmailAddress>) null;
        try
        {
          return (IList<EmailAddress>) JsonConvert.DeserializeObject<List<EmailAddress>>(str, Microsoft.Health.App.Core.Config.Config.SerializerSettings);
        }
        catch (JsonException ex)
        {
          return (IList<EmailAddress>) null;
        }
      }
      set => this.configProvider.Set<string>("CachedUserVips", JsonConvert.SerializeObject((object) value, Microsoft.Health.App.Core.Config.Config.SerializerSettings));
    }

    public bool AreEmailVipsEnabled
    {
      get => this.configProvider.Get<bool>("IsCachedUserVipsEnabled", false);
      set => this.configProvider.Set<bool>("IsCachedUserVipsEnabled", value);
    }

    public Version LastRegisteredBackgroundVersion
    {
      get => this.configProvider.GetVersion(nameof (LastRegisteredBackgroundVersion), (Version) null);
      set => this.configProvider.SetVersion(nameof (LastRegisteredBackgroundVersion), value);
    }

    public bool IsFirmwareUpdateCheckingEnabled
    {
      get => this.configProvider.Get<bool>(nameof (IsFirmwareUpdateCheckingEnabled), true);
      set => this.configProvider.Set<bool>(nameof (IsFirmwareUpdateCheckingEnabled), value);
    }

    public bool IsForegroundSyncOnAppStartupEnabled
    {
      get => this.configProvider.Get<bool>(nameof (IsForegroundSyncOnAppStartupEnabled), true);
      set => this.configProvider.Set<bool>(nameof (IsForegroundSyncOnAppStartupEnabled), value);
    }

    public bool IsNavigatedAwayDuringFirmwareUpdatePromptNeeded
    {
      get => this.configProvider.Get<bool>(nameof (IsNavigatedAwayDuringFirmwareUpdatePromptNeeded), false);
      set => this.configProvider.Set<bool>(nameof (IsNavigatedAwayDuringFirmwareUpdatePromptNeeded), value);
    }

    public int TotalTimesSynced
    {
      get => this.configProvider.Get<int>(nameof (TotalTimesSynced), 0);
      set => this.configProvider.Set<int>(nameof (TotalTimesSynced), value);
    }

    public int GWTimeProgressedDays
    {
      get => this.configProvider.Get<int>(nameof (GWTimeProgressedDays), 0);
      set => this.configProvider.Set<int>(nameof (GWTimeProgressedDays), value);
    }

    public bool IsStarbucksEnabled
    {
      get => this.configProvider.Get<bool>(nameof (IsStarbucksEnabled), false);
      set => this.configProvider.Set<bool>(nameof (IsStarbucksEnabled), value);
    }

    public string StarbucksCardNumber
    {
      get => this.configProvider.Get<string>(nameof (StarbucksCardNumber), string.Empty);
      set => this.configProvider.Set<string>(nameof (StarbucksCardNumber), value);
    }

    public SyncFrequency BackgroundSyncFrequency
    {
      get
      {
        SyncFrequency syncFrequency1 = SyncFrequency.ThirtyMinutes;
        SyncFrequency syncFrequency2 = (SyncFrequency) this.configProvider.Get<int>("SyncFrequency", (int) syncFrequency1);
        if (!Enum.IsDefined(typeof (SyncFrequency), (object) syncFrequency2))
        {
          Microsoft.Health.App.Core.Config.Config.Logger.Warn((object) string.Format("Invalid {0} value {1}. Defaulting to {2}.", new object[3]
          {
            (object) typeof (SyncFrequency).Name,
            (object) syncFrequency2,
            (object) syncFrequency1
          }));
          syncFrequency2 = syncFrequency1;
        }
        return syncFrequency2;
      }
      set
      {
        if (this.BackgroundSyncFrequency == value)
          return;
        this.configProvider.Set<int>("SyncFrequency", (int) value);
        EventHandler frequencyChanged = this.BackgroundSyncFrequencyChanged;
        if (frequencyChanged == null)
          return;
        frequencyChanged((object) this, EventArgs.Empty);
      }
    }

    private string OobeKey => "FreStatus_" + this.AuthBaseUrl.ToLowerInvariant();

    public DateTimeOffset LastCompletedRefresh
    {
      get => this.configProvider.GetDateTimeOffset(nameof (LastCompletedRefresh), DateTimeOffset.MinValue);
      set => this.configProvider.SetDateTimeOffset(nameof (LastCompletedRefresh), value);
    }

    public bool IsLiveTileEnabled
    {
      get => this.configProvider.Get<bool>("PhoneSettings.LiveTileEnabled", true);
      set
      {
        if (this.configProvider.Get<bool>("PhoneSettings.LiveTileEnabled", true) == value)
          return;
        this.configProvider.Set<bool>("PhoneSettings.LiveTileEnabled", value);
      }
    }

    public bool IsTransparentLiveTileEnabled
    {
      get => this.configProvider.Get<bool>("PhoneSettings.TransparentLiveTileEnabled", true);
      set
      {
        if (this.configProvider.Get<bool>("PhoneSettings.TransparentLiveTileEnabled", true) == value)
          return;
        this.configProvider.Set<bool>("PhoneSettings.TransparentLiveTileEnabled", value);
      }
    }

    public bool IsStepsTileEnabled
    {
      get => this.configProvider.Get<bool>("PhoneSettings.StepsTileEnabled", true);
      set
      {
        if (this.configProvider.Get<bool>("PhoneSettings.StepsTileEnabled", true) == value)
          return;
        this.configProvider.Set<bool>("PhoneSettings.StepsTileEnabled", value);
      }
    }

    public bool IsCaloriesTileEnabled
    {
      get => this.configProvider.Get<bool>("PhoneSettings.CaloriesTileEnabled", true);
      set
      {
        if (this.configProvider.Get<bool>("PhoneSettings.CaloriesTileEnabled", true) == value)
          return;
        this.configProvider.Set<bool>("PhoneSettings.CaloriesTileEnabled", value);
      }
    }

    public bool IsBatteryTileEnabled
    {
      get => this.configProvider.Get<bool>("PhoneSettings.BatteryTileEnabled", true);
      set
      {
        if (this.configProvider.Get<bool>("PhoneSettings.BatteryTileEnabled", true) == value)
          return;
        this.configProvider.Set<bool>("PhoneSettings.BatteryTileEnabled", value);
      }
    }

    public bool IsLastEventTileEnabled
    {
      get => this.configProvider.Get<bool>("PhoneSettings.LastEventTileEnabled", true);
      set
      {
        if (this.configProvider.Get<bool>("PhoneSettings.LastEventTileEnabled", true) == value)
          return;
        this.configProvider.Set<bool>("PhoneSettings.LastEventTileEnabled", value);
      }
    }
  }
}
