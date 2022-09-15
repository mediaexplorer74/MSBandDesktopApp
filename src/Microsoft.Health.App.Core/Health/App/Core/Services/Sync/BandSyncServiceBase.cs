// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Sync.BandSyncServiceBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Band;
using Microsoft.Health.App.Core.Config;
using Microsoft.Health.App.Core.Diagnostics;
using Microsoft.Health.App.Core.Exceptions;
using Microsoft.Health.App.Core.Extensions;
using Microsoft.Health.App.Core.Models;
using Microsoft.Health.App.Core.Resources;
using Microsoft.Health.App.Core.Services.Debugging;
using Microsoft.Health.App.Core.Services.Logging.Framework;
using Microsoft.Health.App.Core.Services.Storage;
using Microsoft.Health.App.Core.Services.ToastNotification;
using Microsoft.Health.App.Core.Sync;
using Microsoft.Health.App.Core.Utilities;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.App.Core.Services.Sync
{
  public abstract class BandSyncServiceBase : ISyncService
  {
    public const string BackgroundCancelEventName = "KApp.BackgroundCancel";
    private const string LastSyncTimeName = "LastSyncTime";
    private static readonly ILog Logger = LogManager.GetFileLogger("C:\\Builds\\6968\\6170\\Sources\\Source\\Windows\\KApp\\Core\\Services\\Sync\\BandSyncServiceBase.cs");
    private readonly object lockObj = new object();
    private readonly IConfig config;
    private readonly IFileObjectStorageService isoObjectStore;
    private readonly INetworkService networkService;
    private readonly IEnvironmentService environmentService;
    private readonly IToastNotificationService toastNotificationService;
    private readonly IConfigProvider configProvider;
    private readonly IDateTimeService dateTimeService;
    private readonly IFirmwareUpdateService firmwareUpdateService;
    private readonly IDebugReporterService debugReporterService;
    private long sensorBytesUploaded;
    private double downloadKbytesPerSecondFromDevice;
    private double uploadKbytesPerSecondFromCloud;
    private Task syncTask;
    private bool isSyncing;
    private SyncDebugResult currentSyncResult;
    private static Type[] periodicTaskTypes = new Type[7]
    {
      typeof (LogSyncTask),
      typeof (CloudNotificationUpdatesSyncTask),
      typeof (UpdateTilesTask),
      typeof (GuidedWorkoutsSyncTask),
      typeof (CoachingSyncTask),
      typeof (UpdateGuidedWorkoutTask),
      typeof (UpdateExerciseTileTask)
    };

    public BandSyncServiceBase(
      IConfig config,
      IFileObjectStorageService isoObjectStore,
      INetworkService networkService,
      IEnvironmentService environmentService,
      IToastNotificationService toastNotificationService,
      IConfigProvider configProvider,
      IDateTimeService dateTimeService,
      IFirmwareUpdateService firmwareUpdateService,
      IDebugReporterService debugReporterService)
    {
      this.config = config;
      this.isoObjectStore = isoObjectStore;
      this.networkService = networkService;
      this.environmentService = environmentService;
      this.toastNotificationService = toastNotificationService;
      this.configProvider = configProvider;
      this.dateTimeService = dateTimeService;
      this.firmwareUpdateService = firmwareUpdateService;
      this.debugReporterService = debugReporterService;
    }

    private bool WifiOnlyWithNoWifi => !this.networkService.OnWifi && this.config.IsWiFiOnlySettingEnabled;

    private bool OobeCompleted => this.config.OobeStatus == OobeStatus.Shown;

    private Stopwatch Stopwatch { get; set; }

    public DateTimeOffset LastSyncTime => this.configProvider.GetDateTimeOffset(nameof (LastSyncTime), DateTimeOffset.MinValue);

    public Task SyncAsync(
      SyncType syncType,
      CancellationToken cancellationToken,
      IProgress<DeviceSyncProgress> progress = null)
    {
      this.currentSyncResult = new SyncDebugResult()
      {
        SyncType = syncType.ToString()
      };
      lock (this.lockObj)
      {
        if (!this.isSyncing)
        {
          BandSyncServiceBase.Logger.Debug((object) string.Format("<START> {0} sync", new object[1]
          {
            (object) syncType.ToString()
          }));
          this.isSyncing = true;
          this.BroadcastSyncBandStateChanged(progress, ProgressStage.CheckingYourBand, 10.0);
          Dictionary<SyncTaskType, IList<Type>> dictionary = BandSyncServiceBase.GroupAndFilterTasks(BandSyncServiceBase.periodicTaskTypes, syncType);
          using (CommonTelemetry.TimeSync(syncType))
            this.syncTask = this.RunAsync(syncType, cancellationToken, (IEnumerable<Type>) dictionary[SyncTaskType.Primary], (IEnumerable<Type>) dictionary[SyncTaskType.Secondary], progress);
        }
      }
      return this.syncTask;
    }

    private async Task RunAsync(
      SyncType syncType,
      CancellationToken cancellationToken,
      IEnumerable<Type> primaryTaskTypes,
      IEnumerable<Type> secondaryTaskTypes,
      IProgress<DeviceSyncProgress> progress = null)
    {
      Exception ex = (Exception) null;
      using (CancellationTokenSource syncTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
      {
        this.LogSyncStart();
        using (CancellationTokenSource listenForCancelFromForegroundCancellationTokenSource = new CancellationTokenSource())
        {
          Task listeningForCancelFromForeground = this.StartListeningForCancelFromForegroundAsync(syncType, syncTokenSource, listenForCancelFromForegroundCancellationTokenSource.Token);
          try
          {
            BandSyncServiceBase.Logger.Info((object) ("Memory limit: " + CommonFormatter.FormatBytes(this.environmentService.AppMemoryUsageLimit)));
            BandSyncServiceBase.Logger.Info((object) this.environmentService.BatteryStatus);
            if (await this.CanExecuteSyncAsync(syncType, syncTokenSource.Token))
            {
              await this.TimeAndRunPeriodicTasksAsync(syncType, primaryTaskTypes, secondaryTaskTypes, syncTokenSource.Token, progress);
              this.SaveLastSyncTime();
              this.currentSyncResult.SyncElapsed = (long) (this.dateTimeService.Now - this.currentSyncResult.StartTime).TotalMilliseconds;
              this.LogSyncEnd("completed");
            }
          }
          catch (OperationCanceledException ex1)
          {
            this.LogSyncEnd("canceled");
            ex = (Exception) ex1;
          }
          catch (Exception ex2)
          {
            this.LogSyncEnd("exception");
            BandSyncServiceBase.Logger.Error(ex2, "sync failed.");
            ex = ex2;
          }
          listenForCancelFromForegroundCancellationTokenSource.Cancel();
          await listeningForCancelFromForeground;
          listeningForCancelFromForeground = (Task) null;
        }
      }
      BandSyncServiceBase.Logger.Info((object) "<END> listening for cancellation");
      this.isSyncing = false;
      if (ex != null)
        throw ex;
    }

    private async Task TimeAndRunPeriodicTasksAsync(
      SyncType syncType,
      IEnumerable<Type> primaryTaskTypes,
      IEnumerable<Type> secondaryTaskTypes,
      CancellationToken cancellationToken,
      IProgress<DeviceSyncProgress> progress = null)
    {
      bool isBackgroundSync = syncType == SyncType.Background;
      using (ITimedTelemetryEvent timedEvent = CommonTelemetry.TimeSync(syncType))
      {
        try
        {
          await this.RunPeriodicTasksAsync(syncType, primaryTaskTypes, cancellationToken, progress).ConfigureAwait(false);
          timedEvent.SetStatus(true);
          timedEvent.AddMetric("Bytes Uploaded", (double) this.sensorBytesUploaded);
          timedEvent.AddMetric(isBackgroundSync ? "BackgroundDownloadKbytesPerSecondFromDevice" : "ForegroundDownloadKbytesPerSecondFromDevice", this.downloadKbytesPerSecondFromDevice);
          timedEvent.AddMetric(isBackgroundSync ? "BackgroundUploadKbytesPerSecondFromCloud" : "ForegroundUploadKbytesPerSecondFromCloud", this.uploadKbytesPerSecondFromCloud);
          this.StartSecondaryTasks(secondaryTaskTypes);
          this.currentSyncResult.SyncElapsed = (long) (this.dateTimeService.Now - this.currentSyncResult.StartTime).TotalMilliseconds;
          this.currentSyncResult.FetchLogFromBand = (long) ((double) this.sensorBytesUploaded / this.downloadKbytesPerSecondFromDevice);
          this.currentSyncResult.SendLogToCloud = (long) ((double) this.sensorBytesUploaded / this.uploadKbytesPerSecondFromCloud);
        }
        catch (Exception ex) //when (
        {
          // ISSUE: unable to correctly present filter
          Func<bool> func = (Func<bool>) (() =>
          {
            timedEvent.SetStatus(false);
            return false;
          });
          if (ex.CatchWhen(func))
          {
            //TODO
            //SuccessfulFiltering;
          }
          else
            throw;
        }
        //)
        //{
        //}
      }
    }

    private async void StartSecondaryTasks(IEnumerable<Type> taskTypes)
    {
      using (CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromMinutes(1.0)))
        await Task.Run((Func<Task>) (async () =>
        {
          BandSyncServiceBase.Logger.Debug((object) "<START> secondary sync task: update and sync");
          try
          {
            Stopwatch secondaryStopwatch = Stopwatch.StartNew();
            await this.RunPeriodicTasksAsync(SyncType.Background, taskTypes, cts.Token);
            secondaryStopwatch.Stop();
            this.currentSyncResult.TilesUpdate = secondaryStopwatch.ElapsedMilliseconds;
            BandSyncServiceBase.Logger.Debug((object) "<END> secondary sync task: update and sync");
            secondaryStopwatch = (Stopwatch) null;
          }
          catch (Exception ex)
          {
            BandSyncServiceBase.Logger.Error((object) "One or more exceptions occurred during secondary sync tasks.", ex);
          }
          finally
          {
            this.debugReporterService.RecordSyncResult(this.currentSyncResult);
            this.isSyncing = false;
          }
        }));
    }

    private async Task RunPeriodicTasksAsync(
      SyncType syncType,
      IEnumerable<Type> taskTypes,
      CancellationToken cancellationToken,
      IProgress<DeviceSyncProgress> progress = null)
    {
      BandSyncServiceBase.Logger.Info("Memory use before running periodic tasks: {0}", (object) this.GetMemoryUsage());
      List<Exception> exceptions = new List<Exception>();
      double totalPercentage = 10.0;
      double currentPercentage = 0.0;
      ProgressStage lastSyncStage = ProgressStage.Syncing;
      foreach (Type periodicTaskType in taskTypes)
      {
        cancellationToken.ThrowIfCancellationRequested();
        try
        {
          IBandSyncTask periodicTask = (IBandSyncTask) ServiceLocator.Current.GetInstance(periodicTaskType);
          await periodicTask.RunAsync(new BandSyncContext(syncType, (Action<SyncTaskProgress>) (taskProgress =>
          {
            if (taskProgress.ProgressWeight <= 0.0)
              return;
            currentPercentage = taskProgress.ProgressWeight * taskProgress.PercentageComplete;
            lastSyncStage = taskProgress.Stage;
            this.BroadcastSyncBandStateChanged(progress, taskProgress.Stage, Math.Min(totalPercentage + currentPercentage, 100.0));
          })), cancellationToken, this.currentSyncResult);
          if (periodicTask is LogSyncTask logSyncTask4)
          {
            this.sensorBytesUploaded = logSyncTask4.BytesUploaded;
            this.downloadKbytesPerSecondFromDevice = logSyncTask4.DownloadKbytesPerSecondFromDevice;
            this.uploadKbytesPerSecondFromCloud = logSyncTask4.UploadKbytesPerSecondFromCloud;
          }
          totalPercentage += currentPercentage;
          this.BroadcastSyncBandStateChanged(progress, lastSyncStage, Math.Min(totalPercentage, 100.0));
          periodicTask = (IBandSyncTask) null;
        }
        catch (Exception ex) when (syncType == SyncType.Background)
        {
          exceptions.Add(ex);
        }
        BandSyncServiceBase.Logger.Info("Memory use after task {0}: {1}", (object) periodicTaskType.Name, (object) this.GetMemoryUsage());
      }
      cancellationToken.ThrowIfCancellationRequested();
      if (exceptions.Count > 0)
        throw new AggregateException("Periodic task(s) failed.", (IEnumerable<Exception>) exceptions);
    }

    private void LogSyncStart()
    {
      ApplicationTelemetry.SetSyncInProgress(true);
      this.Stopwatch = Stopwatch.StartNew();
      BandSyncServiceBase.Logger.Info((object) "<START> scheduled task");
      this.currentSyncResult.StartTime = this.dateTimeService.Now;
      this.debugReporterService.ResetSdeTotalElapsed();
    }

    private void LogSyncEnd(string reason)
    {
      this.Stopwatch.Stop();
      BandSyncServiceBase.Logger.Info("<END> scheduled task (reason={0}, elapsed={1})", (object) reason, (object) this.Stopwatch.Elapsed);
      ApplicationTelemetry.SetSyncInProgress(false);
    }

    private async Task<bool> CheckIfFirmwareIsSupportedAsync(CancellationToken cancellationToken) => !await this.firmwareUpdateService.CheckForFirmwareUpdateAsync(cancellationToken);

    private async Task<bool> CheckIfFirmwareMessageIsSentAsync(
      CancellationToken cancellationToken)
    {
      return await this.isoObjectStore.ReadObjectAsync<bool>("IsFirmwareMessageSent", cancellationToken);
    }

    private async Task StartListeningForCancelFromForegroundAsync(
      SyncType syncType,
      CancellationTokenSource syncTokenSource,
      CancellationToken waitToken)
    {
      if (syncType != SyncType.Background || !this.environmentService.SupportsBackgroundSyncCancellation)
        return;
      BandSyncServiceBase.Logger.Info((object) "<START> listening for cancellation");
      EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset, "KApp.BackgroundCancel");
      waitHandle.Reset();
      try
      {
        await waitHandle.WaitAsync(waitToken).ConfigureAwait(false);
      }
      catch (OperationCanceledException ex)
      {
      }
      if (waitToken.IsCancellationRequested)
        return;
      BandSyncServiceBase.Logger.Info((object) "cancel signal from foreground detected, cancelling background sync");
      syncTokenSource.Cancel();
    }

    internal async Task HandleFirmwareNotSupportedAsync(CancellationToken cancellationToken)
    {
      if (await this.CheckIfFirmwareMessageIsSentAsync(cancellationToken))
        return;
      BandSyncServiceBase.Logger.Info((object) "<FLAG> sending out-of-date firmware detected in background toast");
      ToastNotificationData data = new ToastNotificationData(AppResources.ApplicationTitle, AppResources.PendingFirmwareUpdateToast, ToastNotificationIcon.Warning)
      {
        Summary = AppResources.PendingFirmwareUpdateSummary,
        Navigation = (IToastNotificationNavigation) new ToastNotificationNavigationHome()
      };
      this.toastNotificationService.ShowToast(HealthAppConstants.ToastNotificationIds.FirmwareUpdateRequired, data);
      await this.isoObjectStore.WriteObjectAsync((object) true, "IsFirmwareMessageSent", cancellationToken);
    }

    public async Task<bool> CanExecuteSyncAsync(
      SyncType syncType,
      CancellationToken cancellationToken)
    {
      bool success = true;
      try
      {
        if (!this.OobeCompleted)
          throw new InvalidOperationException("oobe is not complete");
        if ((syncType == SyncType.Background || syncType == SyncType.BandEvent) && this.WifiOnlyWithNoWifi)
          throw new InvalidOperationException("wifi only enabled but no wifi connection is available");
        if (!this.networkService.IsInternetAvailable)
          throw new InternetException("Internet is not available.");
        if (!this.CanExecuteSyncOnPlatform())
          throw new InvalidOperationException("cannot execute sync");
        await BandSyncServiceBase.EnforceSingleDevicePolicyAsync(cancellationToken);
        if (!await this.CheckIfFirmwareIsSupportedAsync(cancellationToken))
        {
          this.LogSyncEnd("firmware on device not supported");
          await this.HandleFirmwareNotSupportedAsync(cancellationToken);
          throw new InvalidOperationException("firmware on device is not supported");
        }
      }
      catch (Exception ex) when (ex.CatchWhen((Func<bool>) (() =>
      {
        BandSyncServiceBase.Logger.Error((object) "Environment is not available", ex);
        return syncType == SyncType.Background;
      })))
      {
        success = false;
      }
      return success;
    }

    public abstract bool CanExecuteSyncOnPlatform();

    private static async Task EnforceSingleDevicePolicyAsync(CancellationToken cancellationToken)
    {
      IBandConnectionFactory instance = ServiceLocator.Current.GetInstance<IBandConnectionFactory>();
      cancellationToken.ThrowIfCancellationRequested();
      CancellationToken cancellationToken1 = cancellationToken;
      using (IBandConnection cargoConnection = await instance.CreateConnectionAsync(cancellationToken1))
        await cargoConnection.CheckConnectionWorkingAsync(cancellationToken).ConfigureAwait(false);
    }

    private string GetMemoryUsage() => CommonFormatter.FormatBytes(this.environmentService.AppMemoryUsage);

    private void SaveLastSyncTime()
    {
      BandSyncServiceBase.Logger.Info((object) "<START> save last sync time to isolated storage");
      this.configProvider.SetDateTimeOffset("LastSyncTime", this.dateTimeService.Now);
      BandSyncServiceBase.Logger.Info((object) "<END>  save last sync time to isolated storage");
    }

    private void BroadcastSyncBandStateChanged(
      IProgress<DeviceSyncProgress> progress,
      ProgressStage stage,
      double percentComplete)
    {
      progress?.Report(new DeviceSyncProgress(this.isSyncing, stage, percentComplete));
    }

    private static Dictionary<SyncTaskType, IList<Type>> GroupAndFilterTasks(
      Type[] periodicTaskTypes,
      SyncType syncType)
    {
      BandSyncServiceBase.Logger.Info((object) string.Format("Grouping and Filtering {0} sync tasks:", new object[1]
      {
        (object) syncType
      }));
      SyncTaskType syncTaskType;
      switch (syncType)
      {
        case SyncType.Background:
          syncTaskType = SyncTaskType.Background;
          break;
        case SyncType.BandEvent:
          syncTaskType = SyncTaskType.BandEvent;
          break;
        default:
          syncTaskType = SyncTaskType.Foreground;
          break;
      }
      Dictionary<SyncTaskType, IList<Type>> dictionary = new Dictionary<SyncTaskType, IList<Type>>();
      List<Type> typeList1 = new List<Type>();
      List<Type> typeList2 = new List<Type>();
      foreach (Type periodicTaskType in periodicTaskTypes)
      {
        SyncTaskAttribute syncTaskAttribute = CustomAttributeExtensions.GetCustomAttributes(periodicTaskType.GetTypeInfo(), typeof (SyncTaskAttribute), true).Cast<SyncTaskAttribute>().FirstOrDefault<SyncTaskAttribute>();
        if (syncTaskAttribute == null)
          BandSyncServiceBase.Logger.Error((object) string.Format("Skipping Sync Task of type '{0}'.  Attribute metadata cannot be null", new object[1]
          {
            (object) periodicTaskType.Name
          }));
        else if (syncTaskAttribute.TaskType.HasFlag((Enum) syncTaskType))
        {
          if (syncTaskAttribute.TaskType.HasFlag((Enum) SyncTaskType.Primary))
            typeList1.Add(periodicTaskType);
          else
            typeList2.Add(periodicTaskType);
        }
      }
      foreach (Type type in typeList1)
        BandSyncServiceBase.Logger.Info((object) string.Format("  Primary: {0}", new object[1]
        {
          (object) type.Name
        }));
      foreach (Type type in typeList2)
        BandSyncServiceBase.Logger.Info((object) string.Format("  Secondary: {0}", new object[1]
        {
          (object) type.Name
        }));
      dictionary.Add(SyncTaskType.Primary, (IList<Type>) typeList1);
      dictionary.Add(SyncTaskType.Secondary, (IList<Type>) typeList2);
      return dictionary;
    }

    public static void CancelBackgroundSyncTask()
    {
      EventWaitHandle result;
      if (!EventWaitHandle.TryOpenExisting("KApp.BackgroundCancel", out result))
        return;
      result.Set();
    }
  }
}
