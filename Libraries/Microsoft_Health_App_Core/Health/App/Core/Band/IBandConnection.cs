// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Band.IBandConnection
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band;
using Microsoft.Band.Admin;
using Microsoft.Band.Personalization;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Models.Diagnostics;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Band
{
  public interface IBandConnection : IDisposable
  {
    Task NotifyOfSuspendAsync(CancellationToken cancellationToken);

    void NotifyOfResume();

    bool IsDisposed { get; }

    Task<IBandInfo> GetPrimaryPairedBandAsync(CancellationToken cancellationToken);

    Task<IBandInfo[]> GetPairedBandsAsync(CancellationToken cancellationToken);

    Task<BandUserProfile> GetUserProfileAsync(
      Func<IUserProfile, Task<BandUserProfile>> profileConversion,
      CancellationToken cancellationToken);

    Task<BandUserProfile> GetCloudUserProfileAsync(
      Func<IUserProfile, Task<BandUserProfile>> profileConversion);

    Task SaveCloudUserProfileAsync(Func<IUserProfile, Task> userProfileModifications);

    Task SaveUserProfileAsync(
      Func<IUserProfile, Task> userProfileModifications,
      CancellationToken cancellationToken);

    Task SendFinanceUpdateNotificationsAsync(
      IEnumerable<Stock> stocks,
      DateTimeOffset timestamp,
      CancellationToken cancellationToken);

    Task SendWeatherUpdateNotificationsAsync(
      IList<WeatherDay> dailyConditions,
      string location,
      DateTimeOffset timestamp,
      CancellationToken cancellationToken);

    Task SendStarbucksUpdateNotificationsAsync(
      string cardNumber,
      CancellationToken cancellationToken);

    Task SendCalendarEventsAsync(
      IEnumerable<CalendarEvent> calendarEvents,
      CancellationToken cancellationToken);

    Task SetSleepNotificationAsync(
      SleepNotification sleepNotification,
      CancellationToken cancellationToken);

    Task DisableSleepNotificationAsync(CancellationToken cancellationToken);

    Task SetLightExposureNotificationAsync(
      LightExposureNotification lightExposureNotification,
      CancellationToken cancellationToken);

    Task DisableLightExposureNotificationAsync(CancellationToken cancellationToken);

    Task SaveGoalsToBandAsync(Goals goals);

    Task<bool> TryCheckConnectionWorkingAsync();

    Task<FirmwareVersions> GetBandFirmwareVersionAsync(
      CancellationToken cancellationToken);

    Task NavigateToScreenAsync(CargoScreen screen, CancellationToken cancellationToken);

    Task SetOobeStageAsync(OobeStage stage, CancellationToken cancellationToken);

    Task FinalizeOobeAsync(CancellationToken cancellationToken);

    Task PersonalizeBandAsync(
      uint imageId,
      CancellationToken cancellationToken,
      StartStrip startStrip = null,
      BandImage image = null,
      BandTheme color = null,
      IDictionary<Guid, BandTheme> customColors = null);

    Task UpdateGpsEphemerisDataAsync(CancellationToken cancellationToken, bool forceUpdate);

    Task UpdateTimeZoneListAsync(CancellationToken cancellationToken, bool forceUpdate);

    Task SetCurrentTimeAndTimeZoneAsync(CancellationToken cancellationToken);

    Task<string> GetProductSerialNumberAsync(CancellationToken cancellationToken);

    Task<IList<AdminBandTile>> GetDefaultTilesAsync(
      CancellationToken cancellationToken);

    Task<StartStrip> GetStartStripAsync(CancellationToken cancellationToken);

    Task<StartStrip> GetStartStripWithoutImagesAsync(
      CancellationToken cancellationToken);

    Task<BandTheme> GetBandThemeAsync(CancellationToken cancellationToken);

    Task SetStartStripAsync(StartStrip startStrip, CancellationToken cancellationToken);

    Task<string[]> GetSmsResponsesAsync(CancellationToken cancellationToken);

    Task SetSmsResponsesAsync(
      string response1,
      string response2,
      string response3,
      string response4,
      CancellationToken cancellationToken);

    Task<string[]> GetPhoneCallResponsesAsync(CancellationToken cancellationToken);

    Task SetPhoneCallResponsesAsync(
      string response1,
      string response2,
      string response3,
      string response4,
      CancellationToken cancellationToken);

    Task<AdminTileSettings> GetTileSettingsAsync(Guid id);

    Task SetTileSettingsAsync(Guid id, AdminTileSettings settings);

    Task ImportUserProfileAsync(Func<IUserProfile, Task> userProfileModifications);

    Task<CargoRunDisplayMetrics> GetRunDisplayMetricsAsync();

    Task SetRunDisplayMetricsAsync(CargoRunDisplayMetrics metrics);

    Task<CargoBikeDisplayMetrics> GetBikeDisplayMetricsAsync();

    Task SetBikeDisplayMetricsAsync(CargoBikeDisplayMetrics metrics);

    Task<int> GetBikeSplitDistanceAsync();

    Task SetBikeSplitDistanceAsync(int splitGroupSize);

    Task<uint> GetMaxTileCountAsync();

    Task<int> GetBatteryChargeAsync(CancellationToken cancellationToken);

    Task<SyncResultWrapper> SyncBandToCloudAsync(
      CancellationToken cancellationToken,
      Action<SyncProgressWrapper> onSyncProgress,
      bool logsOnly = false);

    Task UpdateLogProcessingAsync(
      IList<LogProcessingStatusWrapper> logProcessingStatusWrappers,
      CancellationToken cancellationToken,
      Action<double> onCloudProcessingLogsUpdate,
      bool singleCallback = true);

    Task SendWorkoutToBandAsync(Stream workoutStream, CancellationToken cancellationToken);

    Task<bool> GetBandOobeCompletedAsync(CancellationToken cancellationToken);

    Task LinkBandToProfileAsync(CancellationToken cancellationToken);

    Task CheckConnectionWorkingAsync(CancellationToken cancellationToken);

    Task UnlinkBandFromProfileAsync(CancellationToken cancellationToken);

    Task<UpdateInfo> GetLatestAvailableFirmwareVersionAsync(
      CancellationToken cancellationToken);

    Task UpdateFirmwareAsync(
      CancellationToken cancellationToken,
      IProgress<FirmwareUpdateProgressReport> progress = null);

    Task<uint> GetMeTileIdAsync(CancellationToken cancellationToken);

    Task SendGolfCourseToBandAsync(Stream golfCourseStream, CancellationToken cancellationToken);

    Task SyncWebTilesAsync(bool forceSync, CancellationToken cancellationToken);

    Task SyncWebTileAsync(Guid tileId, CancellationToken cancellationToken);

    Task<DiagnosticsBandDevice> GetDiagnosticsInfoAsync(
      CancellationToken token);

    Task<string> UploadFileToCloudAsync(
      IFile file,
      LogFileTypes fileType,
      CancellationToken token);

    Task<IDynamicBandConstants> GetAppBandConstantsAsync(
      CancellationToken cancellationToken);

    Task SetWorkoutActivitiesAsync(
      IList<WorkoutActivity> activities,
      CancellationToken cancellationToken);

    Task SendCallNotificationAsync(
      CallNotification callNotification,
      CancellationToken cancellationToken);

    Task QueueSmsNotificationAsync(BandMessage message, CancellationToken cancellationToken);

    Task SendTileMessageAsync(
      Guid tileId,
      TileMessage message,
      CancellationToken cancellationToken);
  }
}
