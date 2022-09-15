// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Sync.LogSyncTask
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Services.Debugging;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Storage;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Sync
{
  [SyncTask(SyncTaskType.Foreground | SyncTaskType.Background | SyncTaskType.BandEvent | SyncTaskType.Primary)]
  public class LogSyncTask : IBandSyncTask
  {
    private const double SyncTaskWeight = 0.7;
    private const double LogProcessingTaskWeight = 0.3;
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Sync\\LogSyncTask.cs");
    private readonly IConfig config;
    private readonly IBandConnectionFactory cargoConnectionFactory;
    private readonly IFileObjectStorageService isoObjectStore;
    private ITimedTelemetryEvent timedEvent;
    private SyncState currentSyncState;
    private SyncDebugResult debugSyncResult;

    public LogSyncTask(
      IConfig config,
      IBandConnectionFactory cargoConnectionFactory,
      IFileObjectStorageService isoObjectStore)
    {
      this.config = config;
      this.cargoConnectionFactory = cargoConnectionFactory;
      this.isoObjectStore = isoObjectStore;
    }

    public async Task RunAsync(
      BandSyncContext context,
      CancellationToken cancellationToken,
      SyncDebugResult debugResults = null)
    {
      this.debugSyncResult = debugResults;
      SyncResultWrapper syncResultWrapper = await this.PerformLogSyncAsync(context, cancellationToken);
      this.BytesUploaded = syncResultWrapper.UploadedSensorLogBytes;
      this.DownloadKbytesPerSecondFromDevice = syncResultWrapper.DownloadKbytesPerSecond;
      this.UploadKbytesPerSecondFromCloud = syncResultWrapper.UploadKbytesPerSecond;
    }

    public long BytesUploaded { get; private set; }

    public double DownloadKbytesPerSecondFromDevice { get; private set; }

    public double UploadKbytesPerSecondFromCloud { get; private set; }

    public async Task<SyncResultWrapper> PerformLogSyncAsync(
      BandSyncContext context,
      CancellationToken cancellationToken)
    {
      bool isForeground = (uint) context.SyncType > 0U;
      if (isForeground || this.config.IsBackgroundSyncEnabled)
        return await this.LogSyncAsync(isForeground, cancellationToken, context.Progress);
      throw new BandSyncException("Background sync is disabled in config.");
    }

    private async Task<SyncResultWrapper> LogSyncAsync(
      bool isForeground,
      CancellationToken cancellationToken,
      Action<SyncTaskProgress> progress)
    {
      SyncState syncState = SyncState.NotStarted;
      LogSyncTask.Logger.Info((object) "<START> task : sync");
      SyncResultWrapper syncResultWrapper1;
      using (await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
      {
        DeviceSyncProgress deviceSyncProgress = new DeviceSyncProgress();
        SyncResultWrapper syncResultWrapper = await this.SyncDeviceToCloudInternalAsync(isForeground, cancellationToken, (Action<SyncTaskProgress>) (syncProgress => progress(new SyncTaskProgress(0.9, syncProgress.PercentageComplete * 0.7, ProgressStage.Syncing))));
        if (!syncResultWrapper.RanToCompletion)
          throw new BandSyncException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Sync failed with sync state {0}.", new object[1]
          {
            (object) syncState
          }));
        ApplicationTelemetry.LogGetLogsFromBand(isForeground, syncResultWrapper.DownloadedSensorLogBytes, syncResultWrapper.DownloadKbytesPerSecond, syncResultWrapper.DownloadTime);
        ApplicationTelemetry.LogUploadLogsToCloud(isForeground, syncResultWrapper.UploadedSensorLogBytes, syncResultWrapper.UploadKbytesPerSecond, syncResultWrapper.UploadTime);
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        using (ApplicationTelemetry.TimeCloudLogProcessing())
        {
          LogSyncTask.Logger.Info((object) "<START> task : waiting for cloud to process logs from sync");
          await this.WaitOnLogProcessingAsync(syncResultWrapper, cancellationToken, progress);
          LogSyncTask.Logger.Info((object) "<END> task : waiting for cloud to process logs from sync");
        }
        stopWatch.Stop();
        this.debugSyncResult.CloudProcessing = stopWatch.ElapsedMilliseconds;
        progress(new SyncTaskProgress(0.9, 100.0, ProgressStage.Analyzing));
        syncResultWrapper1 = syncResultWrapper;
      }
      return syncResultWrapper1;
    }

    private async Task WaitOnLogProcessingAsync(
      SyncResultWrapper syncResultWrapper,
      CancellationToken cancellationToken,
      Action<SyncTaskProgress> progress)
    {
      using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
      {
        try
        {
          await cargoConnection.UpdateLogProcessingAsync(syncResultWrapper.LogFilesProcessing, cancellationToken, (Action<double>) (logProgress => progress(new SyncTaskProgress(0.9, 70.0 + logProgress * 0.3, ProgressStage.Analyzing))), false);
        }
        catch (Exception ex) when (ex.CatchWhen((Func<bool>) (() =>
        {
          LogSyncTask.Logger.Debug((object) "<FAILED> waiting for cloud to indicate done processing logs before sync complete");
          return ex is OperationCanceledException;
        })))
        {
          throw new InternetException("Waiting for cloud to indicate done processing logs did not complete in time.", ex);
        }
      }
    }

    private Task<SyncResultWrapper> SyncDeviceToCloudInternalAsync(
      bool isForeground,
      CancellationToken cancellationToken,
      Action<SyncTaskProgress> progress)
    {
      Action<SyncProgressWrapper> action = null;
      return Task.Run<SyncResultWrapper>((Func<Task<SyncResultWrapper>>) (async () =>
      {
        SyncResultWrapper syncResultWrapper;
        using (IBandConnection cargoConnection = await this.cargoConnectionFactory.CreateConnectionAsync(cancellationToken))
        {
          DeviceSyncProgress deviceSyncProgress = new DeviceSyncProgress();
          SyncResultWrapper cloudAsync = await cargoConnection.SyncBandToCloudAsync(cancellationToken, action ?? (action = (Action<SyncProgressWrapper>) (syncProgress =>
          {
            this.ProcessSyncState(syncProgress.State);
            progress(new SyncTaskProgress(0.9, syncProgress.PercentageCompletion * 0.7, ProgressStage.Syncing));
          })), isForeground);
          if (this.debugSyncResult != null)
          {
            long? nullable;
            if (cloudAsync.EphemerisCheckElapsed.HasValue)
            {
              SyncDebugResult debugSyncResult = this.debugSyncResult;
              nullable = cloudAsync.EphemerisCheckElapsed;
              long num = nullable.Value;
              debugSyncResult.EphemerisCheckElapsed = num;
            }
            nullable = cloudAsync.EphemerisUpdateElapsed;
            if (nullable.HasValue)
            {
              SyncDebugResult debugSyncResult = this.debugSyncResult;
              nullable = cloudAsync.EphemerisUpdateElapsed;
              long num = nullable.Value;
              debugSyncResult.EphemerisUpdateElapsed = num;
            }
            nullable = cloudAsync.CrashDumpElapsed;
            if (nullable.HasValue)
            {
              SyncDebugResult debugSyncResult = this.debugSyncResult;
              nullable = cloudAsync.CrashDumpElapsed;
              long num = nullable.Value;
              debugSyncResult.CrashDump = num;
            }
            nullable = cloudAsync.TimeZoneElapsed;
            if (nullable.HasValue)
            {
              SyncDebugResult debugSyncResult = this.debugSyncResult;
              nullable = cloudAsync.TimeZoneElapsed;
              long num = nullable.Value;
              debugSyncResult.TimeZone = num;
            }
            nullable = cloudAsync.WebTilesElapsed;
            if (nullable.HasValue)
            {
              SyncDebugResult debugSyncResult = this.debugSyncResult;
              nullable = cloudAsync.WebTilesElapsed;
              long num = nullable.Value;
              debugSyncResult.WebTiles = num;
            }
            nullable = cloudAsync.SensorLogElapsed;
            if (nullable.HasValue)
            {
              SyncDebugResult debugSyncResult = this.debugSyncResult;
              nullable = cloudAsync.SensorLogElapsed;
              long num = nullable.Value;
              debugSyncResult.SendPhoneSensorToCloud = num;
            }
            nullable = cloudAsync.UserProfileFirmwareBytesElapsed;
            if (nullable.HasValue)
            {
              SyncDebugResult debugSyncResult = this.debugSyncResult;
              nullable = cloudAsync.UserProfileFirmwareBytesElapsed;
              long num = nullable.Value;
              debugSyncResult.UserProfileFirmwareBytes = num;
            }
            nullable = cloudAsync.UserProfileFullElapsed;
            if (nullable.HasValue)
            {
              SyncDebugResult debugSyncResult = this.debugSyncResult;
              nullable = cloudAsync.UserProfileFullElapsed;
              long num = nullable.Value;
              debugSyncResult.UserProfileFull = num;
            }
          }
          syncResultWrapper = cloudAsync;
        }
        return syncResultWrapper;
      }), cancellationToken);
    }

    private void ProcessSyncState(SyncState syncState)
    {
      if (this.currentSyncState == syncState)
        return;
      SyncState currentSyncState = this.currentSyncState;
      this.currentSyncState = syncState;
      try
      {
        this.timedEvent?.Dispose();
      }
      catch (Exception ex)
      {
        LogSyncTask.Logger.Error((object) string.Format("Error transitioning timedEvent from {0} to {1}.", new object[2]
        {
          (object) currentSyncState,
          (object) this.currentSyncState
        }), ex);
      }
      try
      {
        this.timedEvent = ApplicationTelemetry.TimeSyncOperationTime(syncState);
      }
      catch (Exception ex)
      {
        LogSyncTask.Logger.Error((object) string.Format("Error starting telemetry for {0}.", new object[1]
        {
          (object) this.currentSyncState
        }), ex);
      }
    }
  }
}
