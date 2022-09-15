// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Pedometer.PedometerSyncManager
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Services.Configuration;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Health.Cloud.Client;
using Microsoft.Health.Cloud.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Pedometer
{
  public sealed class PedometerSyncManager : IPedometerSyncManager, IConfigurationState
  {
    private const string SensorCoreSyncManagerCategory = "SensorCoreSyncManager";
    private static readonly string EnabledTimeKey = ConfigurationValue.CreateKey("SensorCoreSyncManager", "EnabledTime");
    public static readonly ConfigurationValue<int> SensorReadingDurationInMinutesValue = (ConfigurationValue<int>) ConfigurationValue.CreateInteger("SensorCoreSyncManager", "SensorReadingDurationInMinutes", Range.GetInclusive<int>(0, 5), 5);
    public static readonly ConfigurationValue<int> MaxSyncDurationInMinutesValue = (ConfigurationValue<int>) ConfigurationValue.CreateInteger("SensorCoreSyncManager", "MaxSyncDurationInMinutes", Range.GetInclusive<int>(0, 10080), 10080);
    private static readonly string LastSyncTimeKey = ConfigurationValue.CreateKey("SensorCoreSyncManager", "LastSyncTime");
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\Pedometer\\PedometerSyncManager.cs");
    private static readonly TimeSpan MinimumReadingDuration = TimeSpan.FromMinutes(5.0);
    private readonly Mutex syncMutex;
    private readonly IConfigProvider configProvider;
    private readonly IConfigurationService configurationService;
    private readonly IDateTimeService dateTimeService;
    private readonly IPedometerManager pedometerManager;
    private readonly IPedometerLogger pedometerLogger;
    private readonly ISensorUploader sensorUploader;
    private readonly IEnvironmentService environmentService;
    private readonly IDeviceClient deviceClient;
    private readonly SingletonTask<DeviceSyncProgress> syncTask;

    public PedometerSyncManager(
      IConfigProvider configProvider,
      IConfigurationService configurationService,
      IDateTimeService dateTimeService,
      IPedometerManager pedometerManager,
      IPedometerLogger pedometerLogger,
      ISensorUploader sensorUploader,
      IEnvironmentService environmentService,
      IDeviceClient deviceClient,
      IMutexService mutexService)
    {
      Assert.ParamIsNotNull((object) configProvider, nameof (configProvider));
      Assert.ParamIsNotNull((object) configurationService, nameof (configurationService));
      Assert.ParamIsNotNull((object) dateTimeService, nameof (dateTimeService));
      Assert.ParamIsNotNull((object) pedometerManager, nameof (pedometerManager));
      Assert.ParamIsNotNull((object) pedometerLogger, "sensorCoreLogger");
      Assert.ParamIsNotNull((object) sensorUploader, nameof (sensorUploader));
      Assert.ParamIsNotNull((object) sensorUploader, nameof (environmentService));
      Assert.ParamIsNotNull((object) sensorUploader, nameof (deviceClient));
      this.configProvider = configProvider;
      this.configurationService = configurationService;
      this.dateTimeService = dateTimeService;
      this.pedometerManager = pedometerManager;
      this.pedometerLogger = pedometerLogger;
      this.sensorUploader = sensorUploader;
      this.environmentService = environmentService;
      this.deviceClient = deviceClient;
      this.syncMutex = mutexService.GetNamedMutex(false, "KApp.Pedometer.Sync");
      this.syncTask = new SingletonTask<DeviceSyncProgress>((Func<CancellationToken, IProgress<DeviceSyncProgress>, Task>) ((token, progress) => this.syncMutex.RunSynchronizedAsync((Func<Task>) (() => this.OnSyncWithDeviceAsync(token, progress)), token)));
    }

    public Task<DateTimeOffset> GetLastSyncTimeAsync(CancellationToken token)
    {
      token.ThrowIfCancellationRequested();
      return Task.FromResult<DateTimeOffset>(this.GetLastSyncTime());
    }

    public Task SetSyncEnabledTimeAsync(CancellationToken token)
    {
      token.ThrowIfCancellationRequested();
      this.configProvider.SetDateTimeOffset(PedometerSyncManager.EnabledTimeKey, this.dateTimeService.Now.ToUniversalTime());
      return (Task) Task.FromResult<bool>(true);
    }

    public async Task SyncWithDeviceAsync(
      CancellationToken token,
      IProgress<DeviceSyncProgress> progress = null)
    {
      if (!await this.pedometerManager.IsEnabledAsync(token).ConfigureAwait(false))
        PedometerSyncManager.Logger.Info((object) "SensorCore is disabled; sync request ignored.");
      else
        await this.syncTask.RunAsync(token, progress).ConfigureAwait(false);
    }

    public async Task ResetStateAsync(CancellationToken token)
    {
      this.configProvider.Remove(PedometerSyncManager.LastSyncTimeKey);
      ConfiguredTaskAwaitable configuredTaskAwaitable = this.pedometerLogger.ResetStateAsync(token).ConfigureAwait(false);
      await configuredTaskAwaitable;
      configuredTaskAwaitable = this.sensorUploader.ResetStateAsync(token).ConfigureAwait(false);
      await configuredTaskAwaitable;
    }

    private async Task OnSyncWithDeviceAsync(
      CancellationToken token,
      IProgress<DeviceSyncProgress> progress = null)
    {
      PedometerSyncManager.Logger.Info((object) "Starting SensorCore sync...");
      PedometerSyncManager.ReportProgress(progress, true, 0.0);
      TimeSpan readingDuration = this.GetSensorReadingDuration();
      DateTimeOffset lastCompleteIntervalEndTime = default;
      DateTimeOffset dateTimeOffset1 = lastCompleteIntervalEndTime;
      lastCompleteIntervalEndTime = await this.sensorUploader.GetLastCompleteReadingEndTimeAsync(readingDuration, token).ConfigureAwait(false);
      DateTimeOffset now = this.dateTimeService.Now;
      if (lastCompleteIntervalEndTime == DateTimeOffset.MinValue)
      {
        PedometerSyncManager.Logger.Info((object) "No previous sync has been performed.");
        IEnumerable<RegisteredDeviceSettings> source = await this.deviceClient.GetDevicesAsync(token).ConfigureAwait(false);
        string phoneId = this.environmentService.PhoneId;
        if (source != null && source.All<RegisteredDeviceSettings>((Func<RegisteredDeviceSettings, bool>) (d => !StringComparer.OrdinalIgnoreCase.Equals(d.DeviceId, phoneId))))
        {
          IDeviceClient deviceClient = this.deviceClient;
          List<RegisteredDeviceSettings> registeredDeviceSettingsList = new List<RegisteredDeviceSettings>();
          registeredDeviceSettingsList.Add(new RegisteredDeviceSettings()
          {
            DeviceId = phoneId,
            DeviceMetadataHint = this.environmentService.OperatingSystemName
          });
          CancellationToken cancellationToken = token;
          await deviceClient.AddDevicesAsync((IEnumerable<RegisteredDeviceSettings>) registeredDeviceSettingsList, cancellationToken).ConfigureAwait(false);
        }
        DateTimeOffset dateTimeOffset2 = this.configProvider.GetDateTimeOffset(PedometerSyncManager.EnabledTimeKey, DateTimeOffset.MinValue);
        lastCompleteIntervalEndTime = !(lastCompleteIntervalEndTime < dateTimeOffset2) ? (DateTimeOffset) now.Date : this.GetRoundedEnabledTime(dateTimeOffset2);
      }
      else
      {
        DateTimeOffset dateTimeOffset3 = this.configProvider.GetDateTimeOffset(PedometerSyncManager.EnabledTimeKey, DateTimeOffset.MinValue);
        PedometerSyncManager.Logger.Info((object) ("SensorCore Enabled: " + (object) dateTimeOffset3));
        if (lastCompleteIntervalEndTime < dateTimeOffset3)
          lastCompleteIntervalEndTime = this.GetRoundedEnabledTime(dateTimeOffset3);
        PedometerSyncManager.Logger.Info((object) ("SensorCore Last Reading: " + (object) lastCompleteIntervalEndTime));
      }
      TimeSpan timeSpan = now - lastCompleteIntervalEndTime;
      TimeSpan maxSyncDuration = this.GetMaxSyncDuration();
      if (timeSpan > maxSyncDuration)
      {
        PedometerSyncManager.Logger.Warn("We're trying to sync a duration '{0}' greater than the '{1}' maximum allowed.", (object) timeSpan, (object) maxSyncDuration);
        await Task.Run((Action) (() =>
        {
          DateTimeOffset dateTimeOffset6 = now - maxSyncDuration;
          while (lastCompleteIntervalEndTime + readingDuration < dateTimeOffset6)
            lastCompleteIntervalEndTime += readingDuration;
        }), token).ConfigureAwait(false);
      }
      IEnumerable<SensorReading> sensorReadings = await this.GetReadingsAsync(now, lastCompleteIntervalEndTime, readingDuration, token).ConfigureAwait(false);
      PedometerSyncManager.ReportProgress(progress, true, 33.3);
      PedometerSyncManager.Logger.Debug((object) "Logging sensor readings...");
      await this.pedometerLogger.LogReadingsAsync(sensorReadings, token).ConfigureAwait(false);
      PedometerSyncManager.ReportProgress(progress, true, 66.6);
      PedometerSyncManager.Logger.Debug((object) "Uploading sensor readings...");
      IDictionary<string, string> uploadMetadataAsync = await this.GetUploadMetadataAsync(token);
      await this.sensorUploader.UploadReadingsAsync(sensorReadings, readingDuration, uploadMetadataAsync, token).ConfigureAwait(false);
      this.SetLastSyncTime(now);
      PedometerSyncManager.ReportProgress(progress, false, 100.0);
      PedometerSyncManager.Logger.Info((object) "SensorCore sync complete.");
    }

    private async Task<IEnumerable<SensorReading>> GetReadingsAsync(
      DateTimeOffset now,
      DateTimeOffset lastCompleteIntervalEndTime,
      TimeSpan readingDuration,
      CancellationToken token)
    {
      List<DateTimeOffset> range = lastCompleteIntervalEndTime.Range(now.RoundUp(readingDuration), readingDuration).ToList<DateTimeOffset>();
      PedometerSyncManager.Logger.Debug("Sync will begin at '{0}' with {1} interval(s) of duration '{2}'...", (object) lastCompleteIntervalEndTime, (object) range.Count, (object) readingDuration);
      List<SensorReading> allSensorReadings = new List<SensorReading>();
      await this.pedometerManager.StartBatchReadAsync(token).ConfigureAwait(false);
      Exception e = (Exception) null;
      try
      {
        if (range.Any<DateTimeOffset>())
        {
          lastCompleteIntervalEndTime = range.Last<DateTimeOffset>() + readingDuration;
          IEnumerable<SensorReading> collection = await this.pedometerManager.GetSensorReadingsAsync((IList<DateTimeOffset>) range, readingDuration, token).ConfigureAwait(false);
          allSensorReadings.AddRange(collection);
          SensorReading sensorReading = allSensorReadings.Last<SensorReading>();
          if (now.Subtract(sensorReading.StartTime) < readingDuration)
            sensorReading.Duration = now - sensorReading.StartTime;
        }
      }
      catch (Exception ex)
      {
        e = ex;
      }
      await this.pedometerManager.EndBatchReadAsync(token).ConfigureAwait(false);
      if (e != null)
        throw e;
      return (IEnumerable<SensorReading>) allSensorReadings;
    }

    private DateTimeOffset GetRoundedEnabledTime(DateTimeOffset enabledTime) => enabledTime.RoundUp(TimeSpan.FromMinutes((double) this.configurationService.GetValue<int>(PedometerSyncManager.SensorReadingDurationInMinutesValue)));

    private DateTimeOffset GetLastSyncTime() => this.configProvider.GetDateTimeOffset(PedometerSyncManager.LastSyncTimeKey, DateTimeOffset.MinValue);

    private void SetLastSyncTime(DateTimeOffset dateTime) => this.configProvider.SetDateTimeOffset(PedometerSyncManager.LastSyncTimeKey, dateTime);

    private TimeSpan GetSensorReadingDuration() => TimeSpan.FromMinutes((double) this.configurationService.GetValue<int>(PedometerSyncManager.SensorReadingDurationInMinutesValue));

    private TimeSpan GetMaxSyncDuration() => TimeSpan.FromMinutes((double) this.configurationService.GetValue<int>(PedometerSyncManager.MaxSyncDurationInMinutesValue));

    private static void ReportProgress(
      IProgress<DeviceSyncProgress> progress,
      bool isSyncing,
      double percentComplete)
    {
      progress?.Report(new DeviceSyncProgress(isSyncing, ProgressStage.Syncing, percentComplete));
    }

    private async Task<IDictionary<string, string>> GetUploadMetadataAsync(
      CancellationToken cancellationToken)
    {
      int? versionAsync = await this.pedometerManager.GetVersionAsync(cancellationToken);
      string source = this.pedometerManager.GetSource(cancellationToken);
      Assert.IsTrue(source != null, "Pedometer source cannot be null.");
      return (IDictionary<string, string>) new Dictionary<string, string>()
      {
        {
          "HostOS",
          this.environmentService.OperatingSystemName
        },
        {
          "HostOSVersion",
          this.environmentService.OperatingSystemVersion.ToString()
        },
        {
          "HostAppVersion",
          this.environmentService.ApplicationVersion.ToString()
        },
        {
          "DeviceVersion",
          versionAsync.ToString()
        },
        {
          "PedometerSource",
          source
        }
      };
    }
  }
}
