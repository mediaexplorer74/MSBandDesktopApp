// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Config.IConfig
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Sync;
using System;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Config
{
  public interface IConfig
  {
    event EventHandler BackgroundSyncFrequencyChanged;

    string Environment { get; set; }

    string CustomEnvironmentUrl { get; set; }

    string ApiVersion { get; set; }

    string AuthBaseUrl { get; }

    bool IsBackgroundLoggingEnabled { get; set; }

    bool IsCachingEnabled { get; set; }

    bool IsMockInsightsEnabled { get; set; }

    bool IsSmsEnabled { get; set; }

    bool IsPhoneEnabled { get; set; }

    bool IsWeatherEnabled { get; set; }

    bool IsFinanceEnabled { get; set; }

    bool IsCalendarEnabled { get; set; }

    bool IsExerciseEnabled { get; set; }

    OobeStatus OobeStatus { get; set; }

    bool IsWiFiOnlySettingEnabled { get; set; }

    bool IsBackgroundSyncEnabled { get; set; }

    int LastSentInsightId { get; set; }

    IList<string> EnabledTiles { get; set; }

    IList<Stock> UserStocks { get; set; }

    IList<EmailAddress> EmailVips { get; set; }

    bool AreEmailVipsEnabled { get; set; }

    bool IsStarbucksEnabled { get; set; }

    string StarbucksCardNumber { get; set; }

    bool UseOAuth { get; set; }

    string ViewedEventsJson { get; set; }

    bool IsFirmwareUpdateCheckingEnabled { get; set; }

    bool IsForegroundSyncOnAppStartupEnabled { get; set; }

    bool IsNavigatedAwayDuringFirmwareUpdatePromptNeeded { get; set; }

    int TotalTimesSynced { get; set; }

    int GWTimeProgressedDays { get; set; }

    bool EnabledTilesMatchesBand { get; set; }

    Version LastRegisteredBackgroundVersion { get; set; }

    DateTimeOffset LastCompletedFullOobe { get; set; }

    bool IsBackgroundRefreshPending { get; set; }

    SyncFrequency BackgroundSyncFrequency { get; set; }

    DateTimeOffset LastCompletedRefresh { get; set; }

    bool IsLiveTileEnabled { get; set; }

    bool IsTransparentLiveTileEnabled { get; set; }

    bool IsStepsTileEnabled { get; set; }

    bool IsCaloriesTileEnabled { get; set; }

    bool IsBatteryTileEnabled { get; set; }

    bool IsLastEventTileEnabled { get; set; }
  }
}
